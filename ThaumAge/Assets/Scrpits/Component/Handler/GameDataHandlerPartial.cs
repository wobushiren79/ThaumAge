using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameDataHandler
{
    /// <summary>
    /// 初始化数据
    /// </summary>
    public void InitData()
    {
        GameConfigBean gameConfig = manager.GetGameConfig();
        //设置全屏
        Screen.fullScreen = gameConfig.window == 1 ? true : false;
        //环境参数初始化
        VolumeHandler.Instance.InitData();
        //设置FPS
        FPSHandler.Instance.SetData(gameConfig.stateForFrames, gameConfig.frames);
        //修改抗锯齿
        CameraHandler.Instance.ChangeAntialiasing(gameConfig.GetAntialiasingMode(), gameConfig.antialiasingQualityLevel);
    }
}
