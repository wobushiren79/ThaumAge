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
        Box,//箱子
        God,//列表 GOD模式
        Furnaces,//熔炉
        ItemsTransition,//转换
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
    public ItemsBean itemsData;

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
        if (currentViewItem == null)
            return;
        currentViewItem.SetData(itemsData.itemId, itemsData.number, itemsData.meta);
    }

    /// <summary>
    /// 增加道具
    /// </summary>
    public bool AddViewItem(ItemsBean itemsData,Vector3 createPosition)
    {
        //如果已经有一个了 则不能添加
        if (GetViewItem() != null)
            return false;
        SetData(containerType, itemsData, viewIndex);
        currentViewItem.rectTransform
            .DOAnchorPos(createPosition, timeForAddViewItem)
            .From()
            .SetEase(Ease.OutCubic);
        return true;
    }

    /// <summary>
    /// 移出道具
    /// </summary>
    /// <returns></returns>
    public bool RemoveViewItem()
    {
        //如果已经有一个了 则不能删除
        UIViewItem viewItem = GetViewItem();
        if (viewItem == null)
            return false;
        GameObject objViewItem = viewItem.gameObject;
        viewItem.transform
            .DOScale(0, timeForRemoveViewItem)
            .OnComplete(()=> 
            {
                DestroyImmediate(objViewItem);              
            });
        ClearViewItem();
        return true;
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="itemsData"></param>
    /// <param name="viewIndex"></param>
    public void SetData(ContainerType containerType, ItemsBean itemsData, int viewIndex = 0)
    {
        this.containerType = containerType;
        this.itemsData = itemsData;
        this.viewIndex = viewIndex;
        SetViewItem(itemsData);
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
    public void ClearViewItem()
    {
        this.currentViewItem = null;
        ClearItemsData();
    }

    /// <summary>
    /// 清空数据
    /// </summary>
    public void ClearItemsData()
    {
        itemsData.itemId = 0;
        itemsData.number = 0;
        itemsData.meta = null;

        //设置展示信息
        ui_ViewItemContainer.SetItemData(itemsData);
        //设置回调
        callBackForSetViewItem?.Invoke(this, itemsData);
        this.TriggerEvent(EventsInfo.UIViewItemContainer_ItemChange, this, itemsData.itemId);
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
    public bool SetViewItem(UIViewItem uiView)
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
    /// 设置容器道具 用于初始化
    /// </summary>
    /// <param name="itemsData"></param>
    public void SetViewItem(ItemsBean itemsData)
    {
        //设置展示信息
        ui_ViewItemContainer.SetItemData(itemsData);

        //如果没有东西，则删除原来存在的
        if (itemsData == null || itemsData.itemId == 0)
        {
            if (currentViewItem != null)
            {
                Destroy(currentViewItem.gameObject);
            }
            currentViewItem = null;
            return;
        }
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

        callBackForSetViewItem?.Invoke(this, itemsData);
        this.TriggerEvent(EventsInfo.UIViewItemContainer_ItemChange, this, itemsData.itemId);
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