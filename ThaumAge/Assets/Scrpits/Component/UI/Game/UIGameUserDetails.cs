﻿using UnityEditor;
using UnityEngine;

public class UIGameUserDetails : UICommonNormal
{
    public UIViewShortcuts ui_Shortcuts;
    public UIViewBackpackList ui_BackpackList;

    public override void RefreshUI()
    {
        base.RefreshUI();
        ui_Shortcuts.RefreshUI();
        
    }



}