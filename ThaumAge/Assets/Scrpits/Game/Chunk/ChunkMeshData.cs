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
    public int indexVertCollider;
    public Vector3[] vertsCollider;
    public int indexTrisCollider;
    public int[] trisCollider;

    //触发使用的三角形合集
    public List<Vector3> vertsTrigger = new List<Vector3>();
    public List<int> trisTrigger = new List<int>();

    //所有三角形合集，根据材质球区分
    public Dictionary<BlockMaterialEnum, List<int>> dicTris = new Dictionary<BlockMaterialEnum, List<int>>();

    public ChunkMeshData(Chunk chunk)
    {
        int vertsNumber = 65536;
        if (chunk != null) 
        {
            vertsNumber = chunk.chunkData.chunkWidth * chunk.chunkData.chunkWidth * chunk.chunkData.chunkHeight;
        }
        int trisNumber = (vertsNumber / 2) * 3;

        indexVert = 0;
        verts = new Vector3[vertsNumber];

        indexUV = 0;
        uvs = new Vector2[vertsNumber];

        indexVertCollider = 0;
        vertsCollider = new Vector3[vertsNumber];

        indexTrisCollider = 0;
        trisCollider = new int[trisNumber];

        //初始化数据
        List<BlockMaterialEnum> blockMaterialsEnum = EnumUtil.GetEnumValue<BlockMaterialEnum>();
        for (int i = 0; i < blockMaterialsEnum.Count; i++)
        {
            BlockMaterialEnum blockMaterial = blockMaterialsEnum[i];
            dicTris.Add(blockMaterial, new List<int>());
        }
    }
}