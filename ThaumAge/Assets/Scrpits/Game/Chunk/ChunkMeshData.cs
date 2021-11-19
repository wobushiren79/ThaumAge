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
    }

    Dictionary<Vector3Int, ChunkMeshDetailsData> detailsData = new Dictionary<Vector3Int, ChunkMeshDetailsData>();

    public void GetListData(out List<Vector3> verts, out List<Vector2> uvs, out List<int> tris)
    {
        verts = new List<Vector3>();
        uvs = new List<Vector2>();
        tris = new List<int>();
        foreach (var itemData in detailsData)
        {
            ChunkMeshDetailsData itemValue = itemData.Value;
            for (int i = 0; i < itemValue.verts.Length; i++)
            {
                verts.Add(itemValue.verts[i]);
                uvs.Add(itemValue.uvs[i]);
            }
            for (int i = 0; i < itemValue.tris.Length; i++)
            {
                tris.Add(itemValue.tris[i]);
            }
        }
    }

    public void AddVerts(Vector3Int localPosition, Vector3[] verts, Vector2[] uvs,int[] tris)
    {
        detailsData.Add(localPosition, new ChunkMeshDetailsData(verts, uvs, tris));
    }
}


public struct ChunkMeshDetailsData
{
    public Vector3[] verts;
    public Vector2[] uvs;
    public int[] tris;

    public ChunkMeshDetailsData(Vector3[] verts, Vector2[] uvs, int[] tris)
    {
        this.verts = verts;
        this.uvs = uvs;
        this.tris = tris;
    }
}
