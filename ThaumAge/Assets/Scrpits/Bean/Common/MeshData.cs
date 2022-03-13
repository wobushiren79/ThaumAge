﻿using UnityEditor;
using UnityEngine;
using System;

[Serializable]
public class MeshData
{
    public Vector3[] vertices;
    public Vector2[] uv;
    public int[] triangles;

    public Vector3[] verticesCollider;
    public int[] trianglesCollider;

    public Vector3 colliderSize;

    public MeshData(Collider collider, Mesh mesh, float size, Vector3 offset)
    {
        vertices = mesh.vertices;
        uv = mesh.uv;
        triangles = mesh.triangles;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 itemVer = vertices[i];
            Vector3 newVer = itemVer * size + offset;
            vertices[i] = newVer;
        }

        Mesh meshCollider = GetColliderMesh(collider);
        verticesCollider = meshCollider.vertices;
        trianglesCollider = meshCollider.triangles;
    }
    public MeshData(Collider collider, float size, Vector3 offset)
    {
        vertices = new Vector3[0];
        uv = new Vector2[0];
        triangles = new int[0];

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 itemVer = vertices[i];
            Vector3 newVer = itemVer * size + offset;
            vertices[i] = newVer;
        }

        Mesh meshCollider = GetColliderMesh(collider);
        verticesCollider = meshCollider.vertices;
        trianglesCollider = meshCollider.triangles;
    }

    /// <summary>
    /// 获取mesh
    /// </summary>
    /// <returns></returns>
    public Mesh GetMesh()
    {
        Mesh mesh = new Mesh();
        mesh.SetVertices(vertices);
        mesh.SetUVs(0,uv);
        mesh.SetTriangles(triangles,0);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        return mesh;
    }

    /// <summary>
    /// 获取碰撞的mesh
    /// </summary>
    /// <param name="collider"></param>
    /// <returns></returns>
    public Mesh GetColliderMesh(Collider collider)
    {
        Mesh mesh = new Mesh();
        if (collider is BoxCollider boxCollider)
        {
            Vector3 size = boxCollider.size;
            Vector3 center = boxCollider.center;
            mesh.vertices = new Vector3[]
            {
               //左面顶点
               center + new Vector3(-size.x/2,-size.y/2,-size.z/2),
               center + new Vector3(-size.x/2,size.y/2,-size.z/2),
               center + new Vector3(-size.x/2,size.y/2,size.z/2),
               center + new Vector3(-size.x/2,-size.y/2,size.z/2),

               //右面顶点
               center + new Vector3(size.x/2,-size.y/2,-size.z/2),
               center + new Vector3(size.x/2,size.y/2,-size.z/2),
               center + new Vector3(size.x/2,size.y/2,size.z/2),
               center + new Vector3(size.x/2,-size.y/2,size.z/2),
            };
            mesh.triangles = new int[]
            {
                0,2,1, 0,3,2,//左
                4,5,6, 4,6,7,//右
                2,6,5, 2,5,1,//上
                0,4,7, 0,7,3,//下
                0,1,5, 0,5,4,//前
                3,6,2, 3,7,6 //后
            };
            colliderSize = size;
        }
        return mesh;
    }


}