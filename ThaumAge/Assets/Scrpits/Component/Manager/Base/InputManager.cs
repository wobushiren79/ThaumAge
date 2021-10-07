using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : BaseManager
{
    public GameInputActions inputActions;

    public Dictionary<InputActionUIEnum, InputAction> dicInputUI = new Dictionary<InputActionUIEnum, InputAction>();
    public Dictionary<string, InputAction> dicInputPlayer = new Dictionary<string, InputAction>();

    public virtual void Awake()
    {
        inputActions = new GameInputActions();

        InputAction F12 = inputActions.UI.F12;
        InputAction ESC = inputActions.UI.ESC;

        F12.Enable();
        ESC.Enable();

        dicInputUI.Add(F12.name.GetEnum<InputActionUIEnum>(), F12);
        dicInputUI.Add(ESC.name.GetEnum<InputActionUIEnum>(), ESC);
        //----------------------------------------------------------
        InputAction Move = inputActions.Player.Move;
        InputAction Jump = inputActions.Player.Jump;
        InputAction Look = inputActions.Player.Look;
        InputAction Use = inputActions.Player.Use;
        InputAction Cancel = inputActions.Player.Cancel;
        InputAction CameraDistance = inputActions.Player.CameraDistance;
        InputAction Shortcuts = inputActions.Player.Shortcuts;
        InputAction UserDetails = inputActions.Player.UserDetails;

        inputActions.Player.Move.Enable();
        inputActions.Player.Jump.Enable();
        inputActions.Player.Look.Enable();
        inputActions.Player.Use.Enable();
        inputActions.Player.Cancel.Enable();
        inputActions.Player.CameraDistance.Enable();
        inputActions.Player.Shortcuts.Enable();
        inputActions.Player.UserDetails.Enable();

        dicInputPlayer.Add(Move.name, Move);
        dicInputPlayer.Add(Jump.name, Jump);
        dicInputPlayer.Add(Look.name, Look);
        dicInputPlayer.Add(Use.name, Use);
        dicInputPlayer.Add(Cancel.name, Cancel);
        dicInputPlayer.Add(CameraDistance.name, CameraDistance);
        dicInputPlayer.Add(Shortcuts.name, Shortcuts);
        dicInputPlayer.Add(UserDetails.name, UserDetails);
    }

    /// <summary>
    /// 获取UI数据
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public InputAction GetInputUIData(InputActionUIEnum name)
    {
        if (dicInputUI.TryGetValue(name, out InputAction value))
        {
            return value;
        }
        return null;
    }

    /// <summary>
    /// 获取Player数据
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public InputAction GetInputPlayerData(string name)
    {
        if (dicInputPlayer.TryGetValue(name, out InputAction value))
        {
            return value;
        }
        return null;
    }
}