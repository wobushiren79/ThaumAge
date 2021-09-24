using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class UIListItemGameSettingSelect : UIListItemGameSettingBase
{
    protected Action<int> callBack;

    public override void Awake()
    {
        base.Awake();
        ui_Dropdown.onValueChanged.AddListener(SelectChange);
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="title"></param>
    /// <param name="listSelectData"></param>
    /// <param name="callBack"></param>
    public void SetData(string title, List<string> listSelectData, Action<int> callBack)
    {
        this.callBack = callBack;
        SetTitle(title);
        ui_Dropdown.ClearOptions();
        ui_Dropdown.AddOptions(listSelectData);
    }

    /// <summary>
    /// 设置第几项
    /// </summary>
    /// <param name="index"></param>
    public void SetIndex(int index)
    {
        ui_Dropdown.value = index;
    }

    /// <summary>
    /// 修改选择
    /// </summary>
    /// <param name="index"></param>
    public void SelectChange(int index)
    {
        callBack?.Invoke(index);
    }
}