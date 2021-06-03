using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : BaseManager
{
    public GameInputActions inputActions;
    public virtual void Awake()
    {
        inputActions = new GameInputActions();
        inputActions.Player.Move.Enable();
        inputActions.Player.Jump.Enable();
        inputActions.Player.Look.Enable();
        inputActions.Player.Use.Enable();
        inputActions.Player.Cancel.Enable();
        inputActions.Player.CameraDistance.Enable();
        inputActions.Player.Shortcuts.Enable();
        inputActions.Player.UserDetails.Enable();
    }

    /// <summary>
    /// 获取移动数据
    /// </summary>
    /// <returns></returns>
    public InputAction GetMoveData()
    {
        return inputActions.Player.Move;
    }

    /// <summary>
    /// 获取跳跃数据
    /// </summary>
    /// <returns></returns>
    public InputAction GetJumpData()
    {
        return inputActions.Player.Jump;
    }

    /// <summary>
    /// 获取环顾数据
    /// </summary>
    /// <returns></returns>
    public InputAction GetLookData()
    {
        return inputActions.Player.Look;
    }

    /// <summary>
    /// 获取使用数据
    /// </summary>
    /// <returns></returns>
    public InputAction GetUseData()
    {
        return inputActions.Player.Use;
    }

    /// <summary>
    /// 获取取消数据
    /// </summary>
    /// <returns></returns>
    public InputAction GetCancelData()
    {
        return inputActions.Player.Cancel;
    }


    /// <summary>
    /// 获取摄像头距离数据
    /// </summary>
    /// <returns></returns>
    public InputAction GetCameraDistanceData()
    {
        return inputActions.Player.CameraDistance;
    }

    /// <summary>
    /// 获取快捷栏数据
    /// </summary>
    /// <returns></returns>
    public InputAction GetShortcutsData()
    {
        return inputActions.Player.Shortcuts;
    }

    /// <summary>
    /// 获取用户详情按钮数据
    /// </summary>
    /// <returns></returns>
    public InputAction GetUserDetails()
    {
        return inputActions.Player.UserDetails;
    }
}