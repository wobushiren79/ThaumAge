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

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        OnValidate();
    }

    private void OnValidate()
    {
        BlockHandler.Instance.manager.InitData();

        BlockCube blockCube = new BlockCube();
        blockCube.SetData(Vector3Int.zero, Vector3Int.zero,blockType, direction);

        Chunk.ChunkRenderData chunkRender = new Chunk.ChunkRenderData();
        //初始化数据
        List<BlockMaterialEnum> blockMaterialsEnum = EnumUtil.GetEnumValue<BlockMaterialEnum>();
        for (int i = 0; i < blockMaterialsEnum.Count; i++)
        {
            BlockMaterialEnum blockMaterial = blockMaterialsEnum[i];
            chunkRender.dicTris.Add(blockMaterial, new List<int>());
        }

        blockCube.BuildBlockNoCheck(chunkRender);

        mesh = new Mesh();

        List<Vector3> listData = chunkRender.verts;
        for (int i = 0; i < listData.Count; i++)
        {
            Vector3 itemPosition = listData[i];
            //向下移动0.5个单位
            itemPosition -= new Vector3(0.5f, 0.5f, 0.5f);
            listData[i] = itemPosition;
        }

        mesh.SetVertices(listData);
        mesh.SetTriangles(chunkRender.dicTris[BlockMaterialEnum.Normal], 0);
        mesh.SetUVs(0, chunkRender.uvs);
        meshFilter.mesh = mesh;
    }
}