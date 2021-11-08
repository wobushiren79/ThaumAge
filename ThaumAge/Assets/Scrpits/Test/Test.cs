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
    public MeshFilter meshFilter;

    private void Start()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[100];
        vertices[0] = new Vector3(0, 0, 0);
        vertices[1] = new Vector3(0, 1, 0);
        vertices[2] = new Vector3(1, 1, 0);
        vertices[3] = new Vector3(1, 0,0 );
        mesh.SetVertices(vertices);

        int[] tra = new int[30];
        tra[0] = 0;
        tra[1] = 1;
        tra[2] = 2;
        tra[3] = 0;
        tra[4] = 2;
        tra[5] = 3;
        mesh.SetTriangles(tra,0);
        meshFilter.mesh = mesh;
    }

    public void OnGUI()
    {
        if (GUILayout.Button("Test"))
        {

        }
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
