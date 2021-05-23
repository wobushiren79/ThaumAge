using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class Test : BaseMonoBehaviour
{
    private void Start()
    {


        int xCount = 64;
        Dictionary<int, string> dicData1 = new Dictionary<int, string>();
        Dictionary<Vector3, string> dicData2 = new Dictionary<Vector3, string>();
        Dictionary<string, string> dicData3 = new Dictionary<string, string>();
        Stopwatch stopwatch = TimeUtil.GetMethodTimeStart();
        for (int x = 0; x < xCount; x++)
        {
            for (int y = 0; y < xCount; y++)
            {
                for (int z = 0; z < xCount; z++)
                {
                    int index = MathUtil.GetSingleIndexForThree(x, y, z, xCount, xCount);
                    dicData1.Add(index, x + " " + y + " " + z);
                }
            }
        }
        TimeUtil.GetMethodTimeEnd("1", stopwatch);

        stopwatch = TimeUtil.GetMethodTimeStart();
        for (int x = 0; x < xCount; x++)
        {
            for (int y = 0; y < xCount; y++)
            {
                for (int z = 0; z < xCount; z++)
                {
                    dicData2.Add(new Vector3(x, y, z), x + " " + y + " " + z);
                }
            }
        }
        TimeUtil.GetMethodTimeEnd("2", stopwatch);

        stopwatch = TimeUtil.GetMethodTimeStart();
        for (int x = 0; x < xCount; x++)
        {
            for (int y = 0; y < xCount; y++)
            {
                for (int z = 0; z < xCount; z++)
                {
                    dicData3.Add(x + "_" + y + "_" + z, x + " " + y + " " + z);
                }
            }
        }
        TimeUtil.GetMethodTimeEnd("2.5", stopwatch);

        stopwatch = TimeUtil.GetMethodTimeStart();
        for (int x = 0; x < xCount; x++)
        {
            for (int y = 0; y < xCount; y++)
            {
                for (int z = 0; z < xCount; z++)
                {
                    int index = MathUtil.GetSingleIndexForThree(x, y, z, xCount, xCount);
                    dicData1.TryGetValue(index, out string value);
                }
            }
        }
        TimeUtil.GetMethodTimeEnd("3", stopwatch);

        stopwatch = TimeUtil.GetMethodTimeStart();
        for (int x = 0; x < xCount; x++)
        {
            for (int y = 0; y < xCount; y++)
            {
                for (int z = 0; z < xCount; z++)
                {
                    dicData2.TryGetValue(new Vector3(x, y, z), out string value);
                }
            }
        }
        TimeUtil.GetMethodTimeEnd("4", stopwatch);

        stopwatch = TimeUtil.GetMethodTimeStart();
        for (int x = 0; x < xCount; x++)
        {
            for (int y = 0; y < xCount; y++)
            {
                for (int z = 0; z < xCount; z++)
                {
                    dicData3.TryGetValue(x + "_" + y + "_" + z, out string value);
                }
            }
        }
        TimeUtil.GetMethodTimeEnd("4.5", stopwatch);
        //string[] arrayData1 = new string[xCount* xCount* xCount];
        //string[,,] arrayData2 = new string[xCount , xCount ,xCount];
        //Stopwatch stopwatch = TimeUtil.GetMethodTimeStart();
        //for (int x = 0; x < xCount; x++)
        //{
        //    for (int y = 0; y < xCount; y++)
        //    {
        //        for (int z = 0; z < xCount; z++)
        //        {
        //            arrayData1[x* xCount* xCount + y* xCount + z] = (x + y + z) + "";
        //        }
        //    }
        //}
        //TimeUtil.GetMethodTimeEnd("1", stopwatch);

        //stopwatch = TimeUtil.GetMethodTimeStart();
        //for (int x = 0; x < xCount; x++)
        //{
        //    for (int y = 0; y < xCount; y++)
        //    {
        //        for (int z = 0; z < xCount; z++)
        //        {
        //            arrayData2[x, y, z] = (x + y + z) + "";
        //        }
        //    }
        //}
        //TimeUtil.GetMethodTimeEnd("2", stopwatch);

        //stopwatch = TimeUtil.GetMethodTimeStart();
        //for (int x = 0; x < xCount; x++)
        //{
        //    for (int y = 0; y < xCount; y++)
        //    {
        //        for (int z = 0; z < xCount; z++)
        //        {
        //            string data = arrayData1[x * xCount * xCount + y * xCount + z];
        //        }
        //    }
        //}
        //TimeUtil.GetMethodTimeEnd("3", stopwatch);

        //stopwatch = TimeUtil.GetMethodTimeStart();
        //for (int x = 0; x < xCount; x++)
        //{
        //    for (int y = 0; y < xCount; y++)
        //    {
        //        for (int z = 0; z < xCount; z++)
        //        {
        //            string data = arrayData2[x, y, z];
        //        }
        //    }
        //}
        //TimeUtil.GetMethodTimeEnd("4", stopwatch);
    }
}
