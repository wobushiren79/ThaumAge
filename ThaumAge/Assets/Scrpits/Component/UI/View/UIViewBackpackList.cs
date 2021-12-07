using UnityEditor;
using UnityEngine;

public partial class UIViewBackpackList : BaseUIView
{
     protected ItemsBean[] listBackpack;
     /// <summary>
     /// 初始化数据
     /// </summary>
    public void InitData()
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        listBackpack = userData.listBackpack;
        ui_ItemList.SetCellCount(listBackpack.Length);
    }

    /// <summary>
    /// 单个数据回调
    /// </summary>
    /// <param name="itemCell"></param>
    public void OnCellForItem(ScrollGridCell itemCell)
    {
        UIViewItemContainer viewItemContainer = itemCell.GetComponent<UIViewItemContainer>();
        ItemsBean itemsData = listBackpack[itemCell.index];
        viewItemContainer.SetData(itemsData, new Vector2Int(itemCell.index, 0));
    }
}