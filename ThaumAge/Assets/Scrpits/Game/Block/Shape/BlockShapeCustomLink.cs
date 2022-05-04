using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockShapeCustomLink : BlockShapeCustom
{

    #region 增加三角
    protected override void BaseAddTrisForCustom(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum blockDirection, int[] trisAdd)
    {
        BlockBean blockData = chunk.GetBlockData(localPosition.x, localPosition.y, localPosition.z);
        BlockMetaBed blockMetaData = Block.FromMetaData<BlockMetaBed>(blockData.meta);
        //只有基础链接才绘制床
        if (blockMetaData != null && blockMetaData.level == 0)
        {
            int index = chunk.chunkMeshData.verts.Count;
            List<int> trisData = chunk.chunkMeshData.dicTris[block.blockInfo.material_type];
            AddTris(index, trisData, trisAdd);
        }

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
    public override void BaseAddVertsUVsColorsForCustom(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, Vector3[] vertsAdd, Vector2[] uvsAdd, Color[] colorsAdd, Vector3[] vertsColliderAdd)
    {
        BlockBean blockData = chunk.GetBlockData(localPosition.x, localPosition.y, localPosition.z);
        BlockMetaBed blockMetaData = Block.FromMetaData<BlockMetaBed>(blockData.meta);
        //只有基础链接才绘制床
        if (blockMetaData != null && blockMetaData.level == 0)
        {
            AddVertsUVsColors(localPosition, direction,
                chunk.chunkMeshData.verts, chunk.chunkMeshData.uvs, chunk.chunkMeshData.colors,
                vertsAdd, uvsAdd, colorsAdd);
        }

        if (block.blockInfo.collider_state == 1)
            AddVerts(localPosition, direction, chunk.chunkMeshData.vertsCollider, vertsColliderAdd);
        else if (block.blockInfo.trigger_state == 1)
            AddVerts(localPosition, direction, chunk.chunkMeshData.vertsTrigger, vertsColliderAdd);
    }
    #endregion
}