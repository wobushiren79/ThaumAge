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
using UnityEngine.UI;

public class Test : BaseMonoBehaviour
{
    public Canvas canvas;
    private void Start()
    {

    }

    public void OnGUI()
    {
        if (GUILayout.Button("Test"))
        {
            TimeTest();
        }
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
