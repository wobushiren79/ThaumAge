using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public partial class UIGameMain : BaseUIComponent
{
    public override void OpenUI()
    {
        //设置对应数据
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        ui_ViewCharacterStatus.SetData(userData.characterData);

        base.OpenUI();
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        //道具刷新
        ui_Shortcuts.RefreshUI();
        //状态刷新
        ui_ViewCharacterStatus.RefreshUI();
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
        else if (viewButton == ui_Exit)
        {
            OpenExitUI();
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
            case InputActionUIEnum.P:
                OpenSettingUI();
                break;
            case InputActionUIEnum.ESC:
                OpenExitUI();
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
        //播放音效
        AudioHandler.Instance.PlaySound(1);
    }

    /// <summary>
    /// 打开魔法书
    /// </summary>
    public void OpenBookUI()
    {
        //打开魔法书
        UIHandler.Instance.OpenUIAndCloseOther<UIGameBook>(UIEnum.GameBook);
        //播放音效
        AudioHandler.Instance.PlaySound(1);
    }

    /// <summary>
    ///  打开设置
    /// </summary>
    public void OpenSettingUI()
    {
        //打开设置
        UIHandler.Instance.OpenUIAndCloseOther<UIGameSetting>(UIEnum.GameSetting);
        //播放音效
        AudioHandler.Instance.PlaySound(1);
    }

    /// <summary>
    /// 打开离开
    /// </summary>
    public void OpenExitUI()
    {
        //打开离开
        UIHandler.Instance.OpenUIAndCloseOther<UIGameExit>(UIEnum.GameExit);
        //播放音效
        AudioHandler.Instance.PlaySound(1);
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
            //播放音效
            AudioHandler.Instance.PlaySound(1);
        }
    }
}
