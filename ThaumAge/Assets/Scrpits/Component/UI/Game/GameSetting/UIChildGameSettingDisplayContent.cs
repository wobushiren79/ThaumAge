using System;
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
    protected UIListItemGameSettingSelect settingUISize;
    //阴影质量
    protected UIListItemGameSettingSelect settingShadowResolutionLevelData;
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
    //阴影质量
    protected List<string> listShadowResolutionLevelData;
    //UI大小
    protected List<string> listUISizeData;

    public UIChildGameSettingDisplayContent(GameObject objListContainer) : base(objListContainer)
    {
        listUISizeData = new List<string>
        {
            "40%",
            "50%",
            "60%",
            "70%",
            "80%",
            "90%",
            "100%"
            //"105%",
            //"110%",
            //"115%",
            //"120%",
            //"125%",
        };


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

        listShadowResolutionLevelData = new List<string>()
        {
            TextHandler.Instance.GetTextById(10011),
            TextHandler.Instance.GetTextById(10012),
            TextHandler.Instance.GetTextById(10013),
            TextHandler.Instance.GetTextById(10014)
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
        settingFrame.SetPro((gameConfig.frames - 20) / 100f);

        //UI大小
        settingUISize = CreateItemForSelect(TextHandler.Instance.GetTextById(113), listUISizeData, HandleForUISize);
        settingUISize.SetIndex(GetUISizeIndex(gameConfig.uiSize));

        //阴影质量等级
        settingShadowResolutionLevelData = CreateItemForSelect(TextHandler.Instance.GetTextById(112), listShadowResolutionLevelData, HandleForShadowResolutionLevel);
        settingShadowResolutionLevelData.SetIndex(gameConfig.shadowResolutionLevel);

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
    public void HandleForUISize(int value)
    {
        float uiSize = GetUISizeByIndex(value);
        gameConfig.uiSize = uiSize;
        UIHandler.Instance.ChangeUISize(gameConfig.uiSize);
    }

    /// <summary>
    /// 处理-阴影质量
    /// </summary>
    public void HandleForShadowResolutionLevel(int index)
    {
        gameConfig.shadowResolutionLevel = index;
        LightHandler.Instance.ChangeShadowResolutionLevel(gameConfig.shadowResolutionLevel);
        VolumeHandler.Instance.manager.SetShadowsDistance(gameConfig.shadowDis);
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
        FPSHandler.Instance.SetData(gameConfig.stateForFrames, gameConfig.frames);
    }

    /// <summary>
    /// 处理-帧数
    /// </summary>
    public void HandleForFrame(float value)
    {
        gameConfig.frames = 20 + (int)(value * 100);
        settingFrame.SetContent($"{gameConfig.frames}");
        FPSHandler.Instance.SetData(gameConfig.stateForFrames, gameConfig.frames);
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
        gameConfig.GetScreenResolution(out int w, out int h);
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

    /// <summary>
    /// 获取UI大小下标
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    protected int GetUISizeIndex(float size)
    {
        string uiSizeData = $"{Mathf.RoundToInt((size * 100))}%";
        for (int i = 0; i < listUISizeData.Count; i++)
        {
            string uiSizeItem = listUISizeData[i];
            if (uiSizeData.Equals(uiSizeItem))
            {
                return i;
            }
        }
        return 1;
    }

    /// <summary>
    /// 通过大小获取UI
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    protected float GetUISizeByIndex(int index)
    {
        string data = listUISizeData[index];
        int uiSize = int.Parse(data.Replace("%", ""));
        return uiSize / 100f;
    }
}