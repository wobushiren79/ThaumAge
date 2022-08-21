using JetBrains.Annotations;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlForCamera : ControlForBase
{

    public float cameraDistance = 0;


    private void Awake()
    {
        InputAction disAction = InputHandler.Instance.manager.GetInputPlayerData("CameraDistance");
        disAction.started += HandleForDistance;
    }

    /// <summary>
    /// 开关控制
    /// </summary>
    /// <param name="enabled"></param>
    public override void EnabledControl(bool enabled)
    {
        base.EnabledControl(enabled);
        CameraHandler.Instance.EnabledCameraMove(enabled);
    }

    /// <summary>
    /// 镜头远景处理
    /// </summary>
    public void HandleForDistance(InputAction.CallbackContext callBack)
    {
        if (!enabledControl)
            return;
        if (!isActiveAndEnabled)
            return;
        float data = callBack.ReadValue<float>();
        if (data > 0)
        {
            cameraDistance -= 0.2f;
        }
        else if (data < 0)
        {
            cameraDistance += 0.2f;
        }
        if (cameraDistance > 1)
        {
            cameraDistance = 0;
        }
        else if (cameraDistance < 0)
        {
            cameraDistance = 1;
        }
        cameraDistance = (float)Math.Round(double.Parse($"{cameraDistance}"), 2);
        CameraHandler.Instance.ChangeCameraDistance(cameraDistance);
    }
}