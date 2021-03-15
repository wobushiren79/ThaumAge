using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : BaseHandler<CameraHandler, CameraManager>
{
    protected float maxAroundY = 60;
    protected float minAroundY = 0;

    protected float maxOrthographicSize = 100;
    protected float minOrthographicSize = 20;

    /// <summary>
    /// 旋转镜头
    /// </summary>
    /// <param name="aroundPosition"></param>
    /// <param name="rotateOffset"></param>
    /// <param name="speedForMove"></param>
    public void RotateCameraAroundXZ(Vector3 aroundPosition, float rotateOffset, float speedForMove)
    {
        manager.mainCamera.transform.RotateAround(aroundPosition, Vector3.up, rotateOffset * Time.deltaTime * speedForMove);
    }

    /// <summary>
    /// 旋转镜头
    /// </summary>
    /// <param name="aroundPosition"></param>
    /// <param name="rotateOffset"></param>
    /// <param name="speedForMove"></param>
    public void RotateCameraAroundY(Vector3 aroundPosition, float rotateOffset, float speedForMove)
    {

        Vector3 eulerAngles = manager.mainCamera.transform.eulerAngles;
        float tempAngles = rotateOffset * Time.deltaTime * speedForMove;
        if (rotateOffset > 0 && eulerAngles.x + tempAngles >= maxAroundY)
            return;
        if (rotateOffset < 0 && eulerAngles.x + tempAngles <= minAroundY)
            return;
        manager.mainCamera.transform.RotateAround(aroundPosition, manager.mainCamera.transform.right, tempAngles);
    }

    /// <summary>
    /// 缩放镜头
    /// </summary>
    /// <param name="size"></param>
    public void ZoomCamera(float zoomOffset, float speedForZoom)
    {
        SetCameraFieldOfView(zoomOffset * Time.deltaTime * speedForZoom + manager.mainCamera.fieldOfView);
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
}
