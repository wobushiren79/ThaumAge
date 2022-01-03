using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

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

    public override void OnInputActionForStarted(InputActionUIEnum inputType)
    {
        base.OnInputActionForStarted(inputType);
        switch (inputType) 
        {
            case InputActionUIEnum.F12:
            case InputActionUIEnum.ESC:
                HandleForBackMain();
                break;
        }
    }

    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if (viewButton == ui_Time_1)
        {
            HandleForTimeChange(0, 0);
        }
        else if (viewButton == ui_Time_2)
        {
            HandleForTimeChange(6, 0);
        }
        else if (viewButton == ui_Time_3)
        {
            HandleForTimeChange(12, 0);
        }
        else if (viewButton == ui_Time_4)
        {
            HandleForTimeChange(18, 0);
        }
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    public void InitData()
    {
        listItemsInfo.Clear();
        List<ItemsInfoBean> listAllItemsInfo = ItemsHandler.Instance.manager.GetAllItemsInfo();
        for (int i = 0; i < listAllItemsInfo.Count; i++)
        {
            ItemsInfoBean itemData = listAllItemsInfo[i];
            if (itemData.id == 0)
                continue;
            listItemsInfo.Add(listAllItemsInfo[i]);
        }
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
        itemsData.itemId = itemsInfo.id;
        itemsData.number = int.MaxValue;
        viewItemContainer.SetData(UIViewItemContainer.ContainerType.God, itemsData, itemCell.index);
    }

    /// <summary>
    /// 处理时间改变
    /// </summary>
    public void HandleForTimeChange(int hour, int min)
    {
        GameTimeHandler.Instance.SetGameTime(hour, min);
    }
}