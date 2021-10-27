using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class ControlForPlayer : ControlForBase
{
    private CharacterController characterController;
    private Character character;

    private float gravityValue = 10f;
    private Vector3 playerVelocity;
    private bool isJump = false;

    private float timeJump = 0.2f;
    private float timeJumpTemp = 0;

    private float speedJump = 5;
    private float moveSpeed = 1;

    private InputAction inputActionMove;
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        character = GetComponentInChildren<Character>();

        InputAction jumpAction = InputHandler.Instance.manager.GetInputPlayerData("Jump");
        jumpAction.started += HandleForJumpStart;
        InputAction useAction = InputHandler.Instance.manager.GetInputPlayerData("Use");
        useAction.started += HandleForUse;
        InputAction cancelAction = InputHandler.Instance.manager.GetInputPlayerData("Cancel");
        cancelAction.started += HandleForCancel;
        InputAction userDetailsData = InputHandler.Instance.manager.GetInputPlayerData("UserDetails");
        userDetailsData.started += HandleForUserDetails;
        inputActionMove = InputHandler.Instance.manager.GetInputPlayerData("Move");

        InvokeRepeating("HandlerForUseItemTarget", 0.1f, 0.1f);
    }

    private void Update()
    {
        if (GameHandler.Instance.manager.GetGameState() == GameStateEnum.Gaming)
        {
            HandlerForMoveAndJump();
        }
    }

    private void OnDestroy()
    {
        CancelInvoke("HandlerForUseItemTarget");
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
    /// 打开用户详情
    /// </summary>
    /// <param name="callback"></param>
    public void HandleForUserDetails(CallbackContext callback)
    {
        if (!isActiveAndEnabled)
            return;
        UIHandler.Instance.OpenUIAndCloseOther<UIGameUserDetails>(UIEnum.GameUserDetails);
    }

    /// <summary>
    /// 移动处理
    /// </summary>
    public void HandlerForMoveAndJump()
    {
        Vector2 moveData = inputActionMove.ReadValue<Vector2>();
        //旋转角色
        RotateCharacter(moveData, 5);
        //移动角色
        MoveCharacter(moveData, moveSpeed);
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
    public void HandleForJumpStart(CallbackContext callback)
    {
        if (!isActiveAndEnabled)
            return;
        isJump = true;
    }

    /// <summary>
    /// 处理-使用道具目标
    /// </summary>
    public void HandlerForUseItemTarget()
    {
        if (!isActiveAndEnabled)
            return;
        //获取道具栏上的物品
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        ItemsBean itemsData = userData.GetItemsFromShortcut();
        ItemsHandler.Instance.UseItemTarget(itemsData);
    }

    /// <summary>
    /// 处理-使用道具
    /// </summary>
    /// <param name="callback"></param>
    public void HandleForUse(CallbackContext callback)
    {
        if (!isActiveAndEnabled)
            return;
        if (UGUIUtil.IsPointerUI())
            return;
        //获取道具栏上的物品
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        ItemsBean itemsData = userData.GetItemsFromShortcut();
        ItemsHandler.Instance.UseItem(itemsData);
    }

    /// <summary>
    /// 取消处理
    /// </summary>
    /// <param name="callback"></param>
    public void HandleForCancel(CallbackContext callback)
    {
        if (!isActiveAndEnabled)
            return;
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

        if (moveOffset.x == 0 && moveOffset.y == 0)
        {
            character.characterAnim.PlayBaseAnim(CharacterAnimBaseState.Idle);
        }
        else
        {
            character.characterAnim.PlayBaseAnim(CharacterAnimBaseState.Walk);
        }
    }

    /// <summary>
    /// 角色旋转
    /// </summary>
    /// <param name="targetPosition"></param>
    public void RotateCharacter(Vector2 moveOffset, float rotateSpeed)
    {
        if (moveOffset == Vector2.zero)
            return;
        Camera mainCamera = CameraHandler.Instance.manager.mainCamera;
        Vector3 rotateAngles = new Vector3(0, mainCamera.transform.rotation.eulerAngles.y, 0);
        //前进后退的旋转
        if (moveOffset.y > 0)
        {
            //左右移动的旋转
            if (moveOffset.x > 0)
            {
                rotateAngles.y += 45;
            }
            else if (moveOffset.x < 0)
            {
                rotateAngles.y += -45;
            }
            else
            {
                rotateAngles.y += 0;
            }
        }
        else if (moveOffset.y < 0)
        {
            //左右移动的旋转
            if (moveOffset.x > 0)
            {
                rotateAngles.y += 135;
            }
            else if (moveOffset.x < 0)
            {
                rotateAngles.y += -135;
            }
            else
            {
                rotateAngles.y += 180;
            }
        }
        else
        {
            //左右移动的旋转
            if (moveOffset.x > 0)
            {
                rotateAngles.y += 90;
            }
            else if (moveOffset.x < 0)
            {
                rotateAngles.y += -90;
            }
        }
        Quaternion rotate = Quaternion.Euler(rotateAngles);
        //朝摄像头方向移动
        characterController.transform.rotation = Quaternion.Slerp(transform.rotation, rotate, rotateSpeed * Time.unscaledDeltaTime);
    }

}