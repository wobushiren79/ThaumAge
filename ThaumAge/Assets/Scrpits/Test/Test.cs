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

    public struct DataTest
    {
        public int id;
        public byte dr;
        public string remark;
        public string remark2;
    }

    private void Start()
    {
        //UIHandler.Instance.manager.OpenUI<UIGameMain>(UIEnum.GameMain);
        int count = 8124*16;
        int seed = 131;

        List<string> listData = new List<string>();
        Dictionary<int, string> dicData = new Dictionary<int, string>();
        string[] arrayData = new string[count];

        for (int i = 0; i < count; i++)
        {
            dicData.Add(i, "test" + i);
            arrayData[i] = ("test" + i);
        }

        for (int i = 0; i < 1; i++)
        {
            listData.Add("test" + i);
        }



        Stopwatch stopwatch = TimeUtil.GetMethodTimeStart();
        for (int x = 0; x < count; x++)
        {
            string data = arrayData[x];
        }
        TimeUtil.GetMethodTimeEnd("2", stopwatch);

        stopwatch = TimeUtil.GetMethodTimeStart();
        for (int x = 0; x < 1; x++)
        {
            string data = listData[x];
        }
        TimeUtil.GetMethodTimeEnd("1", stopwatch);
    }


}
