using UnityEditor;
using UnityEngine;

public class BlockTypeLinkChild : Block
{
    public override void Interactive(GameObject user, Vector3Int worldPosition, BlockDirectionEnum direction)
    {
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition, out Block targetBlock, out BlockDirectionEnum targetDirection, out Chunk targetChunk);
        Vector3Int localPosition = worldPosition - targetChunk.chunkData.positionForWorld;
        //获取link数据
        BlockBean blockData = targetChunk.GetBlockData(localPosition.x, localPosition.y, localPosition.z);
        BlockMetaBaseLink blockMetaLinkData = FromMetaData<BlockMetaBaseLink>(blockData.meta);
        //获取基础方块
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(blockMetaLinkData.GetBasePosition(), out Block baseBlock, out BlockDirectionEnum baseDirection, out Chunk baseChunk);
        baseBlock.Interactive(user, blockMetaLinkData.GetBasePosition(), baseDirection);
    }


    public override void DestoryBlock(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
    {
        //获取link数据
        BlockBean blockData = chunk.GetBlockData(localPosition.x, localPosition.y, localPosition.z);
        BlockMetaBaseLink blockMetaLinkData = FromMetaData<BlockMetaBaseLink>(blockData.meta);

        chunk.SetBlockForWorld(blockMetaLinkData.GetBasePosition(),BlockTypeEnum.None);

    }
}