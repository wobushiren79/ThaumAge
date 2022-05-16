using UnityEditor;
using UnityEngine;

public class BlockTypeLinkChild : Block
{
    public override void Interactive(GameObject user, Vector3Int worldPosition, BlockDirectionEnum direction)
    {
        base.Interactive(user, worldPosition, direction);
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition, out Block targetBlock, out BlockDirectionEnum targetDirection, out Chunk targetChunk);
        Vector3Int localPosition = worldPosition - targetChunk.chunkData.positionForWorld;
        //获取link数据
        BlockBean blockData = targetChunk.GetBlockData(localPosition.x, localPosition.y, localPosition.z);
        BlockMetaBaseLink blockMetaLinkData = GetLinkBaseBlockData<BlockMetaBaseLink>(blockData.meta);
        //获取基础方块
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(blockMetaLinkData.GetBasePosition(), out Block baseBlock, out BlockDirectionEnum baseDirection, out Chunk baseChunk);
        baseBlock.Interactive(user, worldPosition, direction);
    }
}