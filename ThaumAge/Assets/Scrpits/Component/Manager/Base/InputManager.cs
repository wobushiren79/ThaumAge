using UnityEditor;
using UnityEngine;

public class InputManager : BaseManager
{
    public GameInputActions inputActions;
    public virtual void Awake()
    {
        inputActions = new GameInputActions();
    }
}