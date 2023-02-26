using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public partial class UIViewItemContainer : BaseUIView
{
    //容器类型
    public enum ContainerType
    {
        None,
        Shortcuts,//快捷栏
        Equip,//装备
        Backpack,//背包
        Chest,//箱子
        Bag,//包（用于NPC的包 或者傀儡的包）
        God,//列表 GOD模式
        Furnaces,//熔炉
        ItemsTransition,//转换
        Other,//其他
    }

    [Header("限制的物品类型")]
    public List<ItemsTypeEnum> listLimitTypes;

    //容器类型
    public ContainerType containerType;
    //位置
    public int viewIndex;
    //道具
    protected UIViewItem currentViewItem;
    //容器指向的数据
    [HideInInspector]
    public ItemsBean itemsData = new ItemsBean();

    //放置新道具回调
    protected Action<UIViewItemContainer, ItemsBean> callBackForSetViewItem;

    protected float timeForAddViewItem = 0.2f;
    protected float timeForRemoveViewItem = 0.2f;

    public override void Awake()
    {
        base.Awake();
        ui_ViewItemModel.ShowObj(false);
        SetSelectState(false);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        EventHandler.Instance.RegisterEvent<ItemsBean>(EventsInfo.ItemsBean_MetaChange, CallBackForItemsDataMetaChange);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        EventHandler.Instance.UnRegisterEvent<ItemsBean>(EventsInfo.ItemsBean_MetaChange, CallBackForItemsDataMetaChange);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        callBackForSetViewItem = null;
    }

    /// <summary>
    /// 道具数据发生改变的回调
    /// </summary>
    public void CallBackForItemsDataMetaChange(ItemsBean itemsData)
    {
        if (itemsData != this.itemsData)
            return;
        SetViewItemByData(itemsData, true);
    }

    /// <summary>
    /// 设置新道具回调
    /// </summary>
    /// <param name="callBackForSetViewItem"></param>
    public void SetCallBackForSetViewItem(Action<UIViewItemContainer, ItemsBean> callBackForSetViewItem)
    {
        this.callBackForSetViewItem = callBackForSetViewItem;
    }

    /// <summary>
    /// 设置限制放置的道具类型
    /// </summary>
    public void SetLimitTypes(List<ItemsTypeEnum> listLimitTypes)
    {
        this.listLimitTypes = listLimitTypes;
    }
    public void SetLimitType(ItemsTypeEnum limitType)
    {
        SetLimitTypes(new List<ItemsTypeEnum>() { limitType });
    }
    public void SetLimitType(EquipTypeEnum equipType)
    {
        ItemsTypeEnum itemsType = CharacterEquipBean.EquipTypeEnumToItemsType(equipType);
        SetLimitType(itemsType);
    }

    /// <summary>
    /// 检测是否能放置该道具
    /// </summary>
    /// <param name="itemsType"></param>
    /// <returns>true能设置 false不能设置</returns>
    public bool CheckCanSetItem(ItemsTypeEnum itemsType)
    {
        if (listLimitTypes.IsNull())
        {
            return true;
        }
        for (int i = 0; i < listLimitTypes.Count; i++)
        {
            ItemsTypeEnum limitType = listLimitTypes[i];
            if (itemsType == limitType)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 清空容器
    /// </summary>
    public void ClearViewItem(bool isDestroyViewItem = false, bool isCallBack = true)
    {
        //删除原来存在的
        if (isDestroyViewItem && currentViewItem != null && currentViewItem.gameObject != null)
        {
            currentViewItem.StopAnim();
            DestroyImmediate(currentViewItem.gameObject);
        }
        this.currentViewItem = null;
        itemsData.ClearData();

        //设置展示信息
        ui_ViewItemContainer.SetItemData(itemsData);
        //设置回调
        if (isCallBack)
        {
            callBackForSetViewItem?.Invoke(this, itemsData);
            this.TriggerEvent(EventsInfo.UIViewItemContainer_ItemChange, this, itemsData.itemId);
        }
    }

    /// <summary>
    /// 获取道具
    /// </summary>
    /// <returns></returns>
    public UIViewItem GetViewItem()
    {
        return currentViewItem;
    }

    /// <summary>
    /// 设置容器道具
    /// </summary>
    /// <param name="uiView"></param>
    public bool SetViewItemByView(UIViewItem uiView)
    {
        this.currentViewItem = uiView;
        this.currentViewItem.originalParent = this;
        this.currentViewItem.transform.SetParent(rectTransform);

        itemsData.itemId = uiView.itemId;
        itemsData.number = uiView.itemNumber;
        itemsData.meta = uiView.meta;

        callBackForSetViewItem?.Invoke(this, itemsData);
        this.TriggerEvent(EventsInfo.UIViewItemContainer_ItemChange, this, itemsData.itemId);
        //设置展示信息
        ui_ViewItemContainer.SetItemData(itemsData);

        return true;
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="itemsData"></param>
    /// <param name="viewIndex"></param>
    public void SetViewItemByData(ContainerType containerType, ItemsBean itemsData, int viewIndex = 0)
    {
        this.containerType = containerType;
        this.viewIndex = viewIndex;
        SetViewItemByData(itemsData);
    }

    /// <summary>
    /// 设置容器道具 用于初始化
    /// </summary>
    /// <param name="itemsData"></param>
    public void SetViewItemByData(ItemsBean itemsData, bool isCallBack = true)
    {
        if (itemsData == null)
            itemsData = new ItemsBean();
        this.itemsData = itemsData;
        //设置展示信息
        ui_ViewItemContainer.SetItemData(itemsData);

        //如果没有东西，则删除原来存在的
        if (itemsData.itemId == 0)
        {
            if (currentViewItem != null && currentViewItem.gameObject != null)
            {
                currentViewItem.StopAnim();
                DestroyImmediate(currentViewItem.gameObject);
            }
            currentViewItem = null;
        }
        else
        {
            //如果有东西，则先实例化再设置数据
            if (currentViewItem == null)
            {
                GameObject obj = Instantiate(gameObject, ui_ViewItemModel.gameObject);
                obj.name = "ViewItem";
                currentViewItem = obj.GetComponent<UIViewItem>();
                currentViewItem.originalParent = this;
                currentViewItem.transform.position = ui_ViewItemModel.transform.position;
                currentViewItem.transform.localScale = ui_ViewItemModel.transform.localScale;
                currentViewItem.transform.rotation = ui_ViewItemModel.transform.rotation;
            }
            currentViewItem.SetData(itemsData.itemId, itemsData.number, itemsData.meta);
        }

        if (isCallBack)
        {
            callBackForSetViewItem?.Invoke(this, itemsData);
            this.TriggerEvent(EventsInfo.UIViewItemContainer_ItemChange, this, itemsData.itemId);
        }
    }


    /// <summary>
    /// 设置选择状态
    /// </summary>
    /// <param name="isSelect"></param>
    public void SetSelectState(bool isSelect)
    {
        if (ui_Select == null)
            return;
        if (isSelect)
        {
            ui_Select.ShowObj(true);
        }
        else
        {
            ui_Select.ShowObj(false);
        }
    }
}