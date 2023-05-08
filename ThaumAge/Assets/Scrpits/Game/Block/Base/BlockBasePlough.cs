using UnityEditor;
using UnityEngine;

public class BlockBasePlough : Block
{
    public override void RefreshBlock(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, int refreshType, int updateChunkType)
    {
        base.RefreshBlock(chunk, localPosition, direction, refreshType, updateChunkType);
        GetCloseBlockByDirection(chunk, localPosition, DirectionEnum.UP,
            out Block blockClose, out Chunk blockChunkClose, out Vector3Int closeLocalPositionClose);
        if (blockClose != null && blockClose.blockType != BlockTypeEnum.None)
        {
            if (blockClose.blockInfo.GetBlockShape() != BlockShapeEnum.CropCross
                && blockClose.blockInfo.GetBlockShape() != BlockShapeEnum.CropCrossOblique
                && blockClose.blockInfo.GetBlockShape() != BlockShapeEnum.CropWell)
            {
                //还原成普通的地面
                chunk.SetBlockForLocal(localPosition, (BlockTypeEnum)blockInfo.remark_int, BlockDirectionEnum.UpForward);
                return;
            }
        }
        if (CheckRoundWater(chunk, localPosition))
        {
            ChangeWaterState(chunk, localPosition, 1);
        }
    }

    /// <summary>
    /// 检测四周是否有水方块
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    /// <returns></returns>
    public bool CheckRoundWater(Chunk chunk, Vector3Int localPosition)
    {
        if (CheckRoundWaterItem(chunk, localPosition, DirectionEnum.Left))
        {
            return true;
        }
        if (CheckRoundWaterItem(chunk, localPosition, DirectionEnum.Right))
        {
            return true;
        }
        if (CheckRoundWaterItem(chunk, localPosition, DirectionEnum.Forward))
        {
            return true;
        }
        if (CheckRoundWaterItem(chunk, localPosition, DirectionEnum.Back))
        {
            return true;
        }
        if (CheckRoundWaterItem(chunk, localPosition, DirectionEnum.Down))
        {
            return true;
        }
        return false;
    }

    protected bool CheckRoundWaterItem(Chunk chunk, Vector3Int localPosition, DirectionEnum directionCheck)
    {
        //判断周围是否有水
        GetCloseBlockByDirection(chunk, localPosition, directionCheck,
            out Block blockClose, out Chunk blockChunkClose, out Vector3Int closeLocalPositionClose);
        if (blockChunkClose == null)
            return false;
        if (blockClose != null && blockClose.blockType == BlockTypeEnum.Water)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 改变耕地的水状态
    /// </summary>
    public void ChangeWaterState(Chunk targetChunk, Vector3Int targetLocalPosition, int waterState)
    {
        //修改耕地的状态
        GetBlockMetaData(targetChunk, targetLocalPosition, out BlockBean blockData, out BlockMetaPlough blockMetaPlough);
        blockMetaPlough.waterState = waterState;
        blockData.meta = ToMetaData(blockMetaPlough);
        targetChunk.SetBlockData(blockData);
        //更新区块
        WorldCreateHandler.Instance.manager.AddUpdateChunk(targetChunk, 1);
    }
}