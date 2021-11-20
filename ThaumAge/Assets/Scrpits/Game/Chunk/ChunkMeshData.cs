using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChunkMeshData
{
    //普通使用的三角形合集
    public List<Vector3> verts;
    public List<Vector2> uvs;

    //碰撞使用的三角形合集
    public List<Vector3> vertsCollider;
    public List<int> trisCollider;

    //触发使用的三角形合集
    public List<Vector3> vertsTrigger = new List<Vector3>();
    public List<int> trisTrigger = new List<int>();

    //所有三角形合集，根据材质球区分
    public List<int>[] dicTris;

    public Dictionary<Vector3, ChunkMeshIndexData> dicIndexData;

    public ChunkMeshData()
    {
        verts = new List<Vector3>();
        uvs = new List<Vector2>();

        vertsCollider = new List<Vector3>();
        trisCollider = new List<int>();

        vertsTrigger = new List<Vector3>();
        trisTrigger = new List<int>();

        dicTris = new List<int>[EnumUtil.GetEnumMaxIndex<BlockMaterialEnum>() + 1];
        for (int i = 0; i < dicTris.Length; i++)
        {
            dicTris[i] = new List<int>();
        }

        dicIndexData = new Dictionary<Vector3, ChunkMeshIndexData>();
    }

    public void AddMeshIndexData(Vector3 position,int vertsStartIndex, int vertsEndIndex,int trisStartIndex,int trisEndIndex)
    {
        ChunkMeshIndexData chunkMeshIndex = new ChunkMeshIndexData();
        chunkMeshIndex.vertsStartIndex = vertsStartIndex;
        chunkMeshIndex.vertsEndIndex = vertsEndIndex;
        chunkMeshIndex.trisStartIndex = trisStartIndex;
        chunkMeshIndex.trisEndIndex = trisEndIndex;
        dicIndexData.Add(position, chunkMeshIndex);
    }
}

public struct ChunkMeshIndexData 
{
    public int vertsStartIndex;
    public int vertsEndIndex;
    public int trisStartIndex;
    public int trisEndIndex;
}


