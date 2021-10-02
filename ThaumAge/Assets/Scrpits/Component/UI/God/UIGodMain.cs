using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class UIGodMain : UIGameCommonNormal
{

    protected List<ItemsInfoBean> listItemsInfo = new List<ItemsInfoBean>();

    public override void Awake()
    {
        base.Awake();
        ui_ItemList.AddCellListener(OnCellForItem);
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        ui_Shortcuts.RefreshUI();
    }

    public override void OpenUI()
    {
        base.OpenUI();
        InitData();
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    public void InitData()
    {
        listItemsInfo = ItemsHandler.Instance.manager.GetAllItemsInfo();
        ui_ItemList.SetCellCount(listItemsInfo.Count);
    }

    /// <summary>
    /// 单个数据回调
    /// </summary>
    /// <param name="itemCell"></param>
    public void OnCellForItem(ScrollGridCell itemCell)
    {
        UIViewItemContainer viewItemContainer = itemCell.GetComponent<UIViewItemContainer>();
        ItemsInfoBean itemsInfo = listItemsInfo[itemCell.index];
        ItemsBean itemsData = new ItemsBean();
        itemsData.itemsId = itemsInfo.id;
        itemsData.number = int.MaxValue;
        viewItemContainer.SetData(itemsData, new Vector2Int(itemCell.index, 0));
    }
}