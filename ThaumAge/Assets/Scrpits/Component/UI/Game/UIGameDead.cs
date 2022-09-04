using UnityEditor;
using UnityEngine;

public partial class UIGameDead : UIGameExit
{
    public override void OpenUI()
    {
        base.OpenUI();
        AudioHandler.Instance.PlaySound(1001);
    }

    /// <summary>
    /// 初始化UI
    /// </summary>
    public override void InitUIData()
    {
        base.InitUIData();
        ui_BtnNameBackGame.text = TextHandler.Instance.GetTextById(911);
        ui_Title.text = TextHandler.Instance.GetTextById(912);
    }

    /// <summary>
    /// 重生
    /// </summary>
    public override void HandleForBackGame()
    {
        SaveData();
        //改变场景
        SceneMainHandler.Instance.ChangeScene(ScenesEnum.GameScene);
        //关闭控制
        GameControlHandler.Instance.manager.controlForPlayer.EnabledControl(false);
        //播放音效
        AudioHandler.Instance.PlaySound(1);
    }

    public override void ActionForBackMain()
    {
        SaveData();
        //改变场景
        SceneMainHandler.Instance.ChangeScene(ScenesEnum.MainScene);
        //关闭控制
        GameControlHandler.Instance.manager.controlForPlayer.EnabledControl(false);
    }

    public override void ActionForExitGame()
    {
        SaveData();
        //离开游戏
        GameUtil.ExitGame();
        //关闭控制
        GameControlHandler.Instance.manager.controlForPlayer.EnabledControl(false);
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    public void SaveData()
    {
        var userData = GameDataHandler.Instance.manager.GetUserData();
        //回复所有状态
        userData.characterData.GetCreatureStatus().ReplyAllStatus();
        //获取世界位置
        userData.userPosition.GetWorldPosition(out WorldTypeEnum worldType, out Vector3 worldPosition);
        //保存数据
        GameDataHandler.Instance.manager.SaveUserData(worldType, worldPosition);
    }
}