using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockBaseBed : Block
{

    public override string ItemUseMetaData(Vector3Int worldPosition, BlockTypeEnum blockType, BlockDirectionEnum direction, string curMeta)
    {
        BlockMetaBed blockDoor = new BlockMetaBed();
        blockDoor.level = 0;
        blockDoor.linkBasePosition = new Vector3IntBean(worldPosition);
        return ToMetaData(blockDoor);
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


        Vector3Int localPosition = closeWorldPosition - closeChunk.chunkData.positionForWorld;
        BlockDirectionEnum blockDirection = closeChunk.chunkData.GetBlockDirection(localPosition.x, localPosition.y,localPosition.z);

        DirectionEnum rotateDirection = blockShape.GetRotateDirection(blockDirection, DirectionEnum.Back);
        Vector3Int offsetBack = GetCloseOffsetByDirection(rotateDirection);
        CreateLinkBlock(closeChunk, localPosition, new List<Vector3Int>() { offsetBack });
    }

    public override void DestoryBlock(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
    {
        base.DestoryBlock(chunk, localPosition, direction);

        DirectionEnum rotateDirection = blockShape.GetRotateDirection(direction, DirectionEnum.Back);
        Vector3Int offsetBack = GetCloseOffsetByDirection(rotateDirection);
        DestoryLinkBlock(chunk, localPosition, direction, new List<Vector3Int>() { offsetBack });
    }
}