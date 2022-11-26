using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public partial class UIGameExit : UIGameCommonNormal
{
    public override void RefreshUI(bool isOpenInit = false)
    {
        base.RefreshUI(isOpenInit);
        InitUIData();
    }

    public virtual void InitUIData()
    {
        ui_BtnNameBackGame.text = TextHandler.Instance.GetTextById(901);
        ui_BtnNameBackMain.text = TextHandler.Instance.GetTextById(902);
        ui_BtnNameExit.text = TextHandler.Instance.GetTextById(903);
    }

    public override void OnClickForButton(Button viewButton)
    {
        if(viewButton == ui_BackGame)
        {
            HandleForBackGameMain();
        }
        else if (viewButton == ui_BackMain)
        {
            HandleForBackMainScene();
        }
        else if (viewButton == ui_Exit)
        {
            HandleForExitGame();
        }
    }


    /// <summary>
    /// 返回游戏主菜单
    /// </summary>
    public virtual void HandleForBackMainScene()
    {
        DialogBean dialogData = new DialogBean();
        dialogData.content = TextHandler.Instance.GetTextById(20003);
        dialogData.actionSubmit = (view, data) =>
        {
            ActionForBackMain();
        };
        UIHandler.Instance.ShowDialog<UIDialogNormal>(dialogData);
        AudioHandler.Instance.PlaySound(1);
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public virtual void HandleForExitGame()
    {
        DialogBean dialogData = new DialogBean();
        dialogData.content = TextHandler.Instance.GetTextById(20004);
        dialogData.actionSubmit = (view, data) =>
        {
            ActionForExitGame();
        };
        UIHandler.Instance.ShowDialog<UIDialogNormal>(dialogData);
        AudioHandler.Instance.PlaySound(1);
    }

    public virtual void ActionForBackMain()
    {
        //保存数据
        GameDataHandler.Instance.manager.SaveUserData();
        //改变场景
        SceneMainHandler.Instance.ChangeScene(ScenesEnum.MainScene);
    }

    public virtual void ActionForExitGame()
    {
        //保存数据
        GameDataHandler.Instance.manager.SaveUserData();
        //离开游戏
        GameUtil.ExitGame();
    }
}