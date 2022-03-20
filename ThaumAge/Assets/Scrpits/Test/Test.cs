
using System.Diagnostics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Unity.Burst;
public class Test : BaseMonoBehaviour
{

    public int data1;
    public int data2;
    private void OnGUI()
    {
        if (GUILayout.Button("Test3"))
        {
            LogUtil.Log("Test:"+data1/data2);
        }
        if (GUILayout.Button("Test1"))
        {
            Test1();
        }
        if (GUILayout.Button("Test2"))
        {
            int count = 160 * 160 * 160;
            Block[] data1 = new Block[count];
            BlockManager blockManager =  BlockHandler.Instance.manager;
            Stopwatch stopwatch = TimeUtil.GetMethodTimeStart();

            for (int i = 0; i < count; i++)
            {
                Block itemBlock = blockManager.GetRegisterBlock(1);
            }
            TimeUtil.GetMethodTimeEnd("1", stopwatch);

            stopwatch.Restart();
            int[] data3 = new int[count];
            for (int i = 0; i < count; i++)
            {
                data3[i] =1;
            }

            TimeUtil.GetMethodTimeEnd("2", stopwatch);

            stopwatch.Restart();

            for (int i = 0; i < count; i++)
            {
                Block itemBlock = blockManager.GetRegisterBlock(data3[i]);
            }

            TimeUtil.GetMethodTimeEnd("3", stopwatch);

            stopwatch.Restart();

            for (int i = 0; i < count; i++)
            {
                Block itemBlock = data1[i];
            }

            TimeUtil.GetMethodTimeEnd("4", stopwatch);

        }
    }

    public void Test1()
    {
        int DataCount = 16 * 16 * 16;
        string[] data = new string[DataCount];
        for (int i = 0; i < DataCount; i++)
        {
            data[i] = i + "";
        }

        Stopwatch stopwatch = TimeUtil.GetMethodTimeStart();
        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 16; y++)
            {
                for (int z = 0; z < 16; z++)
                {
                    string item = data[y << 8 | z << 4 | x];
                }
            }
        }
        TimeUtil.GetMethodTimeEnd("1:", stopwatch);
        stopwatch.Restart();
        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 16; y++)
            {
                for (int z = 0; z < 16; z++)
                {
                    data[y << 8 | z << 4 | x] = "x";
                }
            }
        }
        TimeUtil.GetMethodTimeEnd("2:", stopwatch);

        stopwatch.Restart();
        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 16; y++)
            {
                for (int z = 0; z < 16; z++)
                {
                    string item = data[x + y * 16 + z * 16 * 16];
                }
            }
        }
        TimeUtil.GetMethodTimeEnd("3:", stopwatch);


        stopwatch.Restart();
        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 16; y++)
            {
                for (int z = 0; z < 16; z++)
                {
                    data[x + y * 16 + z * 16 * 16] = "x";
                }
            }
        }
        TimeUtil.GetMethodTimeEnd("4:", stopwatch);
    }
}



[BurstCompile]
public struct MyParallelJob : IJobParallelFor
{
    public void Execute(int index)
    {

    }

}
