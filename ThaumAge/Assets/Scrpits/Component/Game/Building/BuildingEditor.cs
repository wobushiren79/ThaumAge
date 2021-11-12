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
        BlockHandler.Instance.manager.InitData();

        BlockCube blockCube = new BlockCube();

        ChunkMeshData chunkMeshData = new ChunkMeshData(null);
        //初始化数据
        List<BlockMaterialEnum> blockMaterialsEnum = EnumUtil.GetEnumValue<BlockMaterialEnum>();
        for (int i = 0; i < blockMaterialsEnum.Count + 1; i++)
        {
            chunkMeshData.dicTris[i] = new ChunkMeshTrisData(100);
        }

        blockCube.BuildBlockNoCheck(null,Vector3Int.zero, direction, chunkMeshData);

        mesh = new Mesh();

        ChunkMeshVertsData vertsData = chunkMeshData.vertsData;
        for (int i = 0; i < vertsData.verts.Length; i++)
        {
            Vector3 itemPosition = vertsData.verts[i];
            //向下移动0.5个单位
            itemPosition -= new Vector3(0.5f, 0.5f, 0.5f);
            vertsData.verts[i] = itemPosition;
        }

        mesh.SetVertices(vertsData.verts);
        mesh.SetTriangles(chunkMeshData.dicTris[(int)BlockMaterialEnum.Normal].tris, 0);
        mesh.SetUVs(0, chunkMeshData.uvsData.uvs);
        meshFilter.mesh = mesh;
    }
}