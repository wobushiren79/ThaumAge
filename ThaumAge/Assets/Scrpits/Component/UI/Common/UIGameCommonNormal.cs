using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class UIGameCommonNormal : BaseUIComponent
{
    public Button ui_Exit;

    public override void OpenUI()
    {
        base.OpenUI();
        GameControlHandler.Instance.SetPlayerControlEnabled(false);

        //关闭提示显示
        PlayerTargetBlock playerTarget = GameHandler.Instance.manager.playerTargetBlock;
        playerTarget?.SetIsShow(false);
    }

    public override void CloseUI()
    {
        base.CloseUI();
        GameControlHandler.Instance.SetPlayerControlEnabled(true);
        //关闭提示显示
        PlayerTargetBlock playerTarget = GameHandler.Instance.manager.playerTargetBlock;
        playerTarget?.SetIsShow(true);
    }

    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if (viewButton == ui_Exit)
        {
            HandleForBackGameMain();
        }
    }

    public override void OnInputActionForStarted(InputActionUIEnum inputName, UnityEngine.InputSystem.InputAction.CallbackContext callback)
    {
        base.OnInputActionForStarted(inputName, callback);
        switch (inputName)
        {
            case InputActionUIEnum.ESC:
                HandleForBackGameMain();
                break;
        }
    }

    /// <summary>
    /// 处理返回主界面
    /// </summary>
    public virtual void HandleForBackGameMain()
    {
        UIHandler.Instance.OpenUIAndCloseOther<UIGameMain>();
        //播放音效
        AudioHandler.Instance.PlaySound(2);
    }
}
