using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public partial class UIGameMain : BaseUIComponent
{
    protected InputAction inputGodMain;

    public override void Awake()
    {
        base.Awake();
        inputGodMain = InputHandler.Instance.manager.GetUIGodMain();
        inputGodMain.started += HandleForOpenGodMain;
    }

    private void OnDestroy()
    {
        inputGodMain.started -= HandleForOpenGodMain;
    }

    /// <summary>
    /// ´ò¿ªGM²Ëµ¥
    /// </summary>
    /// <param name="callback"></param>
    public void HandleForOpenGodMain(CallbackContext callback)
    {
        if (gameObject.activeSelf && ProjectConfigInfo.BUILD_TYPE == ProjectBuildTypeEnum.Debug)
        {
            UIHandler.Instance.OpenUIAndCloseOther<UIGodMain>(UIEnum.GodMain);
        }
    }
}
