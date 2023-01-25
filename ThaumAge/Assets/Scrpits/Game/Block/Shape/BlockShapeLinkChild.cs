using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockShapeLinkChild : BlockShapeCube
{
    public BlockShapeLinkChild(Block block) : base(block)
    {

    }

    public override void BuildBlock(Chunk chunk, Vector3Int localPosition)
    {
        BuildFace(chunk, localPosition, BlockDirectionEnum.UpForward, DirectionEnum.Left, vertsColliderAdd, uvsAddLeft, colorsAdd, trisColliderAdd);
    }

    public override void BaseAddTris(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, DirectionEnum face, int[] trisAdd)
    {
        if (block.blockInfo.collider_state == 1)
        {
            List<int> trisCollider = chunk.chunkMeshData.trisCollider;
            int colliderIndex = chunk.chunkMeshData.vertsCollider.Count;
            AddTris(colliderIndex, trisCollider, trisAdd);
        }
        if (block.blockInfo.trigger_state == 1)
        {
            List<int> trisTrigger = chunk.chunkMeshData.trisTrigger;
            int triggerIndex = chunk.chunkMeshData.vertsTrigger.Count;
            AddTris(triggerIndex, trisTrigger, trisAdd);
        }
    }

    public override void BaseAddVertsUVsColors(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, DirectionEnum face,
    Vector3[] vertsAdd, Vector2[] uvsAdd, Color[] colorsAdd)
    {
        if (block.blockInfo.collider_state == 1)
            AddVerts(localPosition, direction, chunk.chunkMeshData.vertsCollider, vertsAdd);
        if (block.blockInfo.trigger_state == 1)
            AddVerts(localPosition, direction, chunk.chunkMeshData.vertsTrigger, vertsAdd);
    }

    public override Mesh GetCompleteMeshData(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
    {
        block.GetBlockMetaData(chunk, localPosition, out BlockBean blockData, out BlockMetaBaseLink blockMetaLinkData);
        if (!blockMetaLinkData.isBreakMesh)
        {
            return new Mesh();
        }
        if (blockMetaLinkData.level == 0)
        {
            //如果自己是基础方块
            return base.GetCompleteMeshData(chunk, localPosition, direction);
        }
        else
        {
            Vector3Int baseBlockWorldPosition = blockMetaLinkData.GetBasePosition();
            //获取基础方块
            WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(baseBlockWorldPosition, out Block baseBlock, out BlockDirectionEnum baseBlockDirection, out Chunk baseChunk);
            return baseBlock.blockShape.GetCompleteMeshData(baseChunk, baseBlockWorldPosition - baseChunk.chunkData.positionForWorld, baseBlockDirection);
        }
    }
}