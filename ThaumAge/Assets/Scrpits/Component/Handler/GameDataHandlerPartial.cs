using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameDataHandler
{
    /// <summary>
    /// ��ʼ������
    /// </summary>
    public void InitData()
    {
        GameConfigBean gameConfig = manager.GetGameConfig();
        //����ȫ��
        Screen.fullScreen = gameConfig.window == 1 ? true : false;
        //����������ʼ��
        VolumeHandler.Instance.InitData();
        //����FPS
        FPSHandler.Instance.SetData(gameConfig.stateForFrames, gameConfig.frames);
        //�޸Ŀ����
        CameraHandler.Instance.ChangeAntialiasing(gameConfig.GetAntialiasingMode(), gameConfig.antialiasingQualityLevel);
    }
}
