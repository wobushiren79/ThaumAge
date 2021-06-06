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

        int count = 999999;
        Dictionary<int, string> dicData = new Dictionary<int, string>();
        string[] arrayData = new string[count];
        Stopwatch stopwatch = TimeUtil.GetMethodTimeStart();
        for (int i = 0; i < count; i++)
        {
            dicData.Add(i, "count:" + count);
        }
        TimeUtil.GetMethodTimeEnd("1", stopwatch);
        stopwatch = TimeUtil.GetMethodTimeStart();
        for (int i = 0; i < count; i++)
        {
            arrayData[i] = "count:" + count;
        }
        TimeUtil.GetMethodTimeEnd("2", stopwatch);
        stopwatch = TimeUtil.GetMethodTimeStart();
        for (int i = 0; i < count; i++)
        {
            dicData.TryGetValue(i,out string data);
        }
        TimeUtil.GetMethodTimeEnd("3", stopwatch);
        stopwatch = TimeUtil.GetMethodTimeStart();
        for (int i = 0; i < count; i++)
        {
            string data = arrayData[i];
        }
        TimeUtil.GetMethodTimeEnd("4", stopwatch);
    }


}
