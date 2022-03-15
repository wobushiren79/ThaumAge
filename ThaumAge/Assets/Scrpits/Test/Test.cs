
using System.Diagnostics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class Test : BaseMonoBehaviour
{
    public int DataCount;

    private NativeArray<float3> m_JobDatas;

    private NativeArray<float> m_JobResults;

    private Vector3[] m_NormalDatas;

    private float[] m_NormalResults;

    private void OnGUI()
    {

        if (GUILayout.Button("Job"))
        {
            m_JobDatas = new NativeArray<float3>(DataCount, Allocator.TempJob);
            m_JobResults = new NativeArray<float>(DataCount, Allocator.TempJob);

            m_NormalDatas = new Vector3[DataCount];
            m_NormalResults = new float[DataCount];

            for (int i = 0; i < DataCount; i++)
            {
                m_JobDatas[i] = new float3(1, 1, 1);
                m_NormalDatas[i] = new Vector3(1, 1, 1);
            }

            MyParallelJob jobData = new MyParallelJob();
            jobData.data = m_JobDatas;
            jobData.result = m_JobResults;
            jobData.chunk = null;
            JobHandle handle = jobData.Schedule(DataCount, 64);

            Stopwatch stopwatch= TimeUtil.GetMethodTimeStart();
            handle.Complete();
            TimeUtil.GetMethodTimeEnd("Test", stopwatch);

            stopwatch.Restart();
            //正常数据运算
            for (var i = 0; i < DataCount; i++)
            {
                var item = m_NormalDatas[i];
                m_NormalResults[i] = Mathf.Sqrt(item.x * item.x + item.y * item.y + item.z * item.z);
            }
            TimeUtil.GetMethodTimeEnd("Test", stopwatch);

            m_JobDatas.Dispose();
            m_JobResults.Dispose();
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
