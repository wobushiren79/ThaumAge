using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class UIMainStart : BaseUIComponent
{

    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if (viewButton == ui_Start) HandleForStart();
        else if (viewButton == ui_Setting) HandleForSetting();
        else if (viewButton == ui_Exit) HandleForExit();
        else if (viewButton == ui_Title) HandleForMaker();
    }

    /// <summary>
    /// 设置UI文本
    /// </summary>
    public void SetUIText()
    {

        //设置版本号
        ui_VersionContent.text = $"Ver {ProjectConfigInfo.GAME_VERSION}";
    }

    /// <summary>
    /// 处理-继续游戏
    /// </summary>
    public void HandleForStart()
    {
        UIMainUserData uiMainUserData = UIHandler.Instance.manager.OpenUIAndCloseOther<UIMainUserData>(UIEnum.MainUserData);
    }

    /// <summary>
    /// 游戏设置
    /// </summary>
    public void HandleForSetting()
    {
        UIGameSetting uiSetting = UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameSetting>(UIEnum.GameSettings);
    }

    /// <summary>
    /// 处理-打开游戏制作人目录
    /// </summary>
    public void HandleForMaker()
    {
        UIMainMaker uiMaker = UIHandler.Instance.manager.OpenUIAndCloseOther<UIMainMaker>(UIEnum.MainMaker);
    }

    /// <summary>
    /// 处理-离开游戏
    /// </summary>
    public void HandleForExit()
    {
        GameUtil.ExitGame();
    }
}