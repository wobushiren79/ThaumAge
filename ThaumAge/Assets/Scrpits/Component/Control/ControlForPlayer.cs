using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class ControlForPlayer : ControlForBase
{
    private Rigidbody rbPlayer;
    private CapsuleCollider colliderPlayer;
    private CreatureCptCharacter character;

    [Header("攀爬高度")]
    public float stepHeigh = 0.6f;
    [Header("攀爬过度")]
    public float stepSmooth = 0.1f;
    [Header("重力")]
    public Vector3 gravityValue;
    [Header("移动速度")]
    public float speedMove = 0.6f;

    //跳跃速度
    private float speedJump = 1;
    //角色旋转速度
    private float speedCharacterRotate = 10;
    //移动向量
    private Vector3 playerVelocity;
    //攀爬剩余时间
    private float timeClimbEnd = 0;
    //是否正在跳跃
    private bool isJump = false;
    //是否开启跳跃检测
    private bool isJumpCheck = false;
    //地面类型0地面 1水里
    private int groundType = 0;

    private InputAction inputActionUseL;
    private InputAction inputActionUseR;
    private InputAction inputActionUseFace;

    private InputAction inputActionJump;
    private InputAction inputActionMove;
    private InputAction inputActionUserDetailsData;
    private InputAction inputActionUseDrop;
    private InputAction inputActionShift;
    private InputAction inputActionCtrl;
    private InputAction inputActionShortcutsSelect;

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
        rbPlayer = GetComponent<Rigidbody>();
        colliderPlayer = GetComponent<CapsuleCollider>();

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

        inputActionShortcutsSelect = InputHandler.Instance.manager.GetInputPlayerData("ShortcutsSelect");
        inputActionShortcutsSelect.started += HandleForShortcutsSelect;

        gravityValue = Physics.gravity;
    }


    public void Update()
    {
        if (GameHandler.Instance.manager.GetGameState() == GameStateEnum.Gaming)
        {
            HandleForUseUpdate();
            HandleForJumpUpdate();

            timeUpdateForUseItemSightTarget += Time.deltaTime;
            if (timeUpdateForUseItemSightTarget >= timeUpdateMaxForUseItemSightTarget)
            {
                timeUpdateForUseItemSightTarget = 0;
                HandlerForUseItemSightTarget();
            }
        }
    }

    public void FixedUpdate()
    {
        if (GameHandler.Instance.manager.GetGameState() == GameStateEnum.Gaming)
        {
            HandlerForMoveUpdate();
            HandleForGravityUpdate();
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
    /// 改变地面类型 
    /// </summary>
    /// <param name="groundType">0地面 1水里</param>
    public void ChangeGroundType(int groundType)
    {
        this.groundType = groundType;
        if (groundType == 0)
        {
            gravityValue = Physics.gravity;
        }
        else if (groundType == 1)
        {
            gravityValue = new Vector3(0, -2f, 0);
        }
    }

    /// <summary>
    /// 开关控制
    /// </summary>
    /// <param name="enabled"></param>
    public override void EnabledControl(bool enabled)
    {
        base.EnabledControl(enabled);
    }

    /// <summary>
    /// 打开用户详情
    /// </summary>
    /// <param name="callback"></param>
    public void HandleForUserDetails(CallbackContext callback)
    {
        if (!enabledControl)
            return;
        if (!isActiveAndEnabled)
            return;
        UIGameUserDetails uiGameUserDetails = UIHandler.Instance.OpenUIAndCloseOther<UIGameUserDetails>(UIEnum.GameUserDetails);
        uiGameUserDetails.ui_ViewSynthesis.SetDataType(ItemsSynthesisTypeEnum.Self);
    }

    /// <summary>
    /// 移动处理
    /// </summary>
    public void HandlerForMoveUpdate()
    {
        if (!enabledControl)
            return;
        Vector2 moveData = inputActionMove.ReadValue<Vector2>();
        //旋转角色
        RotateCharacter(moveData, speedCharacterRotate);
        //移动角色
        MoveCharacterCalculate(moveData, speedMove);
        //攀爬处理
        if (timeClimbEnd > 0)
        {
            gravityValue = Vector3.zero;
            float climbSpeed = Mathf.Abs(playerVelocity.x) > Mathf.Abs(playerVelocity.z) ? Mathf.Abs(playerVelocity.x) : Mathf.Abs(playerVelocity.z);
            playerVelocity.y = climbSpeed;
            rbPlayer.MovePosition(rbPlayer.transform.position + playerVelocity);
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
                character.characterAnim.creatureAnim.PlayBaseAnim(CreatureAnimBaseState.Climb);
            }
            else
            {
                gravityValue = Physics.gravity;
                //时间到了就还原了
                character.characterAnim.creatureAnim.SetClimbSpeed(0);
                character.characterAnim.creatureAnim.PlayAnim("idle");
            }
            return;
        }
        //播放动画
        if (moveData.x == 0 && moveData.y == 0)
        {
            character.characterAnim.creatureAnim.PlayBaseAnim(CreatureAnimBaseState.Idle);
            return;
        }
        else
        {
            character.characterAnim.creatureAnim.PlayBaseAnim(CreatureAnimBaseState.Walk);
            rbPlayer.MovePosition(rbPlayer.transform.position + playerVelocity);
            //高度翻越处理
            HandleForStepClimb();
        }
    }

    /// <summary>
    /// 重力处理
    /// </summary>
    public void HandleForGravityUpdate()
    {
        if (!rbPlayer.isKinematic && !rbPlayer.IsSleeping() && gravityValue != Vector3.zero)
        {
            rbPlayer.AddForce(gravityValue, ForceMode.Acceleration);
        }
    }

    /// <summary>
    /// 高度翻越处理
    /// </summary>
    public void HandleForStepClimb()
    {
        Vector3 stepLowerPosition = transform.position.AddY(0.05f);
        Vector3 stepUpperPosition = stepLowerPosition.AddY(stepHeigh);

        //Debug.DrawRay(stepLowerPosition, playerVelocity.normalized * 0.5f, Color.red);
        //Debug.DrawRay(stepUpperPosition, playerVelocity.normalized * 0.6f, Color.red);
        if (RayUtil.CheckToCast(stepLowerPosition, playerVelocity, 0.5f, 1 << LayerInfo.ChunkCollider))
        {
            if (!RayUtil.CheckToCast(stepUpperPosition, playerVelocity, 0.6f, 1 << LayerInfo.ChunkCollider))
            {
                rbPlayer.position += new Vector3(0, stepSmooth, 0);
            }
        }
    }

    /// <summary>
    /// 处理跳跃
    /// </summary>
    public void HandleForJumpUpdate()
    {
        if (isJump && isJumpCheck)
        {
            //发射射线检测
            if (RayUtil.CheckToCast
                (
                    rbPlayer.transform.position + new Vector3(0, 0.1f, 0),
                    Vector3.down,
                    0.2f,
                    1 << LayerInfo.ChunkTrigger | 1 << LayerInfo.ChunkCollider | 1 << LayerInfo.Character | 1 << LayerInfo.Creature)
                )
            {
                isJumpCheck = false;
                isJump = false;
                character.characterAnim.creatureAnim.PlayJump(isJump);
            }
        }
    }

    /// <summary>
    /// 开始跳跃处理
    /// </summary>
    public void HandleForJumpStart(CallbackContext callback)
    {
        if (!enabledControl)
            return;
        //如果是在水里
        if (groundType == 1)
        {
            rbPlayer.AddForce(new Vector3(0, 2f * speedJump, 0), ForceMode.Impulse);
            return;
        }
        if (!isActiveAndEnabled || isJump)
            return;
        isJump = true;
        //播放跳跃动画
        character.characterAnim.creatureAnim.PlayJump(isJump);
        rbPlayer.AddForce(new Vector3(0, 5f * speedJump, 0), ForceMode.Impulse);
        //开启跳跃检测
        this.WaitExecuteSeconds(0.25f, () =>
        {
            isJumpCheck = true;
        });
    }

    /// <summary>
    /// 处理攀爬
    /// </summary>
    public void HanldeForClimb()
    {
        timeClimbEnd = 0.25f;
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
        if (!enabledControl)
            return;
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
        if (!enabledControl)
            return;
        HandleForUse(callback, ItemUseTypeEnum.Left);
    }


    /// <summary>
    /// 取消处理
    /// </summary>
    /// <param name="callback"></param>
    public void HandleForUseR(CallbackContext callback)
    {
        if (!enabledControl)
            return;
        HandleForUse(callback, ItemUseTypeEnum.Right);
    }

    /// <summary>
    /// 互动处理
    /// </summary>
    /// <param name="callback"></param>
    public void HandleForUseE(CallbackContext callback)
    {
        if (!enabledControl)
            return;
        HandleForUse(callback, ItemUseTypeEnum.E, false);
    }

    /// <summary>
    /// 丢弃处理
    /// </summary>
    /// <param name="callback"></param>
    public void HandleForDrop(CallbackContext callback)
    {
        if (!enabledControl)
            return;
        //获取道具栏上的物品
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        ItemsBean itemData = userData.GetItemsFromShortcut();

        //丢出道具
        Player player = GameHandler.Instance.manager.player;
        Vector3 randomFroce = new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(0f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f));
        ItemDropBean itemDropData = new ItemDropBean(itemData.itemId, player.transform.position + Vector3.up, player.transform.forward + randomFroce, 1, itemData.meta, ItemDropStateEnum.DropNoPick);
        ItemsHandler.Instance.CreateItemCptDrop(itemDropData);

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
        if (!enabledControl)
            return;
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
        //刷新手上的物品
        GameHandler.Instance.manager.player.RefreshHandItem();
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
    /// 处理-道具切换
    /// </summary>
    /// <param name="callBack"></param>
    public void HandleForShortcutsSelect(CallbackContext callBack)
    {
        if (!enabledControl)
            return;
        if (!isActiveAndEnabled)
            return;
        float data = callBack.ReadValue<float>();
        int changIndex = 0;
        if (data > 0)
        {
            changIndex = 1;
        }
        else if (data < 0)
        {
            changIndex = -1;
        }
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        userData.SetShortcuts(userData.indexForShortcuts + changIndex);
        //刷新手上物品
        GameHandler.Instance.manager.player.RefreshHandItem();
        //刷新UI
        UIHandler.Instance.GetOpenUI().RefreshUI();
    }

    /// <summary>
    /// 角色移动
    /// </summary>
    /// <param name="movePosition"></param>
    public void MoveCharacterCalculate(Vector2 moveOffset, float moveSpeed)
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
        if (shiftInput != 0 && (moveOffset.x != 0 || moveOffset.y != 0))
        {
            //加速消耗耐力
            UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
            CharacterStatusBean characterStatus = userData.characterData.GetCharacterStatus();
            //判断是否成功消耗耐力
            bool isExpendStamina = characterStatus.StaminaChange(-Time.deltaTime * 2);
            moveSpeed = isExpendStamina ? moveSpeed * 1.5f : moveSpeed;
            //刷新UI
            EventHandler.Instance.TriggerEvent(EventsInfo.CharacterStatus_StatusChange);
        }
        //如果按住了ctrl 则速度减慢50%
        float ctrlInput = inputActionCtrl.ReadValue<float>();
        if (ctrlInput != 0 && (moveOffset.x != 0 || moveOffset.y != 0))
        {
            moveSpeed = moveSpeed * 0.5f;
        }
        //朝摄像头方向移动
        playerVelocity = (Vector3.Normalize(forward) * moveOffset.y + Vector3.Normalize(right) * moveOffset.x) * Time.fixedUnscaledDeltaTime * moveSpeed * 5;
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
            rbPlayer.transform.eulerAngles = rotateAngles;
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
            rbPlayer.transform.rotation = Quaternion.Slerp(transform.rotation, rotate, rotateSpeed * Time.fixedUnscaledDeltaTime);
        }
    }


    /// <summary>
    /// 添加一个理
    /// </summary>
    public void AddForce(Vector3 force, ForceMode forceMode)
    {
        //rbPlayer.AddForce(force, forceMode);
        rbPlayer.velocity = force * 5;
    }
}