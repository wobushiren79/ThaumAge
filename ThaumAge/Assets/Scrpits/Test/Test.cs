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
        GameHandler.Instance.manager.SetGameState(GameStateEnum.Gaming);
        //初始化摄像头数据
        CameraHandler.Instance.InitGameData();
        //开关角色控制
        GameControlHandler.Instance.manager.controlForPlayer.EnabledControl(true);
    }

    public void OnGUI()
    {
        if (GUILayout.Button("Test"))
        {
            GameControlHandler.Instance.SetPlayerControlEnabled(true);
        }
    }

}
