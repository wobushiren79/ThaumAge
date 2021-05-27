using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIGameMain : BaseUIComponent
{
    public UIViewShortcuts ui_Shortcuts;
    public UIViewGodMode ui_GodMode;


    public override void OpenUI()
    {
        base.OpenUI();
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        if (ProjectConfigInfo.BUILD_TYPE == ProjectBuildTypeEnum.Debug)
        {
            ui_GodMode.gameObject.SetActive(true);
        }
        else
        {
            ui_GodMode.gameObject.SetActive(false);
        }
    }
}
