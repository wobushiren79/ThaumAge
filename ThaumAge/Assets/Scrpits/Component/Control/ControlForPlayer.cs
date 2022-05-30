﻿using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class ControlForPlayer : ControlForBase
{
    private CharacterController characterController;
    private CreatureCptCharacter character;

    //移动速度
    public float moveSpeed = 0.8f;
    //重力
    public float gravityValue = 10f;

    private Vector3 playerVelocity;
    //是否正在跳跃
    private bool isJump = false;
    //攀爬剩余时间
    private float timeClimbEnd = 0;

    private float timeJump = 0.2f;
    private float timeJumpTemp = 0;

    private float speedJump = 5;

    private float speedCharacterRotate = 10;

    private InputAction inputActionUseL;
    private InputAction inputActionUseR;
    private InputAction inputActionUseFace;

    private InputAction inputActionJump;
    private InputAction inputActionMove;
    private InputAction inputActionUserDetailsData;
    private InputAction inputActionUseDrop;
    private InputAction inputActionShift;
    private InputAction inputActionCtrl;

    //是否正在使用道具
    private bool isUseItem = false;
    //正在使用道具的时间
    protected float timeUpdateForUseItem = 0;
    protected float timeUpdateMaxForUseItem = 0.25f;

    //目标选择更新间隔
    protected float timeUpdateForUseItemSightTarget = 0;
    protected float timeUpdateMaxForUseItemSightTarget = 0.1f;

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

        inputActionUseFace = InputHandler.Instance.manager.GetInputPlayerData("UseE");
        inputActionUseFace.started += HandleForUseE;

        inputActionUseDrop = InputHandler.Instance.manager.GetInputPlayerData("Drop");
        inputActionUseDrop.started += HandleForDrop;

        inputActionUserDetailsData = InputHandler.Instance.manager.GetInputPlayerData("UserDetails");
        inputActionUserDetailsData.started += HandleForUserDetails;
        inputActionMove = InputHandler.Instance.manager.GetInputPlayerData("Move");

        inputActionShift = InputHandler.Instance.manager.GetInputPlayerData("Shift");
        inputActionCtrl = InputHandler.Instance.manager.GetInputPlayerData("Ctrl");
    }

    public void Update()
    {
        if (GameHandler.Instance.manager.GetGameState() == GameStateEnum.Gaming)
        {
            HandlerForMoveAndJumpUpdate();
            HandleForUseUpdate();

            timeUpdateForUseItemSightTarget += Time.deltaTime;
            if (timeUpdateForUseItemSightTarget >= timeUpdateMaxForUseItemSightTarget)
            {
                timeUpdateForUseItemSightTarget = 0;
                HandlerForUseItemSightTarget();
            }
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
        inputActionUseFace.started -= HandleForUseE;
        inputActionUserDetailsData.started -= HandleForUserDetails;
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
        //攀爬处理
        if (timeClimbEnd > 0)
        {
            isJump = false;
            float climbSpeed = Mathf.Abs(playerVelocity.x) > Mathf.Abs(playerVelocity.z) ? Mathf.Abs(playerVelocity.x) : Mathf.Abs(playerVelocity.z);
            playerVelocity.y = climbSpeed;
            characterController.Move(playerVelocity);
            timeClimbEnd -= Time.deltaTime;
            if (timeClimbEnd > 0)
            {
                //播放攀爬动画
                //播放动画
                if (moveData.x == 0 && moveData.y == 0)
                {
                    character.characterAnim.creatureAnim.SetClimbSpeed(0);
                }
                else
                {
                    character.characterAnim.creatureAnim.SetClimbSpeed(1);
                }
                character.characterAnim.creatureAnim.PlayBaseAnim(CharacterAnimBaseState.Climb);
                character.characterAnim.creatureAnim.PlayJump(isJump);
            }
            else
            {
                //时间到了就还原了
                character.characterAnim.creatureAnim.SetClimbSpeed(0);
                character.characterAnim.creatureAnim.PlayAnim("idle");
            }       
            return;
        }
        //播放动画
        if (moveData.x == 0 && moveData.y == 0)
        {
            character.characterAnim.creatureAnim.PlayBaseAnim(CharacterAnimBaseState.Idle);
        }
        else
        {
            character.characterAnim.creatureAnim.PlayBaseAnim(CharacterAnimBaseState.Walk);
        }
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
    /// 处理攀爬
    /// </summary>
    public void HanldeForClimb()
    {
        timeClimbEnd = 0.25f;
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
    /// 处理-瞄准使用道具的目标
    /// </summary>
    public void HandlerForUseItemSightTarget()
    {
        if (!isActiveAndEnabled)
            return;
        //获取道具栏上的物品
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        ItemsBean itemsData = userData.GetItemsFromShortcut();
        ItemsHandler.Instance.UseItemForSightTarget(itemsData);
    }

    /// <summary>
    /// 处理-持续使用道具
    /// </summary>
    public void HandleForUseUpdate()
    {
        if (isUseItem)
        {
            timeUpdateForUseItem += Time.deltaTime;
            if (timeUpdateForUseItem > timeUpdateMaxForUseItem)
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
        HandleForUse(callback, ItemUseTypeEnum.Left);
    }


    /// <summary>
    /// 取消处理
    /// </summary>
    /// <param name="callback"></param>
    public void HandleForUseR(CallbackContext callback)
    {
        HandleForUse(callback, ItemUseTypeEnum.Right);
    }

    /// <summary>
    /// 互动处理
    /// </summary>
    /// <param name="callback"></param>
    public void HandleForUseE(CallbackContext callback)
    {
        HandleForUse(callback, ItemUseTypeEnum.E, false);
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
        ItemsHandler.Instance.CreateItemCptDrop(itemData.itemId, 1, itemData.meta, player.transform.position + Vector3.up, ItemDropStateEnum.DropNoPick, player.transform.forward + randomFroce);

        //扣除道具
        userData.AddItems(itemData, -1);
        //刷新UI
        UIHandler.Instance.RefreshUI();
    }

    /// <summary>
    /// 使用处理
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="useType"></param>
    /// <param name="isUserItem"></param>
    public void HandleForUse(CallbackContext callback, ItemUseTypeEnum useType, bool isUserItem = true)
    {
        if (!isActiveAndEnabled)
            return;
        if (UGUIUtil.IsPointerUI())
            return;
        this.isUseItem = isUserItem;
        timeUpdateForUseItem = 0;
        //获取道具栏上的物品
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        ItemsBean itemsData = userData.GetItemsFromShortcut();
        Player player = GameHandler.Instance.manager.player;
        ItemsHandler.Instance.UseItem(player.gameObject, itemsData, useType);
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
        //多按键组合
        //如果按住了shift 则速度提升20% 并且持续消耗耐力
        float shiftInput = inputActionShift.ReadValue<float>();
        if (shiftInput != 0 && moveOffset.x!=0 && moveOffset.y != 0)
        {
            //加速消耗耐力
            UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
            CharacterStatusBean characterStatus = userData.characterData.GetCharacterStatus();
            //判断是否成功消耗耐力
            bool isExpendStamina = characterStatus.StaminaExpend(Time.deltaTime);
            moveSpeed = isExpendStamina? moveSpeed * 1.5f : moveSpeed;
        }
        //如果按住了ctrl 则速度减慢50%
        float ctrlInput = inputActionCtrl.ReadValue<float>();
        if (ctrlInput != 0 && moveOffset.x != 0 && moveOffset.y != 0)
            moveSpeed = moveSpeed * 0.5f;
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
        ControlForCamera controlForCamera = GameControlHandler.Instance.manager.controlForCamera;
        //如果是第一人称
        if (controlForCamera.cameraDistance <= 0)
        {
            Vector3 rotateAngles = new Vector3(0, mainCamera.transform.rotation.eulerAngles.y, 0);
            //朝摄像头方向移动
            characterController.transform.eulerAngles = rotateAngles;
        }
        else
        {
            if (moveOffset == Vector2.zero)
                return;
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

}