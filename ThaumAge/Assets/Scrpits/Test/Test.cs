
using System.Diagnostics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Unity.Burst;
public class Test : BaseMonoBehaviour
{
    public ComputeShader computeShader;
    public int mainID;
    protected int count = 32;

    public struct TestBuffer
    {
        public Vector3Int pos;
        public float height;
    }

    GameObject[,] objs;
    private void Awake()
    {
        objs = new GameObject[count, count];
        for (int x = 0; x < count; x++)
        {
            for (int z = 0; z < count; z++)
            {
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                obj.transform.position = new Vector3(x, 0, z);
                objs[x, z] = obj;
            }
        }
    }

    private void OnGUI()
    {
        if (GUILayout.Button("TTTTTTTTTTT1"))
        {
            mainID = computeShader.FindKernel("CSMain");

            Stopwatch stopwatch = TimeUtil.GetMethodTimeStart();
            TestBuffer[] data = new TestBuffer[count * count];
            for (int x = 0; x < count; x++)
            {
                for (int z = 0; z < count; z++)
                {
                    data[x * count + z].pos = new Vector3Int(x,z,0);
                }
            }

            ComputeBuffer computeBuffer = new ComputeBuffer(data.Length, 16);
            computeBuffer.SetData(data);

            computeShader.SetBuffer(mainID, "dataBuffer", computeBuffer);
            computeShader.Dispatch(mainID, count/16, count / 16, 1);

            computeBuffer.GetData(data);
            computeBuffer.Dispose();
            TimeUtil.GetMethodTimeEnd("1：", stopwatch);

            for (int x = 0; x < count; x++)
            {
                for (int z = 0; z < count; z++)
                {
                    GameObject obj = objs[x, z];
                    obj.transform.position = new Vector3(x, data[x * count + z].height * 2, z);
                }
            }

        }
        if (GUILayout.Button("TTTTTTTTTTT2"))
        {
            mainID = computeShader.FindKernel("CSMain");

            Stopwatch stopwatch = TimeUtil.GetMethodTimeStart();
            TestBuffer[] data2 = new TestBuffer[count];
            for (int i = 0; i < count; i++)
            {

            }
            TimeUtil.GetMethodTimeEnd("2：", stopwatch);
        }
    }



}



[BurstCompile]
public struct MyParallelJob : IJobParallelFor
{
    public void Execute(int index)
    {

    }

}
