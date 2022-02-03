using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class BuildingEditor : BaseMonoBehaviour
{
    public Mesh mesh;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    public BlockTypeEnum blockType;
    public DirectionEnum direction;
    public float randomRate = 1;

    public void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        OnValidate();
    }

    public void OnValidate()
    {
        //BlockHandler.Instance.manager.InitData();

        //BlockShapeCube blockCube = new BlockShapeCube();

        //ChunkMeshData chunkMeshData = new ChunkMeshData();

        //blockCube.BuildBlockNoCheck(null,Vector3Int.zero, direction);

        //mesh = new Mesh();

        //List<Vector3> verts = chunkMeshData.verts;
        //for (int i = 0; i < verts.Count; i++)
        //{
        //    Vector3 itemPosition = verts[i];
        //    //向下移动0.5个单位
        //    itemPosition -= new Vector3(0.5f, 0.5f, 0.5f);
        //    verts[i] = itemPosition;
        //}

        //mesh.SetVertices(verts);
        //mesh.SetTriangles(chunkMeshData.dicTris[(int)BlockMaterialEnum.Normal], 0);
        //mesh.SetUVs(0, chunkMeshData.uvs);
        //meshFilter.mesh = mesh;
    }
}