
using System.Diagnostics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Unity.Burst;
public class Test : BaseMonoBehaviour
{
    private void Awake()
    {

    }

    private void OnGUI()
    {
        if (GUILayout.Button("11111111111"))
        {
            int[,] data = new int[16,16];
            ref int outData = ref data[18, 18];
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
