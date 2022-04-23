
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

        TextAsset textAsset = LoadAddressablesUtil.LoadAssetSync<TextAsset>($"Assets/Prefabs/BlockMeshData/BlockCraftingTableSimple.txt");
        MeshDataCustom meshDataCustom= JsonUtil.FromJson<MeshDataCustom>(textAsset.text);
        Mesh mesh = meshFilter.mesh;
        mesh.vertices = meshDataCustom.mainMeshData.vertices;
        mesh.uv = meshDataCustom.mainMeshData.uv;
        mesh.triangles = meshDataCustom.mainMeshData.triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        mesh.RecalculateBounds();


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
