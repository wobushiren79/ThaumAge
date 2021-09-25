using System;
using System.Collections.Generic;
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
    //抗锯齿
    protected UIListItemGameSettingSelect settingAntiAliasingSelect;

    //抗锯齿数据
    protected List<string> listAntiAliasingData;

    public UIChildGameSettingDisplayContent(GameObject objListContainer) : base(objListContainer)
    {
        listAntiAliasingData = new List<string>
        {
            "None",
            "FXAA",
            "TAA",
            "SMAA"
        };
    }

    public override void Open()
    {
        base.Open();

        //是否全屏
        settingFullScreen = CreateItemForRB(TextHandler.Instance.GetTextById(102), HandleForFullScreen);
        settingFullScreen.SetState(gameConfig.window == 1 ? true : false);

        //锁定帧数
        settingLockFrame = CreateItemForRB(TextHandler.Instance.GetTextById(103), HandleForFullScreen);
        settingLockFrame.SetState(gameConfig.stateForFrames == 1 ? true : false);

        //帧数
        settingFrame = CreateItemForRange(TextHandler.Instance.GetTextById(104), HandleForFrame);
        settingFrame.SetPro((gameConfig.frames - 20) / 100);

        //UI大小
        //settingUISize = CreateItemForRange("界面大小", HandleForUISize);
        //settingUISize.SetPro(gameConfig.uiSize);

        //阴影距离
        settingShadowDis = CreateItemForRange(TextHandler.Instance.GetTextById(105), HandleForShadowDis);
        settingShadowDis.SetPro(gameConfig.shadowDis / 200);

        //阴影距离
        settingAntiAliasingSelect = CreateItemForSelect(TextHandler.Instance.GetTextById(108), listAntiAliasingData, HandleForAntiAliasing);
        settingAntiAliasingSelect.SetIndex((int)gameConfig.GetAntialiasingMode());
    }

    /// <summary>
    /// 处理抗锯齿
    /// </summary>
    /// <param name="value"></param>
    public void HandleForAntiAliasing(int value)
    {
        gameConfig.SetAntialiasingMode((AntialiasingEnum)value);
        //修改抗锯齿
        CameraHandler.Instance.ChangeAntialiasing((AntialiasingEnum)value);
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