using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class ControlForPlayer : ControlForBase
{
    public CharacterController characterController;

    private float gravityValue = 10f;
    private Vector3 playerVelocity;
    private bool isJump = false;

    private float timeJump = 0.2f;
    private float timeJumpTemp = 0;

    private float speedJump = 5;

    private void Awake()
    {
        InputAction jumpAction = InputHandler.Instance.manager.GetJumpData();
        jumpAction.started += HandlerForJumpStart;
        InputAction useAction = InputHandler.Instance.manager.GetUseData();
        useAction.started += HandlerForUse;
        InputAction cancelAction = InputHandler.Instance.manager.GetCancelData();
        cancelAction.started += HandleForCancel;

    }

    private void Update()
    {
        if (GameHandler.Instance.manager.GetGameState()== GameStateEnum.Gaming)
        {
            HandlerForMoveAndJump();
        }
    }

    /// <summary>
    /// 开关控制
    /// </summary>
    /// <param name="enabled"></param>
    public override void EnabledControl(bool enabled)
    {
        base.EnabledControl(enabled);
        characterController.enabled = enabled;
    }

    /// <summary>
    /// 移动处理
    /// </summary>
    public void HandlerForMoveAndJump()
    {
        InputAction moveAction = InputHandler.Instance.manager.GetMoveData();
        Vector2 moveData = moveAction.ReadValue<Vector2>();
        //旋转角色
        RotateCharacter(moveData, 5);
        //移动角色
        MoveCharacter(moveData, 1);
        //跳跃处理
        if (isJump)
        {
            if (timeJumpTemp <= timeJump)
            {
                playerVelocity.y = speedJump * Time.unscaledDeltaTime;
                timeJumpTemp += Time.unscaledDeltaTime;
            }
            else
            {
                playerVelocity.y -= gravityValue * Time.unscaledDeltaTime;
                if (characterController.isGrounded)
                {
                    timeJumpTemp = 0;
                    isJump = false;
                }
            }
        }
        else
        {
            playerVelocity.y -= gravityValue * Time.unscaledDeltaTime;
        }
        characterController.Move(playerVelocity);
    }

    /// <summary>
    /// 开始跳跃处理
    /// </summary>
    public void HandlerForJumpStart(CallbackContext callback)
    {
        isJump = true;
    }

    /// <summary>
    /// 使用处理
    /// </summary>
    public void HandlerForUse(CallbackContext callback)
    {
        //获取摄像头到角色的距离
        Vector3 cameraPosition = CameraHandler.Instance.manager.mainCamera.transform.position;
        float disMax = Vector3.Distance(cameraPosition, transform.position);
        //发射射线检测
        RayUtil.RayToScreenPointForScreenCenter(disMax + 2, 1 << LayerInfo.Chunk, out bool isCollider, out RaycastHit hit);
        if (isCollider)
        {
            float disHit = Vector3.Distance(cameraPosition, hit.point);
            if (disHit < disMax)
                return;
            Chunk chunk = hit.collider.GetComponentInParent<Chunk>();
            if (chunk)
            {
                Vector3Int position = Vector3Int.zero;
                Vector3Int addPosition = Vector3Int.zero;
                DirectionEnum direction = DirectionEnum.UP;
                if (hit.normal.y > 0)
                {
                    position = new Vector3Int((int)Mathf.Floor(hit.point.x), (int)Mathf.Floor(hit.point.y) - 1, (int)Mathf.Floor(hit.point.z));
                    addPosition = position + Vector3Int.up;
                    direction = DirectionEnum.UP;
                }
                else if (hit.normal.y < 0)
                {
                    position = new Vector3Int((int)Mathf.Floor(hit.point.x), (int)Mathf.Floor(hit.point.y), (int)Mathf.Floor(hit.point.z));
                    addPosition = position + Vector3Int.down;
                    direction = DirectionEnum.Down;
                }
                else if (hit.normal.x > 0)
                {
                    position = new Vector3Int((int)Mathf.Floor(hit.point.x) - 1, (int)Mathf.Floor(hit.point.y), (int)Mathf.Floor(hit.point.z));
                    addPosition = position + Vector3Int.right;
                    direction = DirectionEnum.Right;
                }
                else if (hit.normal.x < 0)
                {
                    position = new Vector3Int((int)Mathf.Floor(hit.point.x), (int)Mathf.Floor(hit.point.y), (int)Mathf.Floor(hit.point.z));
                    addPosition = position + Vector3Int.left;
                    direction = DirectionEnum.Left;
                }
                else if (hit.normal.z > 0)
                {
                    position = new Vector3Int((int)Mathf.Floor(hit.point.x), (int)Mathf.Floor(hit.point.y), (int)Mathf.Floor(hit.point.z) - 1);
                    addPosition = position + Vector3Int.forward;
                    direction = DirectionEnum.Forward;
                }
                else if (hit.normal.z < 0)
                {
                    position = new Vector3Int((int)Mathf.Floor(hit.point.x), (int)Mathf.Floor(hit.point.y), (int)Mathf.Floor(hit.point.z));
                    addPosition = position + Vector3Int.back;
                    direction = DirectionEnum.Back;
                }
                Block block = WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(addPosition);
                if(block != null)
                {
                    block.chunk.SetBlockForWorld(addPosition, BlockTypeEnum.Water);
                }    
            }
        }
    }

    public void HandleForCancel(CallbackContext callback)
    {
        //获取摄像头到角色的距离
        Vector3 cameraPosition = CameraHandler.Instance.manager.mainCamera.transform.position;
        float disMax = Vector3.Distance(cameraPosition, transform.position);
        //发射射线检测
        RayUtil.RayToScreenPointForScreenCenter(disMax+ 2, 1 << LayerInfo.Chunk, out bool isCollider, out RaycastHit hit);
        if (isCollider)
        {
            float disHit = Vector3.Distance(cameraPosition, hit.point);
            if (disHit < disMax)
                return;
            Chunk chunk = hit.collider.GetComponentInParent<Chunk>();
            if (chunk)
            {
                Vector3Int position = Vector3Int.zero; ;
                if (hit.normal.y > 0)
                {
                     position = new Vector3Int((int)Mathf.Floor(hit.point.x), (int)Mathf.Floor(hit.point.y) - 1, (int)Mathf.Floor(hit.point.z));
                }
                else if (hit.normal.y < 0)
                {
                     position = new Vector3Int((int)Mathf.Floor(hit.point.x), (int)Mathf.Floor(hit.point.y), (int)Mathf.Floor(hit.point.z));
                }
                else if (hit.normal.x > 0)
                {
                     position = new Vector3Int((int)Mathf.Floor(hit.point.x) - 1, (int)Mathf.Floor(hit.point.y), (int)Mathf.Floor(hit.point.z));
                }
                else if (hit.normal.x < 0)
                {
                     position = new Vector3Int((int)Mathf.Floor(hit.point.x), (int)Mathf.Floor(hit.point.y), (int)Mathf.Floor(hit.point.z));
                }
                else if (hit.normal.z > 0)
                {
                     position = new Vector3Int((int)Mathf.Floor(hit.point.x), (int)Mathf.Floor(hit.point.y), (int)Mathf.Floor(hit.point.z) - 1);
                }
                else if (hit.normal.z < 0)
                {
                     position = new Vector3Int((int)Mathf.Floor(hit.point.x), (int)Mathf.Floor(hit.point.y), (int)Mathf.Floor(hit.point.z)); 
                }
                chunk.RemoveBlockForWorld(position);
            }
        }
    }

    /// <summary>
    /// 角色移动
    /// </summary>
    /// <param name="movePosition"></param>
    public void MoveCharacter(Vector2 moveOffset, float moveSpeed)
    {
        Camera mainCamera = CameraHandler.Instance.manager.mainCamera;
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;
        //获取摄像机方向 高度为0
        forward.y = 0;
        right.y = 0;
        //朝摄像头方向移动
        playerVelocity = (Vector3.Normalize(forward) * moveOffset.y + Vector3.Normalize(right) * moveOffset.x) * Time.unscaledDeltaTime * moveSpeed * 5;
    }


    /// <summary>
    /// 角色旋转
    /// </summary>
    /// <param name="targetPosition"></param>
    public void RotateCharacter(Vector2 moveOffset, float rotateSpeed)
    {
        Camera mainCamera = CameraHandler.Instance.manager.mainCamera;
        Vector3 rotateAngles = new Vector3(0, mainCamera.transform.rotation.eulerAngles.y, 0);
        //前进后退的旋转
        if (moveOffset.y > 0)
        {
            rotateAngles.y += 0;
        }
        else if (moveOffset.y < 0)
        {
            rotateAngles.y += 180;
        }

        //左右移动的旋转
        if (moveOffset.x > 0)
        {
            rotateAngles.y += 90;
        }
        else if (moveOffset.x < 0)
        {
            rotateAngles.y += -90;
        }

        Quaternion rotate = Quaternion.Euler(rotateAngles);
        //朝摄像头方向移动
        characterController.transform.rotation = Quaternion.Slerp(transform.rotation, rotate, rotateSpeed * Time.unscaledDeltaTime);
    }

}