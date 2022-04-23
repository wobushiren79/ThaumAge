﻿using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class CameraHandler : BaseHandler<CameraHandler, CameraManager>
{
    protected float maxAroundY = 90;
    protected float minAroundY = -60;

    protected float maxOrthographicSize = 100;
    protected float minOrthographicSize = 20;

    protected float maxCameraDis = 10;
    protected float minCameraDis = 0;

    public float timeScale = 1;

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
    /// 是否开启摄像头移动
    /// </summary>
    /// <param name="enabled"></param>
    public void EnabledCameraMove(bool enabled)
    {
        if (enabled)
        {
            GameConfigBean gameConfig = GameDataHandler.Instance.manager.GetGameConfig();
            ChangeCameraSpeed(gameConfig.speedForPlayerCameraMoveX, gameConfig.speedForPlayerCameraMoveY);
        }
        else
        {
            ChangeCameraSpeed(0, 0);
        }
    }

    /// <summary>
    /// 修改摄像头速度
    /// </summary>
    /// <param name="speed"></param>
    public void ChangeCameraSpeed(float xSpeed, float ySpeed)
    {
        CinemachineVirtualCamera cameraForFirst = manager.cameraForFirst;
        //第一人称
        CinemachinePOV cinemachinePOV = cameraForFirst.GetCinemachineComponent<CinemachinePOV>();
        if (cinemachinePOV != null)
        {
            cinemachinePOV.m_VerticalAxis.m_MaxSpeed = ySpeed / timeScale;
            cinemachinePOV.m_HorizontalAxis.m_MaxSpeed = xSpeed / timeScale;
        }

        CinemachineFreeLook cameraForThree = manager.cameraForThree;
        //第三人称
        cameraForThree.m_XAxis.m_MaxSpeed = xSpeed / timeScale;
        cameraForThree.m_YAxis.m_MaxSpeed = ySpeed / 100 / timeScale;
    }

    /// <summary>
    /// 旋转镜头
    /// </summary>
    /// <param name="aroundPosition"></param>
    /// <param name="rotateOffset"></param>
    /// <param name="speedForMove"></param>
    public void RotateCameraAroundXZ(Vector3 aroundPosition, float rotateOffset, float speedForMove)
    {
        manager.mainCamera.transform.RotateAround(aroundPosition, Vector3.up, rotateOffset * Time.unscaledDeltaTime * speedForMove);
    }

    /// <summary>
    /// 旋转镜头
    /// </summary>
    /// <param name="aroundPosition"></param>
    /// <param name="rotateOffset"></param>
    /// <param name="speedForMove"></param>
    public void RotateCameraAroundY(Vector3 aroundPosition, float rotateOffset, float speedForMove)
    {
        Vector3 eulerAngles = manager.mainCamera.transform.rotation.eulerAngles;
        float tempAngles = rotateOffset * Time.unscaledDeltaTime * speedForMove;
        float afterAngles = eulerAngles.x + tempAngles;
        if (afterAngles > 180)
        {
            afterAngles -= 360;
        }
        if (afterAngles > 0)
        {
            if (afterAngles >= maxAroundY)
                return;
        }
        else
        {
            if (afterAngles <= minAroundY)
                return;
        }
        manager.mainCamera.transform.RotateAround(aroundPosition, manager.mainCamera.transform.right, tempAngles);
    }

    /// <summary>
    /// 缩放镜头
    /// </summary>
    /// <param name="size"></param>
    public void ZoomCamera(float zoomOffset, float speedForZoom)
    {
        SetCameraFieldOfView(zoomOffset * Time.unscaledDeltaTime * speedForZoom + manager.mainCamera.fieldOfView);
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
        manager.mainCamera.fieldOfView = fieldOfView;
    }


    /// <summary>
    /// 设置摄像头距离
    /// </summary>
    public void SetCameraDistance(Vector3 targetPosition, int data, float Speed)
    {
        Vector3 oldPosition = manager.mainCamera.transform.position;
        float distance = Vector3.Distance(targetPosition, oldPosition);
        if (data > 0 && distance <= minCameraDis)
            return;
        if (data < 0 && distance >= maxCameraDis)
            return;

        manager.mainCamera.transform.position = Vector3.MoveTowards(oldPosition, targetPosition, data * Speed);
        //如果点重合了 则回到原来的点
        if (Vector3.Distance(targetPosition, manager.mainCamera.transform.position) <= minCameraDis)
        {
            manager.mainCamera.transform.position = Vector3.MoveTowards(oldPosition, targetPosition, -maxCameraDis * data);
        }
    }
}
