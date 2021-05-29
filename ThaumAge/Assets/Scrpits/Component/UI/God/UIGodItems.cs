using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UIGodItems : BaseUIComponent
{
    public ScrollGridVertical ui_ItemList;

    protected List<BlockInfoBean> listBlockInfoData = new List<BlockInfoBean>();

    public override void Awake()
    {
        base.Awake();
        ui_ItemList.AddCellListener(OnCellForItem);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        GameControlHandler.Instance.manager.controlForPlayer.EnabledControl(false);
        GameControlHandler.Instance.manager.controlForCamera.EnabledControl(false);
        InitData();
        
    }

    public override void CloseUI()
    {
        base.CloseUI();
        GameControlHandler.Instance.manager.controlForPlayer.EnabledControl(true);
        GameControlHandler.Instance.manager.controlForCamera.EnabledControl(true);
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    public void InitData()
    {
        listBlockInfoData = BlockHandler.Instance.manager.GetAllBackInfo();
        ui_ItemList.SetCellCount(listBlockInfoData.Count);
    }
 
    /// <summary>
    /// 单个数据回调
    /// </summary>
    /// <param name="itemCell"></param>
    public void OnCellForItem(ScrollGridCell itemCell)
    {
        UIViewItemContainer viewItemContainer = itemCell.GetComponent<UIViewItemContainer>();
        BlockInfoBean blockInfo = listBlockInfoData[itemCell.index];
        viewItemContainer.SetData(blockInfo);
    }
}