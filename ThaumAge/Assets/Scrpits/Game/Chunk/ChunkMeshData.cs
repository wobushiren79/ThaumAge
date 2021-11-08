using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChunkMeshData
{
    //普通使用的三角形合集
    public int indexVert;
    public Vector3[] verts;
    public int indexUV;
    public Vector2[] uvs;

    //碰撞使用的三角形合集
    public List<Vector3> vertsCollider = new List<Vector3>();
    public List<int> trisCollider = new List<int>();

    //触发使用的三角形合集
    public List<Vector3> vertsTrigger = new List<Vector3>();
    public List<int> trisTrigger = new List<int>();

    //所有三角形合集，根据材质球区分
    public Dictionary<BlockMaterialEnum, List<int>> dicTris = new Dictionary<BlockMaterialEnum, List<int>>();

    public ChunkMeshData(Chunk chunk)
    {
        indexVert = 0;
        indexUV = 0;
        int vertsNumber = 65536;
        if (chunk != null) vertsNumber = chunk.chunkData.chunkWidth * chunk.chunkData.chunkWidth * chunk.chunkData.chunkHeight * 16;
        vertsNumber= 3000;
        verts = new Vector3[vertsNumber];
        uvs = new Vector2[vertsNumber];
    }
}