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
        //播放音效
        AudioHandler.Instance.PlaySound(100001);
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        SetUIText();
    }

    /// <summary>
    /// 设置UI文本
    /// </summary>
    public void SetUIText()
    {
        //设置版本号
        ui_VersionContent.text = $"Ver {ProjectConfigInfo.GAME_VERSION}";
        //设置标题
        ui_TitleContent.text = TextHandler.Instance.GetTextById(1);

        ui_BtnNameStart.text = TextHandler.Instance.GetTextById(2);
        ui_BtnNameSetting.text = TextHandler.Instance.GetTextById(3);
        ui_BtnNameExit.text = TextHandler.Instance.GetTextById(4);
    }

    /// <summary>
    /// 处理-继续游戏
    /// </summary>
    public void HandleForStart()
    {
        UIMainUserData uiMainUserData = UIHandler.Instance.OpenUIAndCloseOther<UIMainUserData>(UIEnum.MainUserData);
    }

    /// <summary>
    /// 游戏设置
    /// </summary>
    public void HandleForSetting()
    {
        UIGameSetting uiSetting = UIHandler.Instance.OpenUIAndCloseOther<UIGameSetting>(UIEnum.GameSetting);
    }

    /// <summary>
    /// 处理-打开游戏制作人目录
    /// </summary>
    public void HandleForMaker()
    {
        UIMainMaker uiMaker = UIHandler.Instance.OpenUIAndCloseOther<UIMainMaker>(UIEnum.MainMaker);
    }

    /// <summary>
    /// 处理-离开游戏
    /// </summary>
    public void HandleForExit()
    {
        GameUtil.ExitGame();
    }
}