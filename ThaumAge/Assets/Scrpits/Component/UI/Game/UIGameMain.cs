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

        this.RegisterEvent<UIViewItemContainer, long>(EventsInfo.UIViewItemContainer_ItemChange, CallBackForShortcutsItemExchange);
        this.RegisterEvent<int>(EventsInfo.UIViewShortcuts_ChangeSelect, CallBackForShortcutsChangeSelect);

        ui_Shortcuts.OpenUI();
        ui_ViewCharacterStatus.OpenUI();
    }

    public override void RefreshUI(bool isOpenInit = false)
    {
        base.RefreshUI(isOpenInit);

        //չʾһЩ����UI
        ShowUnlockUI();
        if (isOpenInit)
            return;
        //����ˢ��
        ui_Shortcuts.RefreshUI();
        //״̬ˢ��
        ui_ViewCharacterStatus.RefreshUI();
    }

    public override void CloseUI()
    {
        base.CloseUI();
        ui_Shortcuts.CloseUI();
        ui_ViewCharacterStatus.CloseUI();
    }


    /// <summary>
    /// չʾ����UI
    /// </summary>
    public void ShowUnlockUI()
    {
        ui_MagicCore.gameObject.ShowObj(false);
        ui_ViewShortcutsMagic.CloseUI();
        //���ȵ�UI
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        ItemsBean holdItemsData = userData.GetItemsFromShortcut();
        if (holdItemsData.itemId != 0)
        {
            ItemsInfoBean holdItemInfo = ItemsHandler.Instance.manager.GetItemsInfoById(holdItemsData.itemId);
            if (holdItemInfo.GetItemsType() == ItemsTypeEnum.Wand)
            {
                ui_MagicCore.gameObject.ShowObj(true);
                ui_ViewShortcutsMagic.OpenUI();
            }
        }
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
        else if (viewButton == ui_MagicCore)
        {
            OpenMagicCoreUI();
        }
    }

    public override void OnInputActionForStarted(InputActionUIEnum inputName, CallbackContext callback)
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
            case InputActionUIEnum.I:
                OpenMagicCoreUI();
                break;
        }
    }

    /// <summary>
    /// �򿪷�������װ�����
    /// </summary>
    public void OpenMagicCoreUI()
    {
        if (ui_MagicCore.gameObject.activeSelf)
        {        
            //���ȵ�UI
            UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
            ItemsBean holdItemsData = userData.GetItemsFromShortcut();
            if (holdItemsData.itemId != 0)
            {
                ItemsInfoBean holdItemInfo = ItemsHandler.Instance.manager.GetItemsInfoById(holdItemsData.itemId);
                if (holdItemInfo.GetItemsType() == ItemsTypeEnum.Wand)
                {
                    //�򿪷������Ľ���
                    UIGameMagicCore uiGameMagicCore = UIHandler.Instance.OpenUIAndCloseOther<UIGameMagicCore>();
                    uiGameMagicCore.SetData(holdItemsData);
                    //������Ч
                    AudioHandler.Instance.PlaySound(1);
                }
            }
        }
    }

    /// <summary>
    /// ���û�����
    /// </summary>
    public void OpenUserDetailsUI()
    {
        //���û�����
        UIHandler.Instance.OpenUIAndCloseOther<UIGameUserDetails>();
        //������Ч
        AudioHandler.Instance.PlaySound(1);
    }

    /// <summary>
    /// ��ħ����
    /// </summary>
    public void OpenBookUI()
    {
        //��ħ����
        UIHandler.Instance.OpenUIAndCloseOther<UIGameBook>();
        //������Ч
        AudioHandler.Instance.PlaySound(1);
    }

    /// <summary>
    ///  ������
    /// </summary>
    public void OpenSettingUI()
    {
        //������
        UIHandler.Instance.OpenUIAndCloseOther<UIGameSetting>();
        //������Ч
        AudioHandler.Instance.PlaySound(1);
    }

    /// <summary>
    /// ���뿪
    /// </summary>
    public void OpenExitUI()
    {
        //���뿪
        UIHandler.Instance.OpenUIAndCloseOther<UIGameExit>();
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
            UIHandler.Instance.OpenUIAndCloseOther<UIGodMain>();
            //������Ч
            AudioHandler.Instance.PlaySound(1);
        }
    }

    /// <summary>
    /// �ص� �����޸�
    /// </summary>
    public void CallBackForShortcutsItemExchange(UIViewItemContainer uIViewItem, long itemId)
    {
        ShowUnlockUI();
    }

    /// <summary>
    /// �ص� �����л�
    /// </summary>
    public void CallBackForShortcutsChangeSelect(int selectIndex)
    {
        ShowUnlockUI();
    }
}
