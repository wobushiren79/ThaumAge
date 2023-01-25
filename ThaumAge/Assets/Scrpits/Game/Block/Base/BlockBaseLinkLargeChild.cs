using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockBaseLinkLargeChild : Block
{

    public override void DestoryBlock(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
    {
        BlockBaseLinkLarge.DestoryBlockForLinkLarge(chunk, localPosition, direction);
    }

    public override int GetBlockLife(Chunk chunk, Vector3Int localPosition)
    {
        if (chunk == null)
            return 0;
        GetBlockMetaData(chunk, localPosition, out BlockBean blockData, out BlockMetaBaseLink blockMetaData);
        BlockInfoBean baseBlockInfo =  BlockHandler.Instance.manager.GetBlockInfo(blockMetaData.baseBlockType);
        return baseBlockInfo.life;
    }
}