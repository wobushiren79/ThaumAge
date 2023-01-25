using UnityEditor;
using UnityEngine;

public class BlockBaseLinkChild : Block
{
    public override void Interactive(GameObject user, Vector3Int worldPosition, BlockDirectionEnum direction)
    {
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition, out Block targetBlock, out BlockDirectionEnum targetDirection, out Chunk targetChunk);
        Vector3Int localPosition = worldPosition - targetChunk.chunkData.positionForWorld;
        //获取link数据
        GetBlockMetaData(targetChunk,localPosition, out BlockBean blockData,out BlockMetaBaseLink blockMetaLinkData);
        //获取基础方块
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(blockMetaLinkData.GetBasePosition(), out Block baseBlock, out BlockDirectionEnum baseDirection, out Chunk baseChunk);
        baseBlock.Interactive(user, blockMetaLinkData.GetBasePosition(), baseDirection);
    }


    public override void DestoryBlock(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
    {
        //获取link数据
        GetBlockMetaData(chunk, localPosition, out BlockBean blockData, out BlockMetaBaseLink blockMetaLinkData);
        //主方块设置为null
        chunk.SetBlockForWorld(blockMetaLinkData.GetBasePosition(), BlockTypeEnum.None);
    }

    public override int GetBlockLife(Chunk chunk, Vector3Int localPosition)
    {
        if (chunk == null)
            return 0;
        GetBlockMetaData(chunk, localPosition, out BlockBean blockData, out BlockMetaBaseLink blockMetaData);
        BlockInfoBean baseBlockInfo = BlockHandler.Instance.manager.GetBlockInfo(blockMetaData.baseBlockType);
        return baseBlockInfo.life;
    }
}