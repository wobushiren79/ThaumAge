using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public partial class UIGameMain : BaseUIComponent
{
    protected InputAction inputGodMain;

    public override void RefreshUI()
    {
        base.RefreshUI();
        ui_Shortcuts.RefreshUI();
    }

    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if (viewButton == ui_Details)
        {
            OpenUserDetailsUI();
        }
        else if (viewButton == ui_Book)
        {
            OpenBookUI();
        }
        else if (viewButton == ui_Setting)
        {
            OpenSettingUI();
        }
    }

    public override void OnInputActionForStarted(InputActionUIEnum inputName,CallbackContext callback)
    {
        base.OnInputActionForStarted(inputName, callback);
        switch (inputName)
        {
            case InputActionUIEnum.F12:
                OpenGodMain();
                break;
            case InputActionUIEnum.ESC:
                OpenSettingUI();
                break;
            case InputActionUIEnum.B:
                OpenUserDetailsUI();
                break;
            case InputActionUIEnum.T:
                OpenBookUI();
                break;
        }
    }

    /// <summary>
    /// 打开用户详情
    /// </summary>
    public void OpenUserDetailsUI()
    {
        //打开用户详情
        UIHandler.Instance.OpenUIAndCloseOther<UIGameUserDetails>(UIEnum.GameUserDetails);
    }

    /// <summary>
    /// 打开魔法书
    /// </summary>
    public void OpenBookUI()
    {
        //打开魔法书
        UIHandler.Instance.OpenUIAndCloseOther<UIGameBook>(UIEnum.GameBook);
    }

    /// <summary>
    ///  打开设置
    /// </summary>
    public void OpenSettingUI()
    {
        //打开设置
        UIHandler.Instance.OpenUIAndCloseOther<UIGameSetting>(UIEnum.GameSetting);
    }

    /// <summary>
    /// 打开GM菜单
    /// </summary>
    public void OpenGodMain()
    {
        if (ProjectConfigInfo.BUILD_TYPE == ProjectBuildTypeEnum.Debug)
        {
            //打开GM菜单
            UIHandler.Instance.OpenUIAndCloseOther<UIGodMain>(UIEnum.GodMain);
        }
    }
}
