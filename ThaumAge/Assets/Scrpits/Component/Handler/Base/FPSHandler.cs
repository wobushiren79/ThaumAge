using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSHandler : BaseHandler<FPSHandler, BaseManager>
{

    protected override void Awake()
    {
        base.Awake();
        //Screen.SetResolution(1280, 800, false);	
        GameConfigBean gameConfig = GameDataHandler.Instance.manager.GetGameConfig();
        SetData(gameConfig.stateForFrames, gameConfig.frames);
    }

    public void SetData(bool isLock, int fps)
    {
        if (isLock)
        {
            Application.targetFrameRate = fps;
        }
        else
        {
            Application.targetFrameRate = -1;
        }
    }

    public void SetData(int isLock, int fps)
    {
        if (isLock == 1)
        {
            Application.targetFrameRate = fps;
        }
        else
        {
            Application.targetFrameRate = -1;
        }
    }

    /// <summary>
    /// 设置垂直同步
    /// 使用“不同步”(0) 不等待 VSync。值必须为 0、1、2、3 或 4。
    /// 如果此设置设置为“不同步”(0) 以外的值，则Application.targetFrameRate的值将被忽略。
    /// </summary>
    /// <param name="SyncCount"></param>
    public void SetSyncCount(int SyncCount)
    {
        QualitySettings.vSyncCount = SyncCount;
    }
}
