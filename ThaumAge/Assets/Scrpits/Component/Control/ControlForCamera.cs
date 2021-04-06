using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlForCamera : ControlForBase
{
    protected Vector2 lookData = Vector2.zero;
    protected float offsetDistance;//摄像机对于角色的差量
    private void Awake()
    {
        InputAction disAction = InputHandler.Instance.manager.GetCameraDistanceData();
        disAction.started += HandleForDistance;
    }

    private void Start()
    {
        InitOffsetDistance();
    }

    public void InitOffsetDistance()
    {
        //获取摄像机相对于角色的差量
        Vector3 characterPosition = GameControlHandler.Instance.manager.controlForPlayer.transform.position;
        Vector3 cameraPosition = CameraHandler.Instance.manager.mainCamera.transform.position;
        offsetDistance = Vector3.Distance(cameraPosition, characterPosition);
    }

    private void LateUpdate()
    {
        HandleForPosition();
        HandleForLookAround();
        HandleForDistanceUpdate();
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
            CameraHandler.Instance.SetCameraDistance(characterPosition, 1, 0.5f);
        }
        else if (data < 0)
        {
            CameraHandler.Instance.SetCameraDistance(characterPosition, -1, 0.5f);
        }
        InitOffsetDistance();
    }

    /// <summary>
    /// 距离更新处理
    /// </summary>
    public void HandleForDistanceUpdate()
    {
        //获得由物体射向摄像机的射线以及碰到的所有物体hits
        Vector3 characterPosition = GameControlHandler.Instance.manager.controlForPlayer.transform.position;
        Vector3 cameraPosition = CameraHandler.Instance.manager.mainCamera.transform.position;
        Vector3 dir = -(cameraPosition - characterPosition).normalized;
        float disTemp = Vector3.Distance(characterPosition, cameraPosition);
        bool isHit = RayUtil.RayToCast(characterPosition, -dir, disTemp, 1 << LayerInfo.Chunk, out RaycastHit hit);

        //有碰到且物体没有移走，就拉近
        if (isHit)
        {
            float disHitTemp = Vector3.Distance(characterPosition, hit.point);
            if (disTemp > disHitTemp)
            {
                CameraHandler.Instance.SetCameraDistance(characterPosition, 1, 0.1f);
            }
        }
        else
        {
            //射线检测后面0.5是否有物体。如果没有则摄像头向后移动
            bool isNormalHit = RayUtil.RayToCast(characterPosition, -dir, disTemp + 0.5f, 1 << LayerInfo.Chunk);
            if (!isNormalHit && disTemp < offsetDistance)
            {
                CameraHandler.Instance.SetCameraDistance(characterPosition, -1, 0.1f);
            }
        }
    }
}