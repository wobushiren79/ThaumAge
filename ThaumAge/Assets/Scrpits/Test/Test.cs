
using System.Diagnostics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class Test : BaseMonoBehaviour
{
    public int DataCount;


    private void OnGUI()
    {

        if (GUILayout.Button("Test"))
        {
            DataCount = 16 * 16 * 16;
            string[] data = new string[DataCount];
            for (int i = 0; i < DataCount; i++)
            {
                data[i] = i + "";
            }

            Stopwatch stopwatch= TimeUtil.GetMethodTimeStart();
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
            TimeUtil.GetMethodTimeEnd("1:",stopwatch);
            stopwatch.Start();
            for (int x = 0; x < 16; x++)
            {
                for (int y = 0; y < 16; y++)
                {
                    for (int z = 0; z < 16; z++)
                    {
                         data[x + y * 16 + z * 16 * 16]="x";
                    }
                }
            }
            TimeUtil.GetMethodTimeEnd("2:", stopwatch);

            stopwatch.Start();
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


            stopwatch.Start();
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
}

[BurstCompatible]
public struct MyParallelJob : IJobParallelFor
{
    [ReadOnly] public Chunk chunk;
    [ReadOnly] public NativeArray<float3> data;
    [WriteOnly] public NativeArray<float> result;

    public void Execute(int i)
    {
        Vector3 item = data[i];
        result[i] = Mathf.Sqrt(item.x * item.x + item.y * item.y + item.z * item.z);
    }

}
