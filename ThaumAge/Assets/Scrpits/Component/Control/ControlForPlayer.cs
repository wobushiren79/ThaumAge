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
    }

    private void Update()
    {
        HandlerForMoveAndJump();
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
                playerVelocity.y = speedJump * Time.deltaTime;
                timeJumpTemp += Time.deltaTime;
            }
            else
            {
                playerVelocity.y -= gravityValue * Time.deltaTime;
                if (characterController.isGrounded)
                {
                    timeJumpTemp = 0;
                    isJump = false;
                }
            }
        }
        else
        {
            playerVelocity.y -= gravityValue * Time.deltaTime;
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
        RayUtil.RayToScreenPointForScreenCenter(10, 1 << LayerInfo.Chunk, out bool isCollider, out RaycastHit hit);
        if (isCollider)
        {
            Chunk chunk = hit.collider.GetComponent<Chunk>();
            if (chunk)
            {

                if (hit.normal.y > 0)
                {
                    Vector3Int position = new Vector3Int((int)Mathf.Floor(hit.point.x), (int)Mathf.Floor(hit.point.y) - 1, (int)Mathf.Floor(hit.point.z));
                    //chunk.RemoveBlock(position);
                    chunk.SetBlock(position + Vector3Int.up, BlockTypeEnum.Weed);
                }
                else if (hit.normal.y < 0)
                {
                    Vector3Int position = new Vector3Int((int)Mathf.Floor(hit.point.x), (int)Mathf.Floor(hit.point.y), (int)Mathf.Floor(hit.point.z));
                    chunk.RemoveBlock(position);
                }
                else if (hit.normal.x > 0)
                {
                    Vector3Int position = new Vector3Int((int)Mathf.Floor(hit.point.x) - 1, (int)Mathf.Floor(hit.point.y), (int)Mathf.Floor(hit.point.z));
                    chunk.RemoveBlock(position);
                }
                else if (hit.normal.x < 0)
                {
                    Vector3Int position = new Vector3Int((int)Mathf.Floor(hit.point.x), (int)Mathf.Floor(hit.point.y), (int)Mathf.Floor(hit.point.z));
                    chunk.RemoveBlock(position);
                }
                else if (hit.normal.z > 0)
                {
                    Vector3Int position = new Vector3Int((int)Mathf.Floor(hit.point.x), (int)Mathf.Floor(hit.point.y), (int)Mathf.Floor(hit.point.z) - 1);
                    chunk.RemoveBlock(position);
                }
                else if (hit.normal.z < 0)
                {
                    Vector3Int position = new Vector3Int((int)Mathf.Floor(hit.point.x), (int)Mathf.Floor(hit.point.y), (int)Mathf.Floor(hit.point.z));
                    chunk.RemoveBlock(position);
                }
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
        playerVelocity = (Vector3.Normalize(forward) * moveOffset.y + Vector3.Normalize(right) * moveOffset.x) * Time.deltaTime * moveSpeed * 5;
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
        characterController.transform.rotation = Quaternion.Slerp(transform.rotation, rotate, rotateSpeed * Time.deltaTime);
    }
}