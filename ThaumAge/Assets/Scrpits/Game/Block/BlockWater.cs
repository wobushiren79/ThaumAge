using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Security.Cryptography;

public class BlockWater : BlockLiquid
{
    public override void SetData(Chunk chunk, Vector3Int position, BlockBean blockData)
    {
        base.SetData(chunk, position, blockData);
        if (blockData.contactLevel <= 3)
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
        if (blockData.contactLevel != 0)
        {
            if (!CheckHasHeightContactLevel())
            {
                BlockBean newBlockData = new BlockBean(BlockTypeEnum.None, localPosition, worldPosition);
                Block newBlock = chunk.SetBlock(newBlockData, false);
                chunk.listUpdateBlock.Add(newBlock);
            }
        }
        else
        {
            //添加水方块
            Vector3Int downBlockWorldPosition = worldPosition + Vector3Int.down;
            //设置下方方块
            bool isSuccess = SetCloseWaterBlock(downBlockWorldPosition, 0);
            if (isSuccess)
                return;
            SetCloseWaterBlock(worldPosition + Vector3Int.left, blockData.contactLevel + 1);
            SetCloseWaterBlock(worldPosition + Vector3Int.right, blockData.contactLevel + 1);
            SetCloseWaterBlock(worldPosition + Vector3Int.back, blockData.contactLevel + 1);
            SetCloseWaterBlock(worldPosition + Vector3Int.forward, blockData.contactLevel + 1);
        }
    }

    /// <summary>
    /// 设置靠近的方块为水方块
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <param name="contactLevel"></param>
    /// <returns></returns>
    public bool SetCloseWaterBlock(Vector3Int worldPosition, int contactLevel)
    {
        Block closeBlock = WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition);
        BlockTypeEnum closeBlockType = closeBlock.blockData.GetBlockType();
        BlockInfoBean closeBlockInfo = BlockHandler.Instance.manager.GetBlockInfo(closeBlockType);
        //如果是空方块或者重量等于1
        if (closeBlockType == BlockTypeEnum.None || closeBlockType == BlockTypeEnum.Water || closeBlockInfo.weight == 1)
        {
            if (closeBlockType == BlockTypeEnum.Water && contactLevel > closeBlock.blockData.contactLevel)
            {
                //如果相邻都是水 需要根据关联等级设置
                return false;
            }
            BlockBean newBlockData = new BlockBean(BlockTypeEnum.Water, worldPosition - closeBlock.chunk.worldPosition, worldPosition);
            newBlockData.contactLevel = contactLevel;

            Block newBlock = closeBlock.chunk.SetBlock(newBlockData, false);
            closeBlock.chunk.listUpdateBlock.Add(newBlock);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 检测四周是否有高等级的水流
    /// </summary>
    /// <returns></returns>
    public bool CheckHasHeightContactLevel()
    {
        if (CheckHasHeightContactLevel(worldPosition + Vector3Int.left)
            || CheckHasHeightContactLevel(worldPosition + Vector3Int.right)
             || CheckHasHeightContactLevel(worldPosition + Vector3Int.forward)
              || CheckHasHeightContactLevel(worldPosition + Vector3Int.back))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 检测是否有高等级的水流
    /// </summary>
    /// <param name="wPos"></param>
    /// <returns></returns>
    public bool CheckHasHeightContactLevel(Vector3Int wPos)
    {
        Block closeBlock = WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(wPos);
        if (closeBlock.blockData.contactLevel < blockData.contactLevel)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 刷新数据
    /// </summary>
    public override void RefreshBlock()
    {
        base.RefreshBlock();
        if (blockData.contactLevel <= 3)
        {
            chunk.RegisterEventUpdate(WaterUpdate);
        }
    }
}