using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockBaseLinkLargeChild : Block
{
    public override bool TargetUseBlock(GameObject user, ItemsBean itemData, Chunk targetChunk, Vector3Int blockLocalPosition)
    {
        //获取主方块
        GetBlockMetaData(targetChunk, blockLocalPosition, out BlockBean blockData, out BlockMetaBaseLink blockMetaData);
        Vector3Int basePosition = blockMetaData.GetBasePosition();
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(basePosition, out Block baseBlock, out Chunk baseChunk);
        //使用主方块的事件处理
        return baseBlock.TargetUseBlock(user, itemData, targetChunk, blockLocalPosition);
    }

    public override void DestoryBlock(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
    {
        BlockBaseLinkLarge.DestoryBlockForLinkLarge(chunk, localPosition, direction);
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