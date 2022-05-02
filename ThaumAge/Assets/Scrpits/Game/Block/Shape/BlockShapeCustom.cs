using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockShapeCustom : BlockShape
{
    //自定义形状方块所有的数据
    public MeshDataCustom blockMeshData;
    public Vector3[] vertsColliderAddCustom;
    public int[] trisColliderAddCustom;
    public override void InitData(Block block)
    {
        base.InitData(block);
        blockMeshData = block.blockInfo.GetBlockMeshData();
        vertsAdd = blockMeshData.mainMeshData.vertices;
        trisAdd = blockMeshData.mainMeshData.triangles;
        uvsAdd = blockMeshData.mainMeshData.uv;
        colorsAdd = new Color[uvsAdd.Length];
        for (int i = 0; i < colorsAdd.Length; i++)
        {
            colorsAdd[i] = Color.white;
        }

        if (!blockMeshData.verticesCollider.IsNull())
            vertsColliderAddCustom = blockMeshData.verticesCollider;

        if (!blockMeshData.trianglesCollider.IsNull())
            trisColliderAddCustom = blockMeshData.trianglesCollider;
    }

    public override void BuildBlock(Chunk chunk, Vector3Int localPosition)
    {
        if (block.blockType != BlockTypeEnum.None)
        {
            BuildFace(chunk, localPosition, vertsAdd, uvsAdd, colorsAdd, trisAdd);
        }
    }

    #region 增加三角
    public override void BaseAddTris(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum blockDirection, int[] trisAdd)
    {
        BaseAddTrisForCustom(chunk, localPosition, blockDirection, trisAdd);
    }

    protected virtual void BaseAddTrisForCustom(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum blockDirection, int[] trisAdd)
    {
        int index = chunk.chunkMeshData.verts.Count;
        List<int> trisData = chunk.chunkMeshData.dicTris[block.blockInfo.material_type];
        AddTris(index, trisData, trisAdd);

        if (block.blockInfo.collider_state == 1)
        {
            List<int> trisCollider = chunk.chunkMeshData.trisCollider;
            int colliderIndex = chunk.chunkMeshData.vertsCollider.Count;
            AddTris(colliderIndex, trisCollider, trisColliderAddCustom);
        }
        else if (block.blockInfo.trigger_state == 1)
        {
            List<int> trisTrigger = chunk.chunkMeshData.trisTrigger;
            int triggerIndex = chunk.chunkMeshData.vertsTrigger.Count;
            AddTris(triggerIndex, trisTrigger, trisColliderAddCustom);
        }
    }
    #endregion



    #region 增加顶点UV颜色
    public override void BaseAddVertsUVsColors(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, Vector3[] vertsAdd, Vector2[] uvsAdd, Color[] colorsAdd)
    {
        BaseAddVertsUVsColorsForCustom(chunk, localPosition, direction, vertsAdd, uvsAdd, colorsAdd, vertsColliderAdd);
    }

    public virtual void BaseAddVertsUVsColorsForCustom(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, Vector3[] vertsAdd, Vector2[] uvsAdd, Color[] colorsAdd, Vector3[] vertsColliderAdd)
    {
        AddVertsUVsColors(localPosition, direction,
            chunk.chunkMeshData.verts, chunk.chunkMeshData.uvs, chunk.chunkMeshData.colors,
            vertsAdd, uvsAdd, colorsAdd);
        if (block.blockInfo.collider_state == 1)
            AddVerts(localPosition, direction, chunk.chunkMeshData.vertsCollider, vertsColliderAdd);
        else if (block.blockInfo.trigger_state == 1)
            AddVerts(localPosition, direction, chunk.chunkMeshData.vertsTrigger, vertsColliderAdd);
    }
    #endregion

    /// <summary>
    /// 获取完整mesh数据
    /// </summary>
    /// <returns></returns>
    public override Mesh GetCompleteMeshData(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
    {
        return blockMeshData.GetMainMesh();
    }
}