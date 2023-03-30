using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
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
        RegisterEvent<Vector3Int>(EventsInfo.UIBuildingEditorCreate_PositionChange, CallBackForPositionChange);
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
        else if (viewButton == ui_LoadBuilding)
        {
            OnClickForLoadBuilding();
        }
    }

    public override void OnInputActionForStarted(InputActionUIEnum inputType, InputAction.CallbackContext callback)
    {
        base.OnInputActionForStarted(inputType, callback);
        if(inputType == InputActionUIEnum.F1)
        {
            ui_CreateSelect.isOn = true;
        }
        else if (inputType == InputActionUIEnum.F2)
        {
            ui_DestorySelect.isOn = true;
        }
    }

    public void InitData()
    {
        listBlockInfo = new List<BlockInfoBean>();
        BlockInfoBean[] arrayBlockInfo = BlockHandler.Instance.manager.GetAllBlockInfo();
        //添加不为NULL的数据
        for (int i = 0; i < arrayBlockInfo.Length; i++)
        {
            var blockInfo = arrayBlockInfo[i];
            if (blockInfo != null)
            {
                ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoByBlockId((int)blockInfo.id);
                if (itemsInfo != null)
                {
                    listBlockInfo.Add(blockInfo);
                }
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

    public void SetUIBuildingId(int buildingId)
    {
        ui_BuildingIdEdit.text = $"{buildingId}";
    }

    public void SetUIBuildingName(string buildingName)
    {
        ui_BuildingNameEdit.text = $"{buildingName}";
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

    public void CallBackForPositionChange(Vector3Int position)
    {
        ui_BlockPosition.text = $"{position.x},{position.y},{position.z}";
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
        BuildingEditorHandler.Instance.manager.isStartBuild = false;
        
        DialogBean dialogData = new DialogBean();
        dialogData.content = $"是否要创建ID为 {ui_BuildingIdEdit.text} 的建筑";
        dialogData.actionSubmit = (view, data) =>
        {
            int buildId = int.Parse(ui_BuildingIdEdit.text);
            string buildName = ui_BuildingNameEdit.text;
            BuildingEditorHandler.Instance.SaveBuildingData(buildId, buildName);
            this.WaitExecuteSeconds(0.1f,()=> 
            {
                BuildingEditorHandler.Instance.manager.isStartBuild = true;
            });
        };
        dialogData.actionCancel= (view, data) =>
        {
            this.WaitExecuteSeconds(0.1f, () =>
            {
                BuildingEditorHandler.Instance.manager.isStartBuild = true;
            });
        };
        UIDialogNormal uiDialog = UIHandler.Instance.ShowDialog<UIDialogNormal>(dialogData);
    }

    /// <summary>
    /// 点击-加载建筑
    /// </summary>
    public void OnClickForLoadBuilding()
    {
        BuildingEditorHandler.Instance.manager.isStartBuild = false;
        DialogBean dialogData = new DialogBean();
        dialogData.content = $"是否要加载ID为 {ui_BuildingIdEdit.text} 的建筑";
        dialogData.actionSubmit = (view, data) =>
        {
            //清空所有方块
            BuildingEditorHandler.Instance.ClearAllBlock();
            //加载建筑
            BuildingEditorHandler.Instance.LoadBuilding(long.Parse(ui_BuildingIdEdit.text), (data) =>
            {
                SetUIBuildingId((int)data.id);
                SetUIBuildingName(data.name_cn);
                this.WaitExecuteSeconds(0.1f, () =>
                {
                    BuildingEditorHandler.Instance.manager.isStartBuild = true;
                });
            });
        }; 
        dialogData.actionCancel = (view, data) =>
        {
            this.WaitExecuteSeconds(0.1f, () =>
            {
                BuildingEditorHandler.Instance.manager.isStartBuild = true;
            });
        };
        UIDialogNormal uiDialog = UIHandler.Instance.ShowDialog<UIDialogNormal>(dialogData);
    }

}