﻿using UnityEditor;
using UnityEngine;
public partial class UIGameElementSmeltery : UIGameCommonNormal
{
    public override void OpenUI()
    {
        base.OpenUI();
        ui_Shortcuts.OpenUI();
        ui_ViewBackPack.OpenUI();
    }

    public override void CloseUI()
    {
        base.CloseUI();
        ui_Shortcuts.CloseUI();
        ui_ViewBackPack.CloseUI();
    }

    public override void RefreshUI(bool isOpenInit = false)
    {
        base.RefreshUI(isOpenInit);
        if (isOpenInit)
            return;
        ui_Shortcuts.RefreshUI();
        ui_ViewBackPack.RefreshUI();
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="blockData"></param>
    public void SetData(Vector3Int worldPosition)
    {
        ui_ViewElementSmeltery.SetData(worldPosition);
    }
}