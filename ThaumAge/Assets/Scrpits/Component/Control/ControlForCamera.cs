using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlForCamera : ControlForBase
{
    protected Vector2 lookData = Vector2.zero;

    private void Awake()
    {
        InputAction disAction = InputHandler.Instance.manager.GetCameraDistanceData();
        disAction.started += HandleForDistance;
    }

    private void LateUpdate()
    {
        HandleForPosition();
        HandleForLookAround();
    }

    public void HandleForPosition()
    {
        transform.position = GameHandler.Instance.manager.player.transform.position;
    }

    /// <summary>
    /// 环绕处理
    /// </summary>
    public void HandleForLookAround()
    {
        InputAction lookAction = InputHandler.Instance.manager.GetLookData();
        Vector2 tempLookData = lookAction.ReadValue<Vector2>();
        if (tempLookData != Vector2.zero)
        {
            lookData = tempLookData;
        }
        Vector3 characterPosition = GameControlHandler.Instance.manager.controlForPlayer.transform.position;
        lookData = Vector2.Lerp(lookData, Vector2.zero, 0.06f);
        CameraHandler.Instance.RotateCameraAroundXZ(characterPosition, lookData.x, 20);
        CameraHandler.Instance.RotateCameraAroundY(characterPosition, -lookData.y, 20);
    }

    /// <summary>
    /// 镜头远景处理
    /// </summary>
    public void HandleForDistance(InputAction.CallbackContext callBack)
    {
        Vector3 characterPosition = GameControlHandler.Instance.manager.controlForPlayer.transform.position;
        float data = callBack.ReadValue<float>();
        if (data > 0)
        {
            CameraHandler.Instance.SetCameraDistance(characterPosition, 1, 1);
        }
        else if (data < 0)
        {
            CameraHandler.Instance.SetCameraDistance(characterPosition ,- 1, 1);
        }
    }
}