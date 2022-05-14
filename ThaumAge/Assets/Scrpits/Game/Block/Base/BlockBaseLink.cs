using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockBaseLink : Block
{
    protected List<Vector3Int> listLinkPosition;

    /// <summary>
    /// 道具放置时的meta数据
    /// </summary>
    public override string ItemUseMetaData(Vector3Int worldPosition, BlockTypeEnum blockType, BlockDirectionEnum direction, string curMeta)
    {
        BlockMetaBaseLink blockMetaBaseLink = new BlockMetaBaseLink();
        blockMetaBaseLink.level = 0;
        blockMetaBaseLink.linkBasePosition = new Vector3IntBean(worldPosition);
        return ToMetaData(blockMetaBaseLink);
    }

    /// <summary>
    /// 道具放置
    /// </summary>
    public override void ItemUse(
        Vector3Int targetWorldPosition, BlockDirectionEnum targetBlockDirection, Block targetBlock, Chunk targetChunk,
        Vector3Int closeWorldPosition, BlockDirectionEnum closeBlockDirection, Block closeBlock, Chunk closeChunk,
        BlockDirectionEnum direction, string metaData)
    {
        base.ItemUse(targetWorldPosition, targetBlockDirection, targetBlock, targetChunk, closeWorldPosition, closeBlockDirection, closeBlock, closeChunk, direction, metaData);

        CreateLinkBlock(closeChunk, closeWorldPosition - closeChunk.chunkData.positionForWorld, listLinkPosition);
    }

    /// <summary>
    /// 方块销毁
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    /// <param name="direction"></param>
    public override void DestoryBlock(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
    {
        base.DestoryBlock(chunk, localPosition, direction);
        DestoryLinkBlock(chunk, localPosition, direction, listLinkPosition);
    }
}