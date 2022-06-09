using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class UIBuildingEditorCreate : BaseUIComponent
{
    protected List<BlockInfoBean> listBlockInfo;

    //选中的下标
    protected int indexSelect = 0;

    public override void Awake()
    {
        base.Awake();
        ui_BlockList.AddCellListener(OnCellChangeForBlockSelect);
        ui_CreateSelect.onValueChanged.AddListener((value)=> 
        {
            CallBackForToggleSelect(ui_CreateSelect,value);
        });
        ui_DestorySelect.onValueChanged.AddListener((value) =>
        {
            CallBackForToggleSelect(ui_DestorySelect, value);
        });
    }

    public override void OpenUI()
    {
        base.OpenUI();
        InitData();
        RegisterEvent<int>(EventsInfo.UIBuildingEditorCreate_SelectChange, CallBackForSelectChange);
    }

    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if (viewButton == ui_ClearAll)
        {
            OnClickForClearAll();
        }
        else if (viewButton == ui_CreateBuilding)
        {
            OnClickForCreateBuilding();
        }
    }

    public void InitData()
    {
        listBlockInfo = new List<BlockInfoBean>();
        BlockInfoBean[] arrayBlockInfo = BlockHandler.Instance.manager.GetAllBlockInfo();
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
        itemView.SetData(listBlockInfo[itemCell.index], itemCell.index, indexSelect);
    }

    /// <summary>
    /// 选择不同方块的回调
    /// </summary>
    public void CallBackForSelectChange(int changeIndex)
    {
        this.indexSelect = changeIndex;
        BuildingEditorHandler.Instance.manager.curSelectBlockInfo = listBlockInfo[indexSelect];
        //刷新所有
        ui_BlockList.RefreshAllCells();
    }

    /// <summary>
    /// 建造模式选择
    /// </summary>
    /// <param name="view"></param>
    /// <param name="value"></param>
    public void CallBackForToggleSelect(Toggle view, bool value)
    {
        if (view == ui_DestorySelect && value == true)
        {
            BuildingEditorHandler.Instance.manager.curCreateTyp = 1;
        }
        else if (view == ui_CreateSelect && value == true)
        {
            BuildingEditorHandler.Instance.manager.curCreateTyp = 0;
        }
    }

    /// <summary>
    /// 点击-清除所有方块
    /// </summary>
    public void OnClickForClearAll()
    {
        BuildingEditorHandler.Instance.ClearAllBlock();
    }

    /// <summary>
    /// 点击-创建建筑
    /// </summary>
    public void OnClickForCreateBuilding()
    {
        DialogBean dialogData = new DialogBean();
        dialogData.content = $"是否要创建ID为 {ui_BuildingIdEdit.text} 的建筑";
        dialogData.actionSubmit = (view, data) =>
        {

        };
;       UIDialogNormal uiDialog = UIHandler.Instance.ShowDialog<UIDialogNormal>(dialogData);
    }

}