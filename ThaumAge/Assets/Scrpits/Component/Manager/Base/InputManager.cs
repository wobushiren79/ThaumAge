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
}