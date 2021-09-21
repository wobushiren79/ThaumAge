using System;
using UnityEditor;
using UnityEngine;

public class UIChildGameSettingDisplayContent : UIChildGameSettingBaseContent
{
    //全屏设置
    protected UIListItemGameSettingRB settingFullScreen;
    //锁定帧数
    protected UIListItemGameSettingRB settingLockFrame;
    //帧数
    protected UIListItemGameSettingRange settingFrame;
    //UI大小
    protected UIListItemGameSettingRange settingUISize;
    //阴影距离
    protected UIListItemGameSettingRange settingShadowDis;

    public UIChildGameSettingDisplayContent(GameObject objListContainer) : base(objListContainer)
    {

    }

    public override void Open()
    {
        base.Open();
        //是否全屏
        settingFullScreen = CreateItemForRB("全屏", HandleForFullScreen);
        settingFullScreen.SetState(gameConfig.window == 1 ? true : false);

        //锁定帧数
        settingLockFrame = CreateItemForRB("锁定限制", HandleForFullScreen);
        settingLockFrame.SetState(gameConfig.stateForFrames == 1 ? true : false);

        //帧数
        settingFrame = CreateItemForRange("帧数", HandleForFrame);
        settingFrame.SetPro((gameConfig.frames - 20) / 100);

        //UI大小
        //settingUISize = CreateItemForRange("界面大小", HandleForUISize);
        //settingUISize.SetPro(gameConfig.uiSize);

        //阴影距离
        settingShadowDis = CreateItemForRange("阴影距离", HandleForShadowDis);
        settingShadowDis.SetPro(gameConfig.shadowDis / 200);
    }

    /// <summary>
    /// 处理-UI大小
    /// </summary>
    public void HandleForUISize(float value)
    {
        gameConfig.uiSize = value;
        UIHandler.Instance.manager.RefreshAllUI();
    }

    /// <summary>
    /// 处理-阴影距离
    /// </summary>
    /// <param name="value"></param>
    public void HandleForShadowDis(float value)
    {
        gameConfig.shadowDis = value * 200;
        VolumeHandler.Instance.manager.SetShadowsDistance(gameConfig.shadowDis);
        settingShadowDis.SetContent($"{Math.Round(gameConfig.shadowDis, 0)}m");
    }

    /// <summary>
    /// 处理-锁定帧数
    /// </summary>
    /// <param name="value"></param>
    public void HandleForLockFrame(bool value)
    {
        gameConfig.stateForFrames = value ? 1 : 0;
        FPSHandler.Instance.SetData(value, gameConfig.frames);
    }

    /// <summary>
    /// 处理-帧数
    /// </summary>
    public void HandleForFrame(float value)
    {
        gameConfig.frames = 20 + (int)(value * 100);
        settingFrame.SetContent($"{gameConfig.frames}");
    }

    /// <summary>
    /// 处理-是否开启全屏
    /// </summary>
    /// <param name="value"></param>
    public void HandleForFullScreen(bool value)
    {
        gameConfig.window = value ? 1 : 0;
        Screen.fullScreen = value;
    }


}