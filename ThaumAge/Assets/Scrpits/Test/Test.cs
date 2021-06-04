using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Test : BaseMonoBehaviour
{

    private void Start()
    {
        //UIHandler.Instance.manager.OpenUI<UIGameMain>(UIEnum.GameMain);
        InputAction shortcutsData = InputHandler.Instance.manager.GetShortcutsData();
        shortcutsData.started += HandleForShortcuts;

        CameraHandler.Instance.InitData();
    }

    public void HandleForShortcuts(InputAction.CallbackContext callback)
    {
        float data = callback.ReadValue<float>();
        LogUtil.Log("data:" + data);
    }

}
