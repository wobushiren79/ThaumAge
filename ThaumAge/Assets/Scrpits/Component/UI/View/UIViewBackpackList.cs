using UnityEditor;
using UnityEngine;

public partial class UIViewBackpackList : BaseUIView
{
     protected ItemsBean[] listBackpack;

    public override void Show()
    {
        base.Show();
        InitData();
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    public void InitData()
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        listBackpack = userData.listBackpack;
        ui_ItemList.AddCellListener(OnCellForItem);
        ui_ItemList.SetCellCount(listBackpack.Length);
    }

    /// <summary>
    /// 单个数据回调
    /// </summary>
    /// <param name="itemCell"></param>
    public void OnCellForItem(ScrollGridCell itemCell)
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        UIViewItemContainer viewItemContainer = itemCell.GetComponent<UIViewItemContainer>();
        ItemsBean itemsData = userData.GetItemsFromBackpack(itemCell.index);
        viewItemContainer.SetData(UIViewItemContainer.ContainerType.Backpack, itemsData, itemCell.index);
    }
}