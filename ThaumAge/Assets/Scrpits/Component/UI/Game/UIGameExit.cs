using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public partial class UIGameExit : UIGameCommonNormal
{
    public override void RefreshUI()
    {
        base.RefreshUI();
        InitUIData();
    }

    public void InitUIData()
    {
        ui_BtnNameBackGame.text = TextHandler.Instance.GetTextById(901);
        ui_BtnNameBackMain.text = TextHandler.Instance.GetTextById(902);
        ui_BtnNameExit.text = TextHandler.Instance.GetTextById(903);
    }

    public override void OnInputActionForStarted(InputActionUIEnum inputType, InputAction.CallbackContext callback)
    {
        base.OnInputActionForStarted(inputType, callback);
        switch (inputType)
        {
            case InputActionUIEnum.ESC:
                HandleForBackGame();
                break;
        }
    }

    public override void OnClickForButton(Button viewButton)
    {
        if(viewButton == ui_BackGame)
        {
            HandleForBackGame();
        }
        else if (viewButton == ui_BackMain)
        {
            HandleForBackMain();
        }
        else if (viewButton == ui_Exit)
        {
            HandleForExitGame();
        }
    }

    /// <summary>
    /// 返回游戏继续
    /// </summary>
    public void HandleForBackGame()
    {
        UIHandler.Instance.OpenUIAndCloseOther<UIGameMain>(UIEnum.GameMain);
        AudioHandler.Instance.PlaySound(1);
    }

    /// <summary>
    /// 返回游戏主菜单
    /// </summary>
    public override void HandleForBackMain()
    {
        DialogBean dialogData = new DialogBean();
        dialogData.content = TextHandler.Instance.GetTextById(20003);
        dialogData.actionSubmit = (view, data) =>
        {
            //保存数据
            GameDataHandler.Instance.manager.SaveUserData();
            //改变场景
            SceneMainHandler.Instance.ChangeScene(ScenesEnum.MainScene);
        };
        UIHandler.Instance.ShowDialog<UIDialogNormal>(dialogData);
        AudioHandler.Instance.PlaySound(1);
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void HandleForExitGame()
    {
        DialogBean dialogData = new DialogBean();
        dialogData.content = TextHandler.Instance.GetTextById(20004);
        dialogData.actionSubmit = (view, data) =>
        {            
            //保存数据
            GameDataHandler.Instance.manager.SaveUserData();
            //离开游戏
            GameUtil.ExitGame();
        };
        UIHandler.Instance.ShowDialog<UIDialogNormal>(dialogData);
        AudioHandler.Instance.PlaySound(1);
    }
}