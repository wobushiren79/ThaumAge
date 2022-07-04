using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class CameraHandler : BaseHandler<CameraHandler, CameraManager>
{
    protected float maxOrthographicSize = 120;
    protected float minOrthographicSize = 20;

    public float timeScale = 1;

    /// <summary>
    /// 初始化游戏摄像头数据
    /// </summary>
    public void InitGameCameraData()
    {
        //初始化视野
        GameConfigBean gameConfig = GameDataHandler.Instance.manager.GetGameConfig();
        SetCameraFieldOfView(gameConfig.cameraFOV);
    }

    protected Tween animForShakeCameraFirst;
    protected Tween animForShakeCameraThree_1;
    protected Tween animForShakeCameraThree_2;
    protected Tween animForShakeCameraThree_3;
    /// <summary>
    /// 抖动摄像头
    /// </summary>
    /// <param name="time">时间</param>
    /// <param name="amplitude">强度</param>
    /// <param name="frequency">频率</param>
    public void ShakeCamera(float time, float amplitude = 5f, float frequency = 5f)
    {
        CinemachineVirtualCamera cameraForFirst = manager.cameraForFirst;
        //第一人称
        CinemachineBasicMultiChannelPerlin basicMultiChannelPerlin = cameraForFirst.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (basicMultiChannelPerlin != null)
        {
            basicMultiChannelPerlin.m_AmplitudeGain = amplitude;
            basicMultiChannelPerlin.m_FrequencyGain = frequency;
            //初始化位置
            cameraForFirst.transform.position = GameHandler.Instance.manager.player.objFirstLook.transform.position;
            //执行抖动减缓动画
            DOTween.To(() => basicMultiChannelPerlin.m_AmplitudeGain, x => basicMultiChannelPerlin.m_AmplitudeGain = x, 0, time);
        }

        //第三人称
        CinemachineFreeLook cameraForThree = manager.cameraForThree;
        for (int i = 0; i < cameraForThree.m_Orbits.Length; i++)
        {
            CinemachineVirtualCamera itemVirtualCamera = cameraForThree.GetRig(i);
            CinemachineBasicMultiChannelPerlin itemBasicMultiChannelPerlin = itemVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            //初始化位置
            cameraForThree.transform.position = GameHandler.Instance.manager.player.objThirdLook.transform.position;
            if (itemBasicMultiChannelPerlin != null)
            {
                itemBasicMultiChannelPerlin.m_AmplitudeGain = amplitude;
                itemBasicMultiChannelPerlin.m_FrequencyGain = frequency;
                //执行抖动减缓动画
                DOTween.To(() => itemBasicMultiChannelPerlin.m_AmplitudeGain, x => itemBasicMultiChannelPerlin.m_AmplitudeGain = x, 0, time);
            }
        }
    }

    /// <summary>
    /// 是否开启摄像头移动
    /// </summary>
    /// <param name="enabled"></param>
    /// <param name="type">0:玩家 1:建筑编辑</param>
    public void EnabledCameraMove(bool enabled, int type = 0)
    {
        float xSpeed;
        float ySpeed;

        if (enabled)
        {
            GameConfigBean gameConfig = GameDataHandler.Instance.manager.GetGameConfig();
            xSpeed = gameConfig.speedForPlayerCameraMoveX;
            ySpeed = gameConfig.speedForPlayerCameraMoveY;
        }
        else
        {
            xSpeed = 0;
            ySpeed = 0;
        }

        switch (type)
        {
            case 0:
                ChangeCameraSpeedForPlayer(xSpeed, ySpeed);
                break;
            case 1:
                ChangeCameraSpeedForBuildingEditor(xSpeed, ySpeed);
                break;
        }
    }

    /// <summary>
    /// 修改摄像头抗锯齿
    /// </summary>
    /// <param name="antialiasingEnum"></param>
    /// <param name="qualityLevel"></param>
    public void ChangeAntialiasing(AntialiasingEnum antialiasingEnum, int qualityLevel = 1)
    {
        HDAdditionalCameraData hdAdditionalCamera = manager.mainCamera.GetComponent<HDAdditionalCameraData>();
        HDAdditionalCameraData.AntialiasingMode antialiasingMode = HDAdditionalCameraData.AntialiasingMode.None;
        switch (antialiasingEnum)
        {
            case AntialiasingEnum.None:
                break;
            case AntialiasingEnum.FXAA:
                antialiasingMode = HDAdditionalCameraData.AntialiasingMode.FastApproximateAntialiasing;
                break;
            case AntialiasingEnum.TAA:
                antialiasingMode = HDAdditionalCameraData.AntialiasingMode.TemporalAntialiasing;
                break;
            case AntialiasingEnum.SMAA:
                antialiasingMode = HDAdditionalCameraData.AntialiasingMode.SubpixelMorphologicalAntiAliasing;
                break;
        }
        hdAdditionalCamera.antialiasing = antialiasingMode;
        switch (qualityLevel)
        {
            case 0:
                hdAdditionalCamera.SMAAQuality = HDAdditionalCameraData.SMAAQualityLevel.Low;
                hdAdditionalCamera.TAAQuality = HDAdditionalCameraData.TAAQualityLevel.Low;
                break;
            case 1:
                hdAdditionalCamera.SMAAQuality = HDAdditionalCameraData.SMAAQualityLevel.Medium;
                hdAdditionalCamera.TAAQuality = HDAdditionalCameraData.TAAQualityLevel.Medium;
                break;
            case 2:
                hdAdditionalCamera.SMAAQuality = HDAdditionalCameraData.SMAAQualityLevel.High;
                hdAdditionalCamera.TAAQuality = HDAdditionalCameraData.TAAQualityLevel.High;
                break;
            default:
                hdAdditionalCamera.SMAAQuality = HDAdditionalCameraData.SMAAQualityLevel.Low;
                hdAdditionalCamera.TAAQuality = HDAdditionalCameraData.TAAQualityLevel.Low;
                break;
        }
    }

    /// <summary>
    /// 设置摄像机优先级 priority越高越优先
    /// </summary>
    /// <param name="cinemachineVirtual"></param>
    /// <param name="priority"></param>
    public void ChangeCameraPriority(CinemachineVirtualCameraBase cinemachineVirtual, int priority)
    {
        if (cinemachineVirtual != null)
            cinemachineVirtual.Priority = priority;
    }

    /// <summary>
    ///  修改摄像头距离
    /// </summary>
    /// <param name="distance">距离：0为第一人称，0-1距离依次增加</param>
    public void ChangeCameraDistance(float distance)
    {
        CinemachineVirtualCamera cameraForFirst = manager.cameraForFirst;
        CinemachineFreeLook cameraForThree = manager.cameraForThree;

        if (distance <= 0)
        {
            ChangeCameraPriority(cameraForFirst, 1);
            ChangeCameraPriority(cameraForThree, 0);
        }
        else
        {
            ChangeCameraPriority(cameraForFirst, 0);
            ChangeCameraPriority(cameraForThree, 1);
            for (int i = 0; i < cameraForThree.m_Orbits.Length; i++)
            {
                cameraForThree.m_Orbits[i].m_Height = manager.threeFreeLookOriginalOrbits[i].m_Height * distance;
                cameraForThree.m_Orbits[i].m_Radius = manager.threeFreeLookOriginalOrbits[i].m_Radius * distance;
            }
        }

        //隐藏或显示自己的头
        //if (player != null)
        //{
        //    CreatureCptCharacter creatureCpt = player.GetCharacter();
        //    creatureCpt.SetActiveHead(isShowHead);
        //}

        //保存数据
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        userData.userSetting.cameraDistance = distance;
    }


    /// <summary>
    /// 修改摄像头速度-玩家
    /// </summary>
    /// <param name="speed"></param>
    public void ChangeCameraSpeedForPlayer(float xSpeed, float ySpeed)
    {
        CinemachineVirtualCamera cameraForFirst = manager.cameraForFirst;
        //第一人称
        CinemachinePOV cinemachinePOV = cameraForFirst.GetCinemachineComponent<CinemachinePOV>();
        if (cinemachinePOV != null)
        {
            cinemachinePOV.m_VerticalAxis.m_MaxSpeed = ySpeed / timeScale;
            cinemachinePOV.m_HorizontalAxis.m_MaxSpeed = xSpeed / timeScale;
        }

        //第三人称
        CinemachineFreeLook cameraForThree = manager.cameraForThree;
        if (cameraForThree != null)
        {
            cameraForThree.m_XAxis.m_MaxSpeed = xSpeed / timeScale;
            cameraForThree.m_YAxis.m_MaxSpeed = ySpeed / 100 / timeScale;
        }
    }

    /// <summary>
    /// 修改摄像头速度-建筑编辑
    /// </summary>
    /// <param name="xSpeed"></param>
    /// <param name="ySpeed"></param>
    public void ChangeCameraSpeedForBuildingEditor(float xSpeed, float ySpeed)
    {
        //建筑编辑摄像头
        CinemachineFreeLook cameraForBuildingEditor = manager.cameraForBuildingEditor;
        if (cameraForBuildingEditor != null)
        {
            cameraForBuildingEditor.m_XAxis.m_MaxSpeed = xSpeed / timeScale;
            cameraForBuildingEditor.m_YAxis.m_MaxSpeed = ySpeed / 100 / timeScale;
        }
    }

    /// <summary>
    /// 设置摄像头角度
    /// </summary>
    /// <param name="vAxis"></param>
    /// <param name="hAxis"></param>
    public void SetCameraAxis(float vAxis, float hAxis)
    {
        CinemachineVirtualCamera cameraForFirst = manager.cameraForFirst;
        //第一人称
        CinemachinePOV cinemachinePOV = cameraForFirst.GetCinemachineComponent<CinemachinePOV>();
        if (cinemachinePOV != null)
        {
            cinemachinePOV.m_VerticalAxis.Value = vAxis;
            cinemachinePOV.m_HorizontalAxis.Value = hAxis;
        }

        CinemachineFreeLook cameraForThree = manager.cameraForThree;
        //第三人称
        cameraForThree.m_XAxis.Value = vAxis;
        cameraForThree.m_YAxis.Value = hAxis;
    }

    /// <summary>
    /// 设置镜头视距
    /// </summary>
    /// <param name="fieldOfView"></param>
    public void SetCameraFieldOfView(float fieldOfView)
    {
        if (fieldOfView > maxOrthographicSize)
        {
            fieldOfView = maxOrthographicSize;
        }
        else if (fieldOfView < minOrthographicSize)
        {
            fieldOfView = minOrthographicSize;
        }
        CinemachineVirtualCamera cameraForFirst = manager.cameraForFirst;
        //第一人称
        if (cameraForFirst != null)
            cameraForFirst.m_Lens.FieldOfView = fieldOfView;

        //第三人称
        CinemachineFreeLook cameraForThree = manager.cameraForThree;
        if (cameraForThree != null)
            cameraForThree.m_Lens.FieldOfView = fieldOfView;
    }
}
