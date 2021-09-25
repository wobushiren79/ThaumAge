﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UIChildGameSettingDisplayContent : UIChildGameSettingBaseContent
{
    //全屏设置
    protected UIListItemGameSettingRB settingFullScreen;
    //屏幕分辨率
    protected UIListItemGameSettingSelect settingScreenResolutionSelect;
    //显示帧数
    protected UIListItemGameSettingRB settingFrameShow;
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
    //抗锯齿质量
    protected UIListItemGameSettingSelect settingAntiAliasingQualityLevelSelect;

    //抗锯齿数据
    protected List<string> listAntiAliasingData;
    //抗锯齿质量数据
    protected List<string> listAntiAliasingQualityLevelData;
    //屏幕分辨率
    protected List<string> listScreenResolutionData;

    public UIChildGameSettingDisplayContent(GameObject objListContainer) : base(objListContainer)
    {
        listAntiAliasingData = new List<string>
        {
            "None",
            "FXAA",
            "TAA",
            "SMAA"
        };

        listScreenResolutionData = new List<string>
        {
            "640x480",
            "800x480",
            "800x600",
            "960x540",
            "1024x600",
            "1024x768",
            "1280x720",
            "1280x800",
            "1280x1024",
            "1366x768",
            "1400x1050",
            "1440x900",
            "1600x900",
            "1600x1200",
            "1680x1050",
            "1920x1080",
            "1920x1200",
            "2048x1536",
            "2560x1440",
            "2560x1600",
            "3840x2160"
        };
    }

    public override void Open()
    {
        base.Open();

        listAntiAliasingQualityLevelData = new List<string>()
        {
            TextHandler.Instance.GetTextById(10011),
            TextHandler.Instance.GetTextById(10012),
            TextHandler.Instance.GetTextById(10013)
        };

        //是否全屏
        settingFullScreen = CreateItemForRB(TextHandler.Instance.GetTextById(102), HandleForFullScreen);
        settingFullScreen.SetState(gameConfig.window == 1 ? true : false);

        //屏幕分辨率
        settingScreenResolutionSelect = CreateItemForSelect(TextHandler.Instance.GetTextById(111), listScreenResolutionData, HandleForScreenResolution);
        int indexScreenResolutionSelect = GetScreenResolutionIndex(gameConfig.screenResolution);
        settingScreenResolutionSelect.SetIndex(indexScreenResolutionSelect);

        //显示帧数
        settingFrameShow = CreateItemForRB(TextHandler.Instance.GetTextById(110), HandleForFrameShow);
        settingFrameShow.SetState(gameConfig.framesShow);

        //锁定帧数
        settingLockFrame = CreateItemForRB(TextHandler.Instance.GetTextById(103), HandleForLockFrame);
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

        //抗锯齿
        settingAntiAliasingSelect = CreateItemForSelect(TextHandler.Instance.GetTextById(108), listAntiAliasingData, HandleForAntiAliasing);
        settingAntiAliasingSelect.SetIndex((int)gameConfig.GetAntialiasingMode());

        //抗锯齿质量
        settingAntiAliasingQualityLevelSelect = CreateItemForSelect(TextHandler.Instance.GetTextById(109), listAntiAliasingQualityLevelData, HandleForAntiAliasingQualityLevel);
        settingAntiAliasingQualityLevelSelect.SetIndex(gameConfig.antialiasingQualityLevel);
    }

    /// <summary>
    /// 处理抗锯齿
    /// </summary>
    /// <param name="value"></param>
    public void HandleForAntiAliasing(int value)
    {
        gameConfig.SetAntialiasingMode((AntialiasingEnum)value);
        //修改抗锯齿
        CameraHandler.Instance.ChangeAntialiasing(gameConfig.GetAntialiasingMode(), gameConfig.antialiasingQualityLevel);
    }

    /// <summary>
    /// 处理抗锯齿质量
    /// </summary>
    /// <param name="value"></param>
    public void HandleForAntiAliasingQualityLevel(int value)
    {
        gameConfig.antialiasingQualityLevel = value;
        //修改抗锯齿
        CameraHandler.Instance.ChangeAntialiasing(gameConfig.GetAntialiasingMode(), gameConfig.antialiasingQualityLevel);
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
    /// 处理-是否显示帧数
    /// </summary>
    public void HandleForFrameShow(bool value)
    {
        gameConfig.framesShow = value;
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

    /// <summary>
    /// 处理 屏幕分辨率
    /// </summary>
    /// <param name="value"></param>
    public void HandleForScreenResolution(int value)
    {
        string data = listScreenResolutionData[value];
        gameConfig.screenResolution = data;
        gameConfig.GetScreenResolution(out int w,out int h);
        //只有全屏模式才使用固定分辨率，窗口模式时使用自己的分辨率
        if (Screen.fullScreen)
        {
            Screen.SetResolution(w, h, Screen.fullScreen);
        }
    }

    /// <summary>
    /// 获取屏幕分辨率下标
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    protected int GetScreenResolutionIndex(string data)
    {
        for (int i = 0; i < listScreenResolutionData.Count; i++)
        {
            if (listScreenResolutionData[i].Equals(data))
            {
                return i;
            }
        }
        return 0;
    }

}