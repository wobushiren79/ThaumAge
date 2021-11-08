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
        for (int i = 0; i < blockMaterialsEnum.Count; i++)
        {
            BlockMaterialEnum blockMaterial = blockMaterialsEnum[i];
            chunkMeshData.dicTris.Add(blockMaterial, new List<int>());
        }

        blockCube.BuildBlockNoCheck(null,Vector3Int.zero, direction, chunkMeshData);

        mesh = new Mesh();

        Vector3[] arrayVertsData = chunkMeshData.verts;
        for (int i = 0; i < arrayVertsData.Length; i++)
        {
            Vector3 itemPosition = arrayVertsData[i];
            //向下移动0.5个单位
            itemPosition -= new Vector3(0.5f, 0.5f, 0.5f);
            arrayVertsData[i] = itemPosition;
        }

        mesh.SetVertices(arrayVertsData);
        mesh.SetTriangles(chunkMeshData.dicTris[BlockMaterialEnum.Normal], 0);
        mesh.SetUVs(0, chunkMeshData.uvs);
        meshFilter.mesh = mesh;
    }
}