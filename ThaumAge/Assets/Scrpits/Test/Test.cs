
using System.Diagnostics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Unity.Burst;
using System.Collections.Generic;

public class Test : BaseMonoBehaviour
{

    public void Start()
    {
        //开关角色控制
        //GameControlHandler.Instance.SetPlayerControlEnabled(true);
        //GameHandler.Instance.manager.SetGameState(GameStateEnum.Gaming);
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        List<Color> listColor = new List<Color>();
        for (int i=0;i<meshFilter.mesh.vertexCount;i++)
        {
            listColor.Add(Color.red);
        }
        Mesh mesh = meshFilter.mesh;
        mesh.SetColors(listColor);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        meshFilter.mesh = mesh;
    }

    private void OnGUI()
    {


    }



}



[BurstCompile]
public struct MyParallelJob : IJobParallelFor
{
    public void Execute(int index)
    {

    }

}
