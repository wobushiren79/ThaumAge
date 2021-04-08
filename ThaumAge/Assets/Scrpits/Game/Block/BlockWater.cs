using UnityEditor;
using UnityEngine;
using System.Collections;
public class BlockWater : BlockLiquid
{
    public override void SetData(Chunk chunk, Vector3Int position, BlockBean blockData)
    {
        base.SetData(chunk, position, blockData);
        if (blockData.contactLevel <= 5)
        {
            chunk.RegisterEventUpdate(WaterUpdate);
        }
    }

    /// <summary>
    /// 水更新
    /// </summary>
    public void WaterUpdate()
    {
        //取消注册回调
        chunk.UnRegisterEventUpdate(WaterUpdate);
        Vector3Int downBlockWorldPosition = worldPosition + Vector3Int.down;
        //设置下方方块
        bool isSuccess = SetCloseWaterBlock(downBlockWorldPosition, 0);
        if (isSuccess)
            return;
        Vector3Int leftBlockWorldPosition = worldPosition + Vector3Int.left;
        SetCloseWaterBlock(leftBlockWorldPosition, blockData.contactLevel + 1);
        Vector3Int rightBlockWorldPosition = worldPosition + Vector3Int.right;
        SetCloseWaterBlock(rightBlockWorldPosition, blockData.contactLevel + 1);
        Vector3Int backBlockWorldPosition = worldPosition + Vector3Int.back;
        SetCloseWaterBlock(backBlockWorldPosition, blockData.contactLevel + 1);
        Vector3Int forwardBlockWorldPosition = worldPosition + Vector3Int.forward;
        SetCloseWaterBlock(forwardBlockWorldPosition, blockData.contactLevel + 1);
    }

    public bool SetCloseWaterBlock(Vector3Int worldPosition, int contactLevel)
    {
        Block closeBlock = WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition);
        BlockTypeEnum closeBlockType = closeBlock.blockData.GetBlockType();
        if (closeBlockType == BlockTypeEnum.None || closeBlockType == BlockTypeEnum.Water)
        {
            if (closeBlockType == BlockTypeEnum.Water && contactLevel > closeBlock.blockData.contactLevel)
            {
                //如果相邻都是水 需要根据关联等级设置
                return false;
            }
            BlockBean newBlockData = new BlockBean(BlockTypeEnum.Water, worldPosition - closeBlock.chunk.worldPosition, worldPosition);
            newBlockData.contactLevel = contactLevel;
    
            Block newBlock= closeBlock.chunk.SetBlock(newBlockData, false);
            closeBlock.chunk.listUpdateBlock.Add(newBlock);
            return true;
        }
        return false;
    }
}