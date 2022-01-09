using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class UIViewBackpackList : BaseUIView
{
    protected ItemsBean[] listBackpack;

    public override void OpenUI()
    {
        base.OpenUI();
        InitData();
    }

    public bool AddItems(ItemsBean itemData)
    {
        return false;

    }

    /// <summary>
    /// 增加道具
    /// </summary>
    /// <param name="uiViewItem"></param>
    public bool AddItems(UIViewItem uiViewItem)
    {
        //首先直接在显示的list中搜索空位
        List<GameObject> listCellObj = ui_ItemList.GetAllCellObj();
        for (int i = 0; i < listCellObj.Count; i++)
        {
            GameObject itemObj = listCellObj[i];
            UIViewItemContainer itemContainer = itemObj.GetComponent<UIViewItemContainer>();
            //如果有容器VIEW 并且里面没有东西
            if (itemContainer != null && itemContainer.GetViewItem() == null)
            {
                uiViewItem.ExchangeItemForContainer(itemContainer);
                return true;
            }
        }
        //如果不成功则直接查询整个listBackpack
        for (int i = 0; i < listBackpack.Length; i++)
        {
            ItemsBean itemData = listBackpack[i];
            if (itemData == null || itemData.itemId == 0)
            {
                itemData.itemId = uiViewItem.itemId;
                itemData.number = uiViewItem.itemNumber;
                itemData.meta = uiViewItem.meta;
                Destroy(uiViewItem.gameObject);
                return true;
            }
        }
        return false;
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