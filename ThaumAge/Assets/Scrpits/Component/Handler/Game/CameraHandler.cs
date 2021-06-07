﻿using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : BaseHandler<CameraHandler, CameraManager>
{
    protected float maxAroundY = 90;
    protected float minAroundY = -60;

    protected float maxOrthographicSize = 100;
    protected float minOrthographicSize = 20;

    protected float maxCameraDis = 10;
    protected float minCameraDis = 0;

    public float timeScale = 0;

    private void Update()
    {
        if (timeScale != Time.timeScale)
        {
            timeScale = Time.timeScale;
            ChangeCameraSpeed(manager.speedForCameraMove); 
        }
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    public void InitData()
    {
        CinemachineVirtualCameraBase[] listCinemachineFreeLook = manager.listCameraFreeLook;
        if (CheckUtil.ArrayIsNull(listCinemachineFreeLook))
            return;
        Player player = GameHandler.Instance.manager.player;
        for (int i = 0; i < listCinemachineFreeLook.Length; i++)
        {
            CinemachineVirtualCameraBase cinemachine = listCinemachineFreeLook[i];
            if(cinemachine is CinemachineFreeLook)
            {
                //第三人称
                cinemachine.Follow = player.LookForThird;
                cinemachine.LookAt = player.LookForThird;
            }
            else
            {
                //第一人称
                cinemachine.Follow = player.LookForFirst;
            }
        }
    }


    /// <summary>
    ///  修改摄像头距离
    /// </summary>
    /// <param name="distance">距离：0为第一人称，123依次递增</param>
    public void ChangeCameraDistance(int distance)
    {
        CinemachineVirtualCameraBase[] listCinemachineFreeLook = manager.listCameraFreeLook;
        if (CheckUtil.ArrayIsNull(listCinemachineFreeLook))
            return;
        for (int i = 0; i < listCinemachineFreeLook.Length; i++)
        {
            CinemachineVirtualCameraBase cinemachineFreeLook = listCinemachineFreeLook[i];
            if (cinemachineFreeLook.name.Contains(distance + ""))
            {
                cinemachineFreeLook.Priority = 1;
            }
            else
            {
                cinemachineFreeLook.Priority = 0;
            }
        }
    }

    public void EnabledCameraMove(bool enabled)
    {
        if (enabled)
        {
            ChangeCameraSpeed(manager.speedForCameraMove);
        }
        else
        {
            ChangeCameraSpeed(0);
        }
    }

    public void ChangeCameraSpeed(float speed)
    {
        CinemachineVirtualCameraBase[] listCinemachineFreeLook = manager.listCameraFreeLook;
        if (CheckUtil.ArrayIsNull(listCinemachineFreeLook))
            return;
        for (int i = 0; i < listCinemachineFreeLook.Length; i++)
        {
            CinemachineVirtualCameraBase cinemachine = listCinemachineFreeLook[i];
            if (cinemachine is CinemachineFreeLook)
            {
                //第三人称
                CinemachineFreeLook cinemachineFreeLook = cinemachine as CinemachineFreeLook;
                cinemachineFreeLook.m_XAxis.m_MaxSpeed = speed / timeScale;
                cinemachineFreeLook.m_YAxis.m_MaxSpeed = (speed / 30) / timeScale;
            }
            else
            {
                //第一人称
                CinemachineVirtualCamera cinemachineVirtualCamera = cinemachine as CinemachineVirtualCamera;
                CinemachinePOV cinemachinePOV = cinemachineVirtualCamera.GetCinemachineComponent<CinemachinePOV>();
                if (cinemachinePOV == null)
                    continue;
                cinemachinePOV.m_VerticalAxis.m_MaxSpeed = speed / timeScale;
                cinemachinePOV.m_HorizontalAxis.m_MaxSpeed = speed / timeScale;
            }
        }
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
