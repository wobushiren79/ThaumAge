using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static ChunkComponent;

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

    //下标数据(用于缓存方块的mesh的数据 暂时不需要用)
    //public Dictionary<Vector3, ChunkMeshIndexData> dicIndexData;
    //刷新次数(用于缓存方块的mesh的数据 暂时不需要用)
    //public int refreshNumber;

    public ChunkMeshData()
    {
        verts = new List<Vector3>();
        uvs = new List<Vector2>();

        vertsCollider = new List<Vector3>();
        trisCollider = new List<int>();

        vertsTrigger = new List<Vector3>();
        trisTrigger = new List<int>();

        dicTris = new List<int>[EnumExtension.GetEnumMaxIndex<BlockMaterialEnum>() + 1];
        for (int i = 0; i < dicTris.Length; i++)
        {
            dicTris[i] = new List<int>();
        }

        //dicIndexData = new Dictionary<Vector3, ChunkMeshIndexData>();
        //refreshNumber = 0;

        //默认构建一个触发collider 防止Chunk没有触发时的报错
        //vertsTrigger.AddRange(new List<Vector3> { new Vector3(0, -9999, 0), new Vector3(1, -9999, 0), new Vector3(0, -9999, 1) });
        //trisTrigger.AddRange(new List<int> { 0, 1, 2 });
    }

    public VertexStruct[] GetVertexStruct()
    {
        VertexStruct[] arrayData = new VertexStruct[verts.Count];
        for (int i = 0; i < verts.Count; i++)
        {
            arrayData[i].vertice = verts[i];
            arrayData[i].uv = uvs[i];
        }
        return arrayData;
    }

    public VertexStruct[] GetVertexStructCollider()
    {
        VertexStruct[] arrayData = new VertexStruct[vertsCollider.Count];
        for (int i = 0; i < vertsCollider.Count; i++)
        {
            arrayData[i].vertice = vertsCollider[i];
        }
        return arrayData;
    }

    public VertexStruct[] GetVertexStructTrigger()
    {
        VertexStruct[] arrayData = new VertexStruct[vertsTrigger.Count];
        for (int i = 0; i < vertsTrigger.Count; i++)
        {
            arrayData[i].vertice = vertsTrigger[i];
        }
        return arrayData;
    }

    /// <summary>
    /// 添加mesh下标
    /// </summary>
    //public void AddMeshIndexData(Vector3 position,
    //    int vertsStartIndex, int vertsCount, int trisStartIndex, int trisCount,
    //    int vertsColliderStartIndex = 0, int vertsColliderCount = 0, int trisColliderStartIndex = 0, int trisColliderCount = 0)
    //{
    //    ChunkMeshIndexData chunkMeshIndex = new ChunkMeshIndexData();
    //    chunkMeshIndex.vertsStartIndex = vertsStartIndex;
    //    chunkMeshIndex.vertsCount = vertsCount;
    //    chunkMeshIndex.trisStartIndex = trisStartIndex;
    //    chunkMeshIndex.trisCount = trisCount;

    //    chunkMeshIndex.vertsColliderStartIndex = vertsColliderStartIndex;
    //    chunkMeshIndex.vertsColliderCount = vertsColliderCount;
    //    chunkMeshIndex.trisColliderStartIndex = trisColliderStartIndex;
    //    chunkMeshIndex.trisColliderCount = trisColliderCount;
    //    dicIndexData.Add(position, chunkMeshIndex);
    //}
}


public struct ChunkMeshIndexData
{
    public int vertsStartIndex;
    public int vertsCount;

    public int trisStartIndex;
    public int trisCount;

    public int vertsColliderStartIndex;
    public int vertsColliderCount;

    public int trisColliderStartIndex;
    public int trisColliderCount;
}


