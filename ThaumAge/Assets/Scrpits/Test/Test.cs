﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Test : BaseMonoBehaviour
{
    private void Start()
    {
        FastNoise fastNoise = new FastNoise("Test");
    }

    public void OnGUI()
    {
        if (GUILayout.Button("Test"))
        {
            TimeTest7();
        }
    }

    public void TimeTest7()
    {
        int number = 16 * 16 * 2560;
        Stopwatch stopwatch = TimeUtil.GetMethodTimeStart();

        for (int i = 0; i < number; i++)
        {
            Test1(out BlockBean data1);
        }
        TimeUtil.GetMethodTimeEnd("data1:", stopwatch);
        stopwatch.Restart();

        stopwatch.Start();
        for (int i = 0; i < number; i++)
        {
            BlockBean block = Test2();
        }
        TimeUtil.GetMethodTimeEnd("data2:", stopwatch);
    }


    public void Test1(out BlockBean data1)
    {
        data1 = new BlockBean();
    }
    public BlockBean Test2()
    {
        return new BlockBean();
    }
    public void TimeTest6()
    {
        int number = 16 * 16 * 256;
        List<BlockBean> listData1 = new List<BlockBean>();
        List<BlockBean> listData2 = new List<BlockBean>();
        Stopwatch stopwatch = TimeUtil.GetMethodTimeStart();

        for (int i = 0; i < number; i++)
        {
            BlockBean blockBean1 = new BlockBean();
            int id = 123;
            int di = 1;
            string remakr = "remak";
            blockBean1.meta = $"{id}|{di}|{remakr}";
            listData1.Add(blockBean1);
        }
        TimeUtil.GetMethodTimeEnd("data1:", stopwatch);
        stopwatch.Restart();

        stopwatch.Start();
        for (int i = 0; i < number; i++)
        {
            BlockBean blockBean1 = new BlockBean();
            blockBean1.meta = JsonUtil.ToJson(new TestBean() { id = 123, di = 1, remark = "remak" });
            listData2.Add(blockBean1);
        }
        TimeUtil.GetMethodTimeEnd("data2:", stopwatch);
        stopwatch.Restart();

        stopwatch.Start();
        for (int i = 0; i < listData1.Count; i++)
        {
            BlockBean itemData= listData1[i];
            string[] dataList = StringUtil.SplitBySubstringForArrayStr(itemData.meta, '|');
            int id = int.Parse(dataList[0]);
            int di = int.Parse(dataList[1]);
            string remak = dataList[2];
        }
        TimeUtil.GetMethodTimeEnd("data1find:", stopwatch);
        stopwatch.Restart();

        stopwatch.Start();
        for (int i = 0; i < listData2.Count; i++)
        {
            BlockBean itemData = listData2[2];
            TestBean test = JsonUtil.FromJson<TestBean>(itemData.meta);
            int id = test.id;
            int di = test.di;
            string remak = test.remark;
        }
        TimeUtil.GetMethodTimeEnd("data2find:", stopwatch);
    }

    public class TestBean
    {
        public int id;
        public int di;
        public string remark;
    }

    public void TimeTest5()
    {
        int number = 10000000;
        int x1 = 0;
        int x2 = -1;
        int x3 = 1;
        Stopwatch stopwatch = TimeUtil.GetMethodTimeStart();
        for (int i = 0; i < number; i++)
        {
            if (x1 == 0 || x2 == -1 || x3 == 1)
            {

            }
        }
        TimeUtil.GetMethodTimeEnd("data1:", stopwatch);
        stopwatch.Restart();

        stopwatch.Start();
        for (int i = 0; i < number; i++)
        {
            switch (x1)
            {
                case -1:
                    break;
            }

            if (x1 == 1)
            {

            }
            else if (x2 == -2)
            {

            }
            else if (x3 == 1)
            {

            }
        }
        TimeUtil.GetMethodTimeEnd("data2:", stopwatch);
    }

    public void TimeTest4()
    {
        int number = 100000;
        Vector3Int startPosition = Vector3Int.one;
        Stopwatch stopwatch = TimeUtil.GetMethodTimeStart();
        for (int i = 0; i < number; i++)
        {
            Vector3Int target = startPosition + Vector3Int.one;
        }
        TimeUtil.GetMethodTimeEnd("data1:", stopwatch);
        stopwatch.Restart();

        stopwatch.Start();
        for (int i = 0; i < number; i++)
        {
            Vector3Int target = new Vector3Int(startPosition.x + 1, startPosition.y + 1, startPosition.z + 1);
        }
        TimeUtil.GetMethodTimeEnd("data2:", stopwatch); stopwatch.Restart();

        stopwatch.Start();
        for (int i = 0; i < number; i++)
        {
            Vector3Int target = startPosition.AddXYZ(1, 1, 1);
        }
        TimeUtil.GetMethodTimeEnd("data3:", stopwatch);
    }

    public void TimeTest3()
    {
        int number = 100000;
        int[] arrayData = new int[number];
        List<int> listData = new List<int>();
        Stopwatch stopwatch = TimeUtil.GetMethodTimeStart();
        for (int i = 0; i < number; i++)
        {
            arrayData[i] = i;
        }
        TimeUtil.GetMethodTimeEnd("data1:", stopwatch);
        stopwatch.Restart();

        stopwatch.Start();
        for (int i = 0; i < number; i++)
        {
            listData.Add(i);
        }
        TimeUtil.GetMethodTimeEnd("data2:", stopwatch);
    }

    public void TimeTest2()
    {
        int number = 100000;
        List<int> listData1 = new List<int>();
        List<int> listData2 = new List<int>();
        Stopwatch stopwatch = TimeUtil.GetMethodTimeStart();
        for (int i = 0; i < number; i++)
        {
            listData1.Add(1);
            listData1.Add(2);
            listData1.Add(3);
            listData1.Add(4);
            listData1.Add(5);
        }
        TimeUtil.GetMethodTimeEnd("data1:", stopwatch);
        int[] itemList1 = new int[5]
{
                 1,2,3,4,5
};
        stopwatch.Restart();

        stopwatch.Start();

        for (int i = 0; i < number; i++)
        {

            listData2.AddRange(itemList1);
        }
        TimeUtil.GetMethodTimeEnd("data2:", stopwatch);
    }

    public void TimeTest()
    {
        int xNumber = 100;
        int yNumber = 100;
        int zNumber = 100;
        Stopwatch stopwatch = TimeUtil.GetMethodTimeStart();
        string[,,] data1 = new string[xNumber, yNumber, zNumber];
        for (int x = 0; x < xNumber; x++)
        {
            for (int y = 0; y < yNumber; y++)
            {
                for (int z = 0; z < zNumber; z++)
                {
                    data1[x, y, z] = $"i";
                }
            }
        }
        TimeUtil.GetMethodTimeEnd("data1 init:", stopwatch);
        stopwatch.Restart();

        stopwatch.Start();
        int number = xNumber * yNumber * zNumber;
        string[] data2 = new string[number];
        for (int i = 0; i < number; i++)
        {
            data2[i] = $"i";
        }
        TimeUtil.GetMethodTimeEnd("data2 init:", stopwatch);
        stopwatch.Restart();

        stopwatch.Start();
        for (int x = 0; x < xNumber; x++)
        {
            for (int y = 0; y < yNumber; y++)
            {
                for (int z = 0; z < zNumber; z++)
                {
                    string itemData = data1[x, y, z];
                }
            }
        }
        TimeUtil.GetMethodTimeEnd("data1 for:", stopwatch);
        stopwatch.Restart();

        stopwatch.Start();
        for (int i = 0; i < number; i++)
        {
            string itemData = data2[i];
        }
        TimeUtil.GetMethodTimeEnd("data2 for:", stopwatch);
        stopwatch.Restart();

        stopwatch.Start();
        for (int i = 0; i < 100000; i++)
        {
            string itemData = data1[50, 50, 50];
        }
        TimeUtil.GetMethodTimeEnd("data2 get10000:", stopwatch);
        stopwatch.Restart();

        int Half = number / 2;
        stopwatch.Start();
        for (int i = 0; i < 100000; i++)
        {
            string itemData = data2[(xNumber / 2) * yNumber * zNumber + (yNumber / 2) * zNumber + zNumber / 2];
        }
        TimeUtil.GetMethodTimeEnd("data2 get10000:", stopwatch);
        stopwatch.Restart();
    }

}
