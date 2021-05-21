using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.AI;

public class Test : BaseMonoBehaviour
{
    public struct Data1
    {
        public string data;
    }

    public class Data2
    {
        public string data;
    }

    public static Data1 Empty = new Data1();

    private void Start()
    {
        int count1 = 64;
        int count2 = 256;
        int count = 999999;
        string[] arrayData1 = new string[count1 * count2 * count1];
        string[,,] arrayData2 = new string[count1, count2, count1];

        //Stopwatch stopwatch = TimeUtil.GetMethodTimeStart();
        //for (int x = 0; x < count1; x++)
        //{
        //    for (int y = 0; y < count2; y++)
        //    {
        //        for (int z = 0; z < count1; z++)
        //        {
        //            arrayData1[x + y + z] = (x + y + z) + "";
        //        }
        //    }
        //}
        //TimeUtil.GetMethodTimeEnd("1", stopwatch);

        //stopwatch = TimeUtil.GetMethodTimeStart();
        //for (int x = 0; x < count1; x++)
        //{
        //    for (int y = 0; y < count2; y++)
        //    {
        //        for (int z = 0; z < count1; z++)
        //        {
        //            arrayData2[x, y, z] = (x + y + z) + "";
        //        }
        //    }
        //}
        //TimeUtil.GetMethodTimeEnd("2", stopwatch);

        //stopwatch = TimeUtil.GetMethodTimeStart();
        //for (int x = 0; x < count1; x++)
        //{
        //    for (int y = 0; y < count2; y++)
        //    {
        //        for (int z = 0; z < count1; z++)
        //        {
        //            string data = arrayData1[x + y + z];
        //        }
        //    }
        //}
        //TimeUtil.GetMethodTimeEnd("3", stopwatch);

        //stopwatch = TimeUtil.GetMethodTimeStart();
        //for (int x = 0; x < count1; x++)
        //{
        //    for (int y = 0; y < count2; y++)
        //    {
        //        for (int z = 0; z < count1; z++)
        //        {
        //            string data = arrayData2[x, y, z];
        //        }
        //    }
        //}
        //TimeUtil.GetMethodTimeEnd("4", stopwatch);
    }
    public struct TestData1
    {
        public string data;
    }
    public class TestData2
    {
        public string data;
    }
}
