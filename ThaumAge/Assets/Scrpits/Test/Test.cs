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
        int count = 999999;

        DataTest[] dataList1 = new DataTest[count];
        int[] dataList2 = new int[count];
        byte[] dataList3 = new byte[count];
        string[] dataList5 = new string[count];
        string[] dataList6 = new string[count];
        BlockBean[] dataList4 = new BlockBean[count];

        Stopwatch stopwatch = TimeUtil.GetMethodTimeStart();
        for (int i = 0; i < count; i++)
        {
            DataTest dataTest = new DataTest();
            dataTest.id = i;
            dataTest.dr = 1;
            dataTest.remark = "test";
            dataTest.remark2 = "test";
            dataList1[i] = dataTest;
        }
        TimeUtil.GetMethodTimeEnd("1", stopwatch);

        stopwatch = TimeUtil.GetMethodTimeStart();
        for (int i = 0; i < count; i++)
        {
            dataList2[i] = i;
            dataList3[i] = 1;
            dataList5[i] = "test";
            dataList6[i] = "test";
        }
        TimeUtil.GetMethodTimeEnd("2", stopwatch);

        stopwatch = TimeUtil.GetMethodTimeStart();
        for (int i = 0; i < count; i++)
        {
            BlockBean dataTest = new BlockBean();
            dataList4[i] = dataTest;
        }
        TimeUtil.GetMethodTimeEnd("3", stopwatch);

        stopwatch = TimeUtil.GetMethodTimeStart();
        for (int i = 0; i < count; i++)
        {
            DataTest dataTes = dataList1[i];
            int id = dataTes.id;
            int dr = dataTes.dr;
            string remark = dataTes.remark;
        }
        TimeUtil.GetMethodTimeEnd("11", stopwatch);

        stopwatch = TimeUtil.GetMethodTimeStart();
        for (int i = 0; i < count; i++)
        {
            int id = dataList2[i];
            byte dr = dataList3[i];
            string remark = dataList5[i];
            string remark2 = dataList6[i];
        }
        TimeUtil.GetMethodTimeEnd("22", stopwatch);

        stopwatch = TimeUtil.GetMethodTimeStart();
        for (int i = 0; i < count; i++)
        {
            BlockBean dataTes = dataList4[i];
        }
        TimeUtil.GetMethodTimeEnd("33", stopwatch);
    }


}
