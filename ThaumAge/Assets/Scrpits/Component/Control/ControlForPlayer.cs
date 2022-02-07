﻿using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class ControlForPlayer : ControlForBase
{
    private CharacterController characterController;
    private CreatureCptCharacter character;

    private float gravityValue = 10f;
    private Vector3 playerVelocity;
    private bool isJump = false;

    private float timeJump = 0.2f;
    private float timeJumpTemp = 0;

    private float speedJump = 5;
    public float moveSpeed = 1;
    private float speedCharacterRotate = 10;

    private InputAction inputActionUseL;
    private InputAction inputActionUseR;
    private InputAction inputActionUseF;

    private InputAction inputActionJump;
    private InputAction inputActionMove;
    private InputAction inputActionuserDetailsData;
    private InputAction inputActionUseDrop; 

    //是否正在使用道具
    private bool isUseItem = false;
    //正在使用道具的时间
    private float timeForUseItem;
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        character = GetComponentInChildren<CreatureCptCharacter>();

        inputActionJump = InputHandler.Instance.manager.GetInputPlayerData("Jump");
        inputActionJump.started += HandleForJumpStart;

        inputActionUseL = InputHandler.Instance.manager.GetInputPlayerData("UseL");
        inputActionUseL.started += HandleForUseL;
        inputActionUseL.canceled += HandleForUseEnd;

        inputActionUseR = InputHandler.Instance.manager.GetInputPlayerData("UseR");
        inputActionUseR.started += HandleForUseR;
        inputActionUseR.canceled += HandleForUseEnd;

        inputActionUseF = InputHandler.Instance.manager.GetInputPlayerData("UseF");
        inputActionUseF.started += HandleForUseF;

        inputActionUseDrop = InputHandler.Instance.manager.GetInputPlayerData("Drop");
        inputActionUseDrop.started += HandleForDrop;

        inputActionuserDetailsData = InputHandler.Instance.manager.GetInputPlayerData("UserDetails");
        inputActionuserDetailsData.started += HandleForUserDetails;
        inputActionMove = InputHandler.Instance.manager.GetInputPlayerData("Move");

        InvokeRepeating("HandlerForUseItemTarget", 0.1f, 0.1f);
    }

    public void Update()
    {
        if (GameHandler.Instance.manager.GetGameState() == GameStateEnum.Gaming)
        {
            HandlerForMoveAndJumpUpdate();
            HandleForUseUpdate();
        }
    }

    public void OnDestroy()
    {
        CancelInvoke("HandlerForUseItemTarget");
        inputActionJump.started -= HandleForJumpStart;
        inputActionUseL.started -= HandleForUseL;
        inputActionUseL.canceled -= HandleForUseEnd;
        inputActionUseR.started -= HandleForUseR;
        inputActionUseR.canceled -= HandleForUseEnd;
        inputActionUseF.started -= HandleForUseF;
        inputActionuserDetailsData.started -= HandleForUserDetails;
        inputActionUseDrop.started -= HandleForDrop;
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
        UIGameUserDetails uiGameUserDetails = UIHandler.Instance.OpenUIAndCloseOther<UIGameUserDetails>(UIEnum.GameUserDetails);
        uiGameUserDetails.ui_ViewSynthesis.SetDataType(ItemsSynthesisTypeEnum.Self);
    }

    /// <summary>
    /// 移动处理
    /// </summary>
    public void HandlerForMoveAndJumpUpdate()
    {
        Vector2 moveData = inputActionMove.ReadValue<Vector2>();
        //旋转角色
        RotateCharacter(moveData, speedCharacterRotate);
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
        //播放跳跃动画
        character.characterAnim.creatureAnim.PlayJump(isJump);
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
    /// 处理-持续使用道具
    /// </summary>
    public void HandleForUseUpdate()
    {
        if (isUseItem)
        {
            timeForUseItem += Time.deltaTime;
            if (timeForUseItem > 0.25f)
            {
                HandleForUseL(new CallbackContext());
            }
            //转动方向
            RotateCharacter(Vector2.up, speedCharacterRotate);
        }
    }

    /// <summary>
    /// 处理-使用道具
    /// </summary>
    /// <param name="callback"></param>
    public void HandleForUseL(CallbackContext callback)
    {
        HandleForUse(callback, 0);
    }


    /// <summary>
    /// 取消处理
    /// </summary>
    /// <param name="callback"></param>
    public void HandleForUseR(CallbackContext callback)
    {
        HandleForUse(callback, 1);
    }

    /// <summary>
    /// 互动处理
    /// </summary>
    /// <param name="callback"></param>
    public void HandleForUseF(CallbackContext callback)
    {
        HandleForUse(callback, 2, false);
    }

    /// <summary>
    /// 丢弃处理
    /// </summary>
    /// <param name="callback"></param>
    public void HandleForDrop(CallbackContext callback)
    {
        //获取道具栏上的物品
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        ItemsBean itemData = userData.GetItemsFromShortcut();

        //丢出道具
        Player player = GameHandler.Instance.manager.player;
        Vector3 randomFroce = new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(0f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f));
        ItemsHandler.Instance.CreateItemCptDrop(itemData.itemId, 1, player.transform.position + Vector3.up, ItemDropStateEnum.DropNoPick, player.transform.forward + randomFroce);

        //扣除道具
        userData.AddItems(itemData, -1);
        //刷新UI
        UIHandler.Instance.RefreshUI();      
    }

    public void HandleForUse(CallbackContext callback, int type, bool isUserItem = true)
    {
        if (!isActiveAndEnabled)
            return;
        if (UGUIUtil.IsPointerUI())
            return;
        this.isUseItem = isUserItem;
        timeForUseItem = 0;
        //获取道具栏上的物品
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        ItemsBean itemsData = userData.GetItemsFromShortcut();
        Player player = GameHandler.Instance.manager.player;
        ItemsHandler.Instance.UseItem(player.gameObject, itemsData, type);
    }

    /// <summary>
    /// 处理-停止使用道具
    /// </summary>
    /// <param name="callback"></param>
    public void HandleForUseEnd(CallbackContext callback)
    {
        isUseItem = false;
        character.characterAnim.creatureAnim.PlayUse(false);
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
            character.characterAnim.creatureAnim.PlayBaseAnim(CharacterAnimBaseState.Idle);
        }
        else
        {
            character.characterAnim.creatureAnim.PlayBaseAnim(CharacterAnimBaseState.Walk);
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