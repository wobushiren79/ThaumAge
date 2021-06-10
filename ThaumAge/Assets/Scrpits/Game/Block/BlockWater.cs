using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Security.Cryptography;

public class BlockWater : BlockLiquid
{

    /// <summary>
    /// 水更新
    /// </summary>
    //public void WaterUpdate()
    //{
    //    //添加下方水方块
    //    Vector3Int downBlockWorldPosition = worldPosition + Vector3Int.down;
    //    //设置下方方块
    //    bool isSuccess = SetCloseWaterBlock(downBlockWorldPosition);
    //}

    /// <summary>
    /// 设置靠近的方块为水方块
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <param name="contactLevel"></param>
    /// <returns></returns>
    //public bool SetCloseWaterBlock(Vector3Int worldPosition)
    //{
    //    WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition, out Block closeBlock, out Chunk closeChunk);
    //    if (closeChunk == null)
    //        return false;
    //    if (closeBlock == null)
    //    {
    //        //如果是空方块
    //        BlockBean newBlockData = new BlockBean(BlockTypeEnum.Water, worldPosition - closeChunk.worldPosition, worldPosition);
    //        closeChunk.AddUpdateBlock(newBlockData);
    //        return true;
    //    }
    //    else
    //    {
    //        BlockTypeEnum closeBlockType = closeBlock.blockType;
    //        BlockInfoBean blockInfo = closeBlock.blockInfo;
    //        //如果是空方块 或者总量为1 则替换为水方块
    //        if (closeBlockType == BlockTypeEnum.None || blockInfo.weight == 1)
    //        {
    //            BlockBean newBlockData = new BlockBean(blockType, worldPosition - closeBlock.chunk.worldPosition, worldPosition);

    //            closeBlock.chunk.AddUpdateBlock(newBlockData);
    //            return true;
    //        }
    //        //其他情况则不做变化
    //        else
    //        {
    //            return false;
    //        }
    //    }
    //}

    /// <summary>
    /// 刷新数据
    /// </summary>
    public override void RefreshBlock()
    {
        base.RefreshBlock();
    }
}