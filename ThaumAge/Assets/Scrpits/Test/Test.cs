using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Test : BaseMonoBehaviour
{
    public MeshFilter meshFilter;
    public Mesh testMesh;

    private void Start()
    {
        testMesh = meshFilter.mesh;
        Vector3[] vertices = testMesh.vertices;
        Vector2[] uv = testMesh.uv;
        int[] triangles = testMesh.triangles;
    }

}
