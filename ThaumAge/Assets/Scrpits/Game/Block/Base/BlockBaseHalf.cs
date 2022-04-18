using UnityEditor;
using UnityEngine;

public class BlockBaseHalf : Block
{
    public override string GetUseMetaData(Vector3Int worldPosition, BlockTypeEnum blockType, BlockDirectionEnum blockDirection, string curMeta)
    {
        BlockMetaCubeHalf blockMeta = FromMetaData<BlockMetaCubeHalf>(curMeta);
        if (blockMeta == null)
            blockMeta = new BlockMetaCubeHalf();
        int direction = MathUtil.GetUnitTen((int)blockDirection);
        switch (direction) 
        {
            case 1:
                blockMeta.SetHalfPosition(DirectionEnum.Down);
                break;
            case 2:
                blockMeta.SetHalfPosition(DirectionEnum.UP);
                break;
            case 3:
                blockMeta.SetHalfPosition(DirectionEnum.Right);
                break;
            case 4:
                blockMeta.SetHalfPosition(DirectionEnum.Left);
                break;
            case 5:
                blockMeta.SetHalfPosition(DirectionEnum.Forward);
                break;
            case 6:
                blockMeta.SetHalfPosition(DirectionEnum.Back);
                break;
        }
       return ToMetaData(blockMeta);
    }

    public override void ItemUse(
        Vector3Int targetWorldPosition, BlockDirectionEnum targetBlockDirection, Block targetBlock, Chunk targetChunk, 
        Vector3Int closeWorldPosition, BlockDirectionEnum closeBlockDirection, Block closeBlock, Chunk closeChunk, 
        BlockDirectionEnum direction, string metaData)
    {
        //如果同样是半砖 并且是同一种类
        if(targetBlock.blockInfo.GetBlockShape() == BlockShapeEnum.CubeHalf && targetBlock.blockType == blockType)
        {
            BlockBean targetBlockData = targetChunk.GetBlockData(targetWorldPosition - targetChunk.chunkData.positionForWorld);
            BlockMetaCubeHalf targetMetaData = targetBlockData.GetBlockMeta<BlockMetaCubeHalf>();
            BlockMetaCubeHalf curMetaData = FromMetaData<BlockMetaCubeHalf>(metaData);

            DirectionEnum targetHalfPosition = targetMetaData.GetHalfPosition();
            DirectionEnum curHalfPosition = curMetaData.GetHalfPosition();
            bool isMerge = false;
            switch (targetHalfPosition)
            {
                case DirectionEnum.UP:
                    if (curHalfPosition == DirectionEnum.Down)
                        isMerge = true;
                    break;
                case DirectionEnum.Down:
                    if (curHalfPosition == DirectionEnum.UP)
                        isMerge = true;
                    break;
                case DirectionEnum.Left:
                    if (curHalfPosition == DirectionEnum.Right)
                        isMerge = true;
                    break;
                case DirectionEnum.Right:
                    if (curHalfPosition == DirectionEnum.Left)
                        isMerge = true;
                    break;
                case DirectionEnum.Forward:
                    if (curHalfPosition == DirectionEnum.Back)
                        isMerge = true;
                    break;
                case DirectionEnum.Back:
                    if (curHalfPosition == DirectionEnum.Forward)
                        isMerge = true;
                    break;
            }
            if (isMerge)
            {         
                targetChunk.SetBlockForWorld(targetWorldPosition, (BlockTypeEnum)blockInfo.remark_int, BlockDirectionEnum.UpForward);
            }
            else
            {
                base.ItemUse(targetWorldPosition, targetBlockDirection, targetBlock, targetChunk, closeWorldPosition, closeBlockDirection, closeBlock, closeChunk, direction, metaData);
            }
        }
        else
        {
            base.ItemUse(targetWorldPosition, targetBlockDirection, targetBlock, targetChunk, closeWorldPosition, closeBlockDirection, closeBlock, closeChunk, direction, metaData);
        }     
    }
}