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
    public List<Color> colors;

    //碰撞使用的三角形合集
    public List<Vector3> vertsCollider;
    public List<int> trisCollider;

    //触发使用的三角形合集
    public List<Vector3> vertsTrigger = new List<Vector3>();
    public List<int> trisTrigger = new List<int>();

    //所有三角形合集，根据材质球区分
    public List<int>[] dicTris;

    public ChunkMeshData()
    {
        verts = new List<Vector3>();
        uvs = new List<Vector2>();
        colors = new List<Color>();

        vertsCollider = new List<Vector3>();
        trisCollider = new List<int>();

        vertsTrigger = new List<Vector3>();
        trisTrigger = new List<int>();

        dicTris = new List<int>[EnumExtension.GetEnumMaxIndex<BlockMaterialEnum>() + 1];
        for (int i = 0; i < dicTris.Length; i++)
        {
            dicTris[i] = new List<int>();
        }

        //默认构建一个触发collider 防止Chunk没有触发时的报错
        vertsTrigger.AddRange(new List<Vector3> { new Vector3(0, -9999, 0), new Vector3(1, -9999, 0), new Vector3(0, -9999, 1) });
        trisTrigger.AddRange(new List<int> { 0, 1, 2 });
    }

    public VertexStruct[] GetVertexStruct()
    {
        VertexStruct[] arrayData = new VertexStruct[verts.Count];
        for (int i = 0; i < verts.Count; i++)
        {
            arrayData[i].vertice = verts[i];
            arrayData[i].uv = uvs[i];
            arrayData[i].color = colors[i];
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


