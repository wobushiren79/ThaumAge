using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChunkMeshData
{
    //普通使用的三角形合集
    public ChunkMeshVertsData vertsData;
    public ChunkMeshUVData uvsData;

    //碰撞使用的三角形合集
    public ChunkMeshVertsData vertsColliderData;
    public ChunkMeshTrisData trisColliderData;

    //触发使用的三角形合集
    public List<Vector3> vertsTrigger = new List<Vector3>();
    public List<int> trisTrigger = new List<int>();

    //所有三角形合集，根据材质球区分
    public ChunkMeshTrisData[] dicTris;

    public ChunkMeshData(Chunk chunk)
    {
        int vertsNumber = 65536;
        if (chunk != null)
        {
            vertsNumber = chunk.chunkData.chunkWidth * chunk.chunkData.chunkWidth * chunk.chunkData.chunkHeight;
        }
        int trisNumber = (vertsNumber / 2) * 3;

        vertsData = new ChunkMeshVertsData(vertsNumber);
        uvsData = new ChunkMeshUVData(vertsNumber);

        vertsColliderData = new ChunkMeshVertsData(vertsNumber);

        trisColliderData = new ChunkMeshTrisData(trisNumber);

        dicTris = new ChunkMeshTrisData[EnumUtil.GetEnumMaxIndex<BlockMaterialEnum>() + 1];
        for (int i = 0; i < dicTris.Length; i++)
        {
            dicTris[i] = new ChunkMeshTrisData(trisNumber);
        }
    }
}

public class ChunkMeshTrisData
{
    public int index;
    public int[] tris;

    public ChunkMeshTrisData(int trisNumber)
    {
        index = 0;
        tris = new int[trisNumber];
    }
}

public class ChunkMeshUVData
{
    public int index;
    public Vector2[] uvs;

    public ChunkMeshUVData(int uvNumber)
    {
        index = 0;
        uvs = new Vector2[uvNumber];
    }
}

public class ChunkMeshVertsData
{
    public int index;
    public Vector3[] verts;

    public ChunkMeshVertsData(int vertsNumber)
    {
        index = 0;
        verts = new Vector3[vertsNumber];
    }
}