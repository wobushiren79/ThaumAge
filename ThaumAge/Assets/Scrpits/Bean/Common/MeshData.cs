using UnityEditor;
using UnityEngine;
using System;

[Serializable]
public class MeshData
{
    public Vector3[] vertices;
    public Vector2[] uv;
    public int[] triangles;

    public MeshData(Mesh mesh)
    {
        vertices = mesh.vertices;
        uv = mesh.uv;
        triangles = mesh.triangles;
    }
}