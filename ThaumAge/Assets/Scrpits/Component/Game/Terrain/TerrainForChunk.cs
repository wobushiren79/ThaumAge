using UnityEditor;
using UnityEngine;

public class TerrainForChunk : BaseMonoBehaviour
{
    //Chunk的网格
    protected Mesh chunkMesh;

    //噪音采样时会用到的偏移
    protected Vector3 offset0;
    protected Vector3 offset1;
    protected Vector3 offset2;

    protected MeshRenderer meshRenderer;
    protected MeshCollider meshCollider;
    protected MeshFilter meshFilter;
}