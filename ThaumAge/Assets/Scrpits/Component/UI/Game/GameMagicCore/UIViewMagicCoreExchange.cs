using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class UIViewMagicCoreExchange : BaseUIView
{
    protected ItemsBean itemData;
    protected ItemMetaWand itemMetaWand;

    protected List<UIViewItemContainer> listMagicCoreItem = new List<UIViewItemContainer>();
    public override void Awake()
    {
        base.Awake();
        ui_ViewItemContainer.ShowObj(false);
    }

    public override void OpenUI()
    {
        base.OpenUI();
    }


    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="worldPosition"></param>
    public void SetData(ItemsBean itemData)
    {
        this.itemData = itemData;
        itemMetaWand = itemData.GetMetaData<ItemMetaWand>();
        SetInstrumentIcon();
        SetListMagicCore(itemMetaWand);
    }

    /// <summary>
    /// 设置法器图标
    /// </summary>
    public void SetInstrumentIcon()
    {
        ItemsHandler.Instance.SetItemsIconById(ui_InstrumentIcon, itemData.itemId, itemData);
    }

    /// <summary>
    /// 设置法术核心
    /// </summary>
    public void SetListMagicCore(ItemMetaWand itemMetaWand)
    {
        listMagicCoreItem.Clear();
        ui_MagicCoreListContainer.DestroyAllChild(true);
        MagicInstrumentInfoBean magicInstrumentInfo = MagicInstrumentInfoCfg.GetItemDataByItemId(itemMetaWand.capId);
        if (magicInstrumentInfo == null)
        {
            Debug.LogError($"没有找到杖端ID为{itemMetaWand.capId}的法器数据");
            return;
        }
        for (int i = 0; i < magicInstrumentInfo.magic_core_num; i++)
        {
            GameObject objItem = Instantiate(ui_MagicCoreListContainer.gameObject, ui_ViewItemContainer.gameObject);
            objItem.ShowObj(true);
            UIViewItemContainer uiViewItemContainer = objItem.GetComponent<UIViewItemContainer>();

            ItemsBean itemMagicCoreData;
            if (itemMetaWand.listMagicCore.IsNull())
            {
                itemMagicCoreData = new ItemsBean();
            }
            else
            {
                if (i >= itemMetaWand.listMagicCore.Count)
                {
                    itemMagicCoreData = new ItemsBean();
                }
                else
                {
                    itemMagicCoreData = itemMetaWand.listMagicCore[i];
                }
            }
            uiViewItemContainer.SetViewItemByData(UIViewItemContainer.ContainerType.Other, itemMagicCoreData);
            uiViewItemContainer.SetLimitType(ItemsTypeEnum.MagicCore);
            uiViewItemContainer.SetCallBackForSetViewItem(CallBackForMagicItemChange);
            listMagicCoreItem.Add(uiViewItemContainer);
        }
    }

    /// <summary>
    /// 改变核心回调
    /// </summary>
    public void CallBackForMagicItemChange(UIViewItemContainer changeViewItem, ItemsBean changeItemData)
    {
        if (itemMetaWand.listMagicCore == null)
            itemMetaWand.listMagicCore = new List<ItemsBean>();
        itemMetaWand.listMagicCore.Clear();

        for (int i = 0; i < listMagicCoreItem.Count; i++)
        {
            var tempItem = listMagicCoreItem[i];
            if (tempItem.itemsData != null)
            {
                itemMetaWand.listMagicCore.Add(tempItem.itemsData);
            }
        }

        itemData.SetMetaData(itemMetaWand);
        //刷新UI
        this.TriggerEvent(EventsInfo.UIViewShortcutsMagic_InitData);
    }
}