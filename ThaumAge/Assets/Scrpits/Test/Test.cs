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
    }

    public void OnGUI()
    {
        if (GUILayout.Button("Test"))
        {
            int count = 8124 * 8;
            int useCount = 2;
            int findCount = 8124 * 8;

            string[] listData = new string[count];
            Dictionary<int, string> dicData = new Dictionary<int, string>();

            string[] arrayData = new string[count];

            for (int i = 0; i < count; i++)
            {
                arrayData[i] = ("test" + i);
            }

            for (int i = 0; i < useCount; i++)
            {
                dicData.Add(i, "test" + i);
            }



            Stopwatch stopwatch = TimeUtil.GetMethodTimeStart();
            for (int x = 0; x < findCount; x++)
            {
                string data = listData[x];
            }
            TimeUtil.GetMethodTimeEnd("string[]", stopwatch);

            Stopwatch stopwatch2 = TimeUtil.GetMethodTimeStart();
            for (int x = 0; x < findCount; x++)
            {
                string data = dicData[1];
            }
            TimeUtil.GetMethodTimeEnd("Dictionary", stopwatch2);
        }
    }

}
