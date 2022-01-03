using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    protected ItemsBean itemsData;

    //放置新道具回调
    protected Action<UIViewItemContainer, long> callBackForSetViewItem;

    public override void Awake()
    {
        base.Awake();
        ui_Hint.ShowObj(false);
        ui_ViewItemModel.ShowObj(false);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        callBackForSetViewItem = null;
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
    public void SetCallBackForSetViewItem(Action<UIViewItemContainer, long> callBackForSetViewItem)
    {
        this.callBackForSetViewItem += callBackForSetViewItem;
    }

    /// <summary>
    /// 设置提示文本
    /// </summary>
    /// <param name="hintText"></param>
    public void SetHintText(string hintText)
    {
        ui_Hint.ShowObj(true);
        ui_Hint.text = hintText;
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
        ItemsTypeEnum itemsType = UserEquipBean.EquipTypeEnumToItemsType(equipType);
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
        ui_ViewItemContainer.SetItemId(itemsData.itemId);
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

        callBackForSetViewItem?.Invoke(this, itemsData.itemId);

        //设置展示信息
        ui_ViewItemContainer.SetItemId(itemsData.itemId);

        return true;
    }

    /// <summary>
    /// 设置容器道具 用于初始化
    /// </summary>
    /// <param name="itemsData"></param>
    public void SetViewItem(ItemsBean itemsData)
    {
        //设置展示信息
        ui_ViewItemContainer.SetItemId(itemsData.itemId);

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

        callBackForSetViewItem?.Invoke(this, itemsData.itemId);
    }


    /// <summary>
    /// 设置选择状态
    /// </summary>
    /// <param name="isSelect"></param>
    public void SetSelectState(bool isSelect)
    {
        if (ui_IVBackground == null)
            return;
        if (isSelect)
        {
            ui_IVBackground.color = Color.green;
        }
        else
        {
            ui_IVBackground.color = Color.white;
        }
    }
}