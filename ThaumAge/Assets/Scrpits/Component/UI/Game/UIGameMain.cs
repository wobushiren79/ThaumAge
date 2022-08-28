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
        //���ö�Ӧ����
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        ui_ViewCharacterStatus.SetData(userData.characterData);

        base.OpenUI();
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        //����ˢ��
        ui_Shortcuts.RefreshUI();
        //״̬ˢ��
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
    /// ���û�����
    /// </summary>
    public void OpenUserDetailsUI()
    {
        //���û�����
        UIHandler.Instance.OpenUIAndCloseOther<UIGameUserDetails>(UIEnum.GameUserDetails);
        //������Ч
        AudioHandler.Instance.PlaySound(1);
    }

    /// <summary>
    /// ��ħ����
    /// </summary>
    public void OpenBookUI()
    {
        //��ħ����
        UIHandler.Instance.OpenUIAndCloseOther<UIGameBook>(UIEnum.GameBook);
        //������Ч
        AudioHandler.Instance.PlaySound(1);
    }

    /// <summary>
    ///  ������
    /// </summary>
    public void OpenSettingUI()
    {
        //������
        UIHandler.Instance.OpenUIAndCloseOther<UIGameSetting>(UIEnum.GameSetting);
        //������Ч
        AudioHandler.Instance.PlaySound(1);
    }

    /// <summary>
    /// ���뿪
    /// </summary>
    public void OpenExitUI()
    {
        //���뿪
        UIHandler.Instance.OpenUIAndCloseOther<UIGameExit>(UIEnum.GameExit);
        //������Ч
        AudioHandler.Instance.PlaySound(1);
    }

    /// <summary>
    /// ��GM�˵�
    /// </summary>
    public void OpenGodMain()
    {
        if (ProjectConfigInfo.BUILD_TYPE == ProjectBuildTypeEnum.Debug)
        {
            //��GM�˵�
            UIHandler.Instance.OpenUIAndCloseOther<UIGodMain>(UIEnum.GodMain);
            //������Ч
            AudioHandler.Instance.PlaySound(1);
        }
    }
}
