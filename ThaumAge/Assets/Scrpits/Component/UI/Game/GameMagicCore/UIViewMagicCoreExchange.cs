using UnityEditor;
using UnityEngine;

public partial class UIViewMagicCoreExchange : BaseUIView
{
    protected ItemsBean itemData;


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
        ItemMetaWand itemMetaWand = itemData.GetMetaData<ItemMetaWand>();
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
        ui_MagicCoreListContainer.DestroyAllChild(true);
        MagicInstrumentInfoBean magicInstrumentInfo = GameInfoHandler.Instance.manager.GetMagicInstrumentInfo(itemMetaWand.capId);
        for (int i = 0; i < magicInstrumentInfo.magic_core_num; i++)
        {
            GameObject objItem = Instantiate(ui_MagicCoreListContainer.gameObject, ui_ViewItemContainer.gameObject);
            objItem.ShowObj(true);
            UIViewItemContainer uiViewItemContainer = objItem.GetComponent<UIViewItemContainer>();

            ItemsBean itemData;
            if (itemMetaWand.listMagicCore.IsNull())
            {
                itemData = new ItemsBean();
            }
            else
            {
                if (i >= itemMetaWand.listMagicCore.Count)
                {
                    itemData = new ItemsBean();
                }
                else
                {
                    itemData = itemMetaWand.listMagicCore[i];
                }
            }
            uiViewItemContainer.SetViewItemByData(UIViewItemContainer.ContainerType.Other, itemData);
            uiViewItemContainer.SetLimitType(ItemsTypeEnum.MagicCore);
        }
    }
}