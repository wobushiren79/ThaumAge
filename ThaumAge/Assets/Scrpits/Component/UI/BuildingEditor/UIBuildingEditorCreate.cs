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
        ui_CreateSelect.onValueChanged.AddListener((value) =>
        {
            CallBackForToggleSelect(ui_CreateSelect, value);
        });
        ui_DestorySelect.onValueChanged.AddListener((value) =>
        {
            CallBackForToggleSelect(ui_DestorySelect, value);
        });
        InitCreateType();
        InitBlockDirection();
    }

    public override void OpenUI()
    {
        base.OpenUI();
        InitData();
        RegisterEvent<int>(EventsInfo.UIBuildingEditorCreate_SelectChange, CallBackForSelectChange);
        BuildingEditorHandler.Instance.manager.isStartBuild = true;
    }

    public override void CloseUI()
    {
        base.CloseUI(); 
        BuildingEditorHandler.Instance.manager.isStartBuild = false;
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
        else if (viewButton== ui_LoadBuilding)
        {
            OnClickForLoadBuilding();
        }
    }

    public void InitData()
    {
        listBlockInfo = new List<BlockInfoBean>();
        BlockInfoBean[] arrayBlockInfo = BlockHandler.Instance.manager.GetAllBlockInfo();
        //添加不为NULL的数据
        for (int i = 0; i < arrayBlockInfo.Length; i++)
        {
            var itemInfo = arrayBlockInfo[i];
            if (itemInfo != null)
            {
                listBlockInfo.Add(itemInfo);
            }
        }
        ui_BlockList.SetCellCount(listBlockInfo.Count);
    }

    /// <summary>
    /// 初始化建筑选择
    /// </summary>
    public void InitCreateType()
    {
        int type = BuildingEditorHandler.Instance.manager.curCreateType;
        if (type == 0)
        {
            ui_CreateSelect.isOn = true;
        }
        else
        {
            ui_DestorySelect.isOn = true;
        }
    }

    /// <summary>
    /// 初始化方块方向
    /// </summary>
    public void InitBlockDirection()
    {
        List<Dropdown.OptionData> listBlockDirection = new List<Dropdown.OptionData>();
        List<BlockDirectionEnum> listDirection = EnumExtension.GetEnumValue<BlockDirectionEnum>();
        for (int i = 0; i < listDirection.Count; i++)
        {
            BlockDirectionEnum itemDirection = listDirection[i];
            listBlockDirection.Add(new Dropdown.OptionData(itemDirection.ToString()));
        }
        ui_BlockDirection.options = listBlockDirection;
        ui_BlockDirection.onValueChanged.AddListener((index) =>
        {
            CallBackForDropDown(ui_BlockDirection, index);
        });
        ui_BlockDirection.value = 0;
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
        BuildingEditorHandler.Instance.manager.curBlockDirection = BlockDirectionEnum.UpForward;
        ui_BlockDirection.value = 3;
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
            BuildingEditorHandler.Instance.manager.curCreateType = 1;
        }
        else if (view == ui_CreateSelect && value == true)
        {
            BuildingEditorHandler.Instance.manager.curCreateType = 0;
        }
    }

    /// <summary>
    /// 下拉菜单监听
    /// </summary>
    public void CallBackForDropDown(Dropdown dropdownView, int index)
    {
        //方块方向选择
        if (dropdownView == ui_BlockDirection)
        {
            List<BlockDirectionEnum> listDirection = EnumExtension.GetEnumValue<BlockDirectionEnum>();
            BuildingEditorHandler.Instance.manager.curBlockDirection = listDirection[index];
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
            BuildingEditorHandler.Instance.SaveBuildingData();
        };
        UIDialogNormal uiDialog = UIHandler.Instance.ShowDialog<UIDialogNormal>(dialogData);
    }

    /// <summary>
    /// 点击-加载建筑
    /// </summary>
    public void OnClickForLoadBuilding()
    {
        //清空所有方块
        BuildingEditorHandler.Instance.ClearAllBlock();
        //加载建筑
        BuildingEditorHandler.Instance.LoadBuilding(long.Parse(ui_BuildingIdEdit.text));

    }
}