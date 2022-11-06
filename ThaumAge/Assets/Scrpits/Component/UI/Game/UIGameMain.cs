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

        this.RegisterEvent<UIViewItemContainer, long>(EventsInfo.UIViewItemContainer_ItemChange, CallBackForShortcutsItemExchange);
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        //道具刷新
        ui_Shortcuts.RefreshUI();
        //状态刷新
        ui_ViewCharacterStatus.RefreshUI();
        //展示一些解锁UI
        ShowUnlockUI();
    }


    /// <summary>
    /// 展示解锁UI
    /// </summary>
    public void ShowUnlockUI()
    {
        ui_MagicCore.gameObject.ShowObj(false);
        ui_ViewMagicCoreList.ShowObj(false);
        //法杖的UI
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        ItemsBean holdItemsData = userData.GetItemsFromShortcut();
        if (holdItemsData.itemId != 0)
        {
            ItemsInfoBean holdItemInfo = ItemsHandler.Instance.manager.GetItemsInfoById(holdItemsData.itemId);
            if (holdItemInfo.GetItemsType() == ItemsTypeEnum.Wand)
            {
                ui_MagicCore.gameObject.ShowObj(true);
                ui_ViewMagicCoreList.ShowObj(true);
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
    /// 打开法术核心装配界面
    /// </summary>
    public void OpenMagicCoreUI()
    {
        if (ui_MagicCore.gameObject.activeSelf)
        {
            //打开法术核心界面
            UIHandler.Instance.OpenUIAndCloseOther<UIGameMagicCore>(UIEnum.GameMagicCore);
            //播放音效
            AudioHandler.Instance.PlaySound(1);
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

    /// <summary>
    /// 回调 道具修改
    /// </summary>
    /// <param name="uIViewItem"></param>
    /// <param name="itemId"></param>
    public void CallBackForShortcutsItemExchange(UIViewItemContainer uIViewItem, long itemId)
    {
        ShowUnlockUI();
    }
}
