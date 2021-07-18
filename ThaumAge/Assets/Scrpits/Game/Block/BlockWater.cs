using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Security.Cryptography;

public class BlockWater : BlockLiquid
{
    public override void InitBlock(Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {
        base.InitBlock(chunk, localPosition, direction);
        chunk.RegisterEventUpdate(localPosition);
    }

    public override void EventBlockUpdate(Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {
        base.EventBlockUpdate(chunk, localPosition, direction);
        //添加下方水方块
        Vector3Int downBlockWorldPosition = localPosition + Vector3Int.down + chunk.chunkData.positionForWorld;
        //设置下方方块
        bool isSuccess = SetCloseFlowBlock(downBlockWorldPosition);
        //取消注册
        chunk.UnRegisterEventUpdate(localPosition);
    }

    /// <summary>
    /// 设置靠近的方块为水方块
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <param name="contactLevel"></param>
    /// <returns></returns>
    public bool SetCloseFlowBlock(Vector3Int worldPosition)
    {
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition, out BlockTypeEnum closeBlock, out DirectionEnum closeBlockDirection, out Chunk closeChunk);
        if (closeChunk == null)
            return false;
        if (closeBlock == BlockTypeEnum.None)
        {
            //如果是空方块 替换成当前方块
            closeChunk.SetBlockForWorld(worldPosition, blockType);
            //更新区块
            WorldCreateHandler.Instance.HandleForUpdateChunk(true, null);
            return true;
        }
        else
        {
            //获取靠近方块信息
            BlockInfoBean closeBlockInfo = BlockHandler.Instance.manager.GetBlockInfo(closeBlock);
            //如果是空方块 或者总量为1 则替换为当前方块
            if (blockInfo.weight == 1)
            {
                //如果是空方块 替换成岩浆
                closeChunk.SetBlockForWorld(worldPosition, blockType);
                //更新区块
                WorldCreateHandler.Instance.HandleForUpdateChunk(true, null);
                return true;
            }
            //其他情况则不做变化
            else
            {
                return false;
            }
        }
    }


    /// <summary>
    /// 删除方块
    /// </summary>
    /// <param name="chunk"></param>
    public override void DestoryBlock(Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {
        base.DestoryBlock(chunk, localPosition, direction);
        //取消注册
        chunk.UnRegisterEventUpdate(localPosition);
    }
}