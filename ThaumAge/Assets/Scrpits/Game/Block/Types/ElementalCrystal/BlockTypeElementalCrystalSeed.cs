using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockTypeElementalCrystalSeed : Block
{
    //注册每分钟刷新
    public override void InitBlock(Chunk chunk, Vector3Int localPosition, int state)
    {
        base.InitBlock(chunk, localPosition, state);
        if (state == 0 || state == 1)
            chunk.RegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Min);
    }

    public override void EventBlockUpdateForMin(Chunk chunk, Vector3Int localPosition)
    {
        base.EventBlockUpdateForMin(chunk, localPosition);
        chunk.chunkData.GetBlockForLocal(localPosition, out Block block, out BlockDirectionEnum direction);

        BlockBean blockData = chunk.GetBlockData(localPosition);
        BlockMetaCrop blockMetaCrop = null;
        if (blockData == null)
        {
            //如果没有数据
            blockMetaCrop = new BlockMetaCrop();
            blockMetaCrop.growPro = 0;
            blockMetaCrop.isStartGrow = false;

            string meta = ToMetaData(blockMetaCrop);
            blockData = new BlockBean(localPosition, blockInfo.GetBlockType(), direction, meta);
            chunk.SetBlockData(blockData, false);
        }
        else
        {
            if (blockData.meta.IsNull())
            {
                blockMetaCrop = new BlockMetaCrop();
                blockMetaCrop.growPro = 0;
                blockMetaCrop.isStartGrow = false;
            }
            else
            {
                blockMetaCrop = FromMetaData<BlockMetaCrop>(blockData.meta);
            }
        }
        //如果是已经长好的 就不在监听
        if (blockMetaCrop.level > 0)
        {
            chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Min);
            return;
        }
        //成长周期+1
        if (blockMetaCrop.isStartGrow)
        {
            //是否开始生长
            blockMetaCrop.growPro++;
        }
        else
        {
            blockMetaCrop.isStartGrow = true;
        }

        //判断是否已经是最大生长周期
        int lifeCycle = GetCropLifeCycle(blockInfo);
        if (blockMetaCrop.growPro >= lifeCycle)
        {
            chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Min);
            //设置完整的水晶
            chunk.SetBlockForLocal(localPosition, (BlockTypeEnum)blockInfo.remark_int, direction);
        }
        else
        {
            //设置新数据
            blockData.SetBlockMeta(blockMetaCrop);
            chunk.SetBlockData(blockData);
        }
    }

    public override void RefreshBlock(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, int updateChunkType = 1)
    {
        //获取下方方块
        Vector3Int downLocalPosition = localPosition + Vector3Int.down;
        chunk.chunkData.GetBlockForLocal(downLocalPosition, out Block downBlock, out BlockDirectionEnum downBlockDirection);
        //如果下方方块不为石头或者苔藓石
        bool checkDownBlock = CheckDownBlock(downBlock);
        if (!checkDownBlock)
        {
            //移除方块
            chunk.RemoveBlockForLocal(localPosition);
            //创建道具
            ItemsHandler.Instance.CreateItemCptDrop(this, chunk, localPosition + chunk.chunkData.positionForWorld);
        }
    }

    /// <summary>
    /// 检测下方方块是否符合要求
    /// </summary>
    /// <returns></returns>
    public static bool CheckDownBlock(Block downBlock)
    {
        if (downBlock == null)
        {
            return false;
        }
        else
        {
            if (downBlock.blockType == BlockTypeEnum.Stone || downBlock.blockType == BlockTypeEnum.StoneMoss)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 获取作物的生长周期
    /// </summary>
    /// <returns></returns>
    public virtual int GetCropLifeCycle(BlockInfoBean blockInfo)
    {
        return 10;
    }
}