using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class UIBuildingEditorCreate : BaseUIComponent
{
    protected List<BlockInfoBean> listBlockInfo;
    public override void Awake()
    {
        base.Awake();
        ui_BlockList.AddCellListener(OnCellChangeForBlockSelect);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        InitData();
    }

    public void InitData()
    {
        listBlockInfo = new List<BlockInfoBean>();
        BlockInfoBean[] arrayBlockInfo = BlockHandler.Instance.manager.GetAllBackInfo();
        //添加不为NULL的数据
        for (int i = 0; i < arrayBlockInfo.Length; i++)
        {
            var itemInfo =  arrayBlockInfo[i];
            if (itemInfo != null)
            {
                listBlockInfo.Add(itemInfo);
            }
        }
        ui_BlockList.SetCellCount(listBlockInfo.Count);
    }

    /// <summary>
    /// 选择方块列表设置
    /// </summary>
    /// <param name="itemCell"></param>
    public void OnCellChangeForBlockSelect(ScrollGridCell itemCell)
    {
        UIItemBuildingEditorCreateBlockSelect itemView = itemCell.GetComponent<UIItemBuildingEditorCreateBlockSelect>();
        itemView.SetData(listBlockInfo[itemCell.index]);
    }
}