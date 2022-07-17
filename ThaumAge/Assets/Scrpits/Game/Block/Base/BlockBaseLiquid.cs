using UnityEditor;
using UnityEngine;

public class BlockBaseLiquid : Block
{
    public static int maxLiquidVolume = 8;
    public override void InitBlock(Chunk chunk, Vector3Int localPosition, int state)
    {
        base.InitBlock(chunk, localPosition, state);
        if (state == 1)
        {
            RegisterEventUpdate(chunk, localPosition);
        }
    }

    /// <summary>
    /// 刷新方块
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    /// <param name="direction"></param>
    public override void RefreshBlock(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
    {
        base.RefreshBlock(chunk, localPosition, direction);
        //刷新的时候注册事件 
        RegisterEventUpdate(chunk, localPosition);
    }

    /// <summary>
    /// 注册事件
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    public void RegisterEventUpdate(Chunk chunk, Vector3Int localPosition)
    {    
        chunk.chunkComponent.WaitExecuteEndOfFrame(1, () =>
        {
            chunk.RegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
        });
    }

    /// <summary>
    /// 每秒刷新
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    public override void EventBlockUpdateForSec(Chunk chunk, Vector3Int localPosition)
    {
        base.EventBlockUpdateForSec(chunk, localPosition);

        Vector3Int worldPosition = localPosition + chunk.chunkData.positionForWorld;
        BlockBean blockData = chunk.GetBlockData(localPosition);
        BlockMetaLiquid blockMetaLiquid = GetBlockMetaLiquid(blockType, localPosition, ref blockData);
        int oldVolume = blockMetaLiquid.volume;
        //设置下方的方块
        SetCloseBlockForDown(chunk, worldPosition, blockData, blockMetaLiquid);

        //最后处理
        if (blockMetaLiquid.volume <= 0)
        {
            //取消注册
            chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
            chunk.SetBlockForLocal(localPosition, BlockTypeEnum.None);
        }
        else
        {
            if (oldVolume != blockMetaLiquid.volume)
            {
                blockData.meta = ToMetaData(blockMetaLiquid);
                chunk.SetBlockData(blockData);

                RefreshBlockRange(chunk, localPosition, blockData.GetDirection());
                WorldCreateHandler.Instance.manager.AddUpdateChunk(chunk, 1);
            }
            else
            {
                //取消注册
                chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
                WorldCreateHandler.Instance.manager.AddUpdateChunk(chunk, 1);
            }
        }
    }

    /// <summary>
    /// 设置下方的方块
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <param name="contactLevel"></param>
    /// <returns></returns>
    public void SetCloseBlockForDown(Chunk chunk, Vector3Int worldPosition, BlockBean blockData, BlockMetaLiquid blockMetaLiquid)
    {
        Vector3Int downWorldPosition = worldPosition + Vector3Int.down;
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(downWorldPosition, out Block downBlock, out BlockDirectionEnum downBlockDirection, out Chunk downChunk);
        //首先检测下方的方块
        if (downBlock == null || downBlock.blockType == BlockTypeEnum.None)
        {
            //如果是空方块 则把本身的水分1份给下方
            blockMetaLiquid.volume--;
            BlockMetaLiquid downBlockMetaLiquid = new BlockMetaLiquid();
            downBlockMetaLiquid.volume = 1;
            downChunk.SetBlockForWorld(downWorldPosition, blockType, meta: ToMetaData(downBlockMetaLiquid));
            return;
        }
        else
        {
            //如果不是空方块 首先判断下方方块是不是同一种方块
            if (downBlock.blockType == blockType)
            {
                Vector3Int downLocalPosition = downWorldPosition - downChunk.chunkData.positionForWorld;
                BlockBean downBlockData = downChunk.GetBlockData(downLocalPosition);
                BlockMetaLiquid downBlockMetaLiquid = GetBlockMetaLiquid(blockType, downLocalPosition, ref downBlockData);
                //如果下方的水没有满 把本身的水分1份给下方的水方块
                if (downBlockMetaLiquid.volume < maxLiquidVolume)
                {
                    blockMetaLiquid.volume--;
                    downBlockMetaLiquid.volume++;
                    downBlockData.meta = ToMetaData(downBlockMetaLiquid);
                    downChunk.SetBlockData(downBlockData);
                    //刷新
                    RefreshBlock(downChunk, downWorldPosition - downChunk.chunkData.positionForWorld, downBlockDirection);
                    RefreshBlockRange(downChunk, downWorldPosition - downChunk.chunkData.positionForWorld, downBlockDirection);
                    WorldCreateHandler.Instance.manager.AddUpdateChunk(downChunk, 1);
                    return;
                }
                //如果下方的水满了 则开始检测四周
                else
                {
                    SetCloseBlockForAround(chunk, worldPosition, blockData, blockMetaLiquid);
                }
            }
            //如果不是空方块 并且不是同一种类型
            else
            {
                //首先判断是不是重量为1的
                if (downBlock.blockInfo.weight == 1)
                {
                    //如果下方方块的重量是1 则冲掉下方方块并且把水分给下方方块一份
                    blockMetaLiquid.volume--;
                    //创建掉落
                    ItemsHandler.Instance.CreateItemCptDrop(downBlock, downChunk, downWorldPosition);

                    BlockMetaLiquid downBlockMetaLiquid = new BlockMetaLiquid();
                    downBlockMetaLiquid.volume = 1;
                    downChunk.SetBlockForWorld(downWorldPosition, blockType, meta: ToMetaData(downBlockMetaLiquid));
                    return;
                }
                //开始检测四周
                else
                {
                    SetCloseBlockForAround(chunk, worldPosition, blockData, blockMetaLiquid);
                }
            }
        }
    }

    /// <summary>
    /// 设置四周的方块
    /// </summary>
    public void SetCloseBlockForAround(Chunk chunk, Vector3Int worldPosition, BlockBean blockData, BlockMetaLiquid blockMetaLiquid)
    {
        bool leftCheck = SetCloseBlockForAroundForDetails(worldPosition + Vector3Int.left, blockData, blockMetaLiquid);
        bool rightCheck = SetCloseBlockForAroundForDetails(worldPosition + Vector3Int.right, blockData, blockMetaLiquid);
        bool forwardCheck = SetCloseBlockForAroundForDetails(worldPosition + Vector3Int.forward, blockData, blockMetaLiquid);
        bool backCheck = SetCloseBlockForAroundForDetails(worldPosition + Vector3Int.back, blockData, blockMetaLiquid);
        if (leftCheck || rightCheck || forwardCheck || backCheck)
        {
            //只要有一个可以流动 则减少1份体积
            blockMetaLiquid.volume--;
            //刷新
            WorldCreateHandler.Instance.manager.AddUpdateChunk(chunk, 1);
        }
    }

    /// <summary>
    /// 检测四周的方块是否可以流动 并流动
    /// </summary>
    protected bool SetCloseBlockForAroundForDetails(Vector3Int wPos, BlockBean blockData, BlockMetaLiquid blockMetaLiquid)
    {
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(wPos, out Block closeBlock, out BlockDirectionEnum closeDirection, out Chunk closeChunk);
        //如果没有区块 则不能流动
        if (closeChunk == null)
            return false;
        //如果方块是空气 则可以流动
        if (closeBlock == null || closeBlock.blockType == BlockTypeEnum.None)
        {
            //如果是只有1格水 则不流动了
            if(blockMetaLiquid.volume == 1)
            {
                return false;
            }
            else
            {
                BlockMetaLiquid closeBlockMetaLiquid = new BlockMetaLiquid();
                closeBlockMetaLiquid.volume = 1;
                closeChunk.SetBlockForWorld(wPos, blockType, meta: ToMetaData(closeBlockMetaLiquid));
                return true;
            }
        }
        else
        {
            //如果不是空方块 首先判断方块是不是同一种方块
            if (closeBlock.blockType == blockType)
            {
                Vector3Int closeLocalPosition = wPos - closeChunk.chunkData.positionForWorld;
                BlockBean closeBlockData = closeChunk.GetBlockData(closeLocalPosition);
                BlockMetaLiquid closeBlockMetaLiquid = GetBlockMetaLiquid(blockType, closeLocalPosition, ref closeBlockData);
                //如果靠近液体方块水量比本身的少2个 则把本身的水分给1份
                if (blockMetaLiquid.volume > closeBlockMetaLiquid.volume + 1)
                {
                    closeBlockMetaLiquid.volume++;
                    closeBlockData.meta = ToMetaData(closeBlockMetaLiquid);
                    closeChunk.SetBlockData(closeBlockData);
                    //刷新
                    RefreshBlock(closeChunk, wPos - closeChunk.chunkData.positionForWorld, closeDirection);
                    RefreshBlockRange(closeChunk, wPos - closeChunk.chunkData.positionForWorld, closeDirection);
                    WorldCreateHandler.Instance.manager.AddUpdateChunk(closeChunk, 1);
                    return true;
                }
                //如果旁边的水比本身多 则不转移
                else
                {
                    return false;
                }
            }
            //如果不是空方块 并且不是同一种类型
            else
            {
                //首先判断是不是重量为1的
                if (closeBlock.blockInfo.weight == 1)
                {
                    //如果是只有1格水 则不流动了
                    if (blockMetaLiquid.volume == 1)
                    {
                        return false;
                    }
                    else
                    {
                        //如果方块的重量是1 则冲掉方块并且把水分给方块一份
                        //创建掉落
                        ItemsHandler.Instance.CreateItemCptDrop(closeBlock, closeChunk, wPos);

                        BlockMetaLiquid closeBlockMetaLiquid = new BlockMetaLiquid();
                        closeBlockMetaLiquid.volume = 1;
                        closeChunk.SetBlockForWorld(wPos, blockType, meta: ToMetaData(closeBlockMetaLiquid));
                        return true;
                    }
                }
                //开始检测四周
                else
                {
                    return false;
                }
            }
        }
    }

    /// <summary>
    /// 获取液体数据
    /// </summary>
    /// <param name="localPosition"></param>
    /// <param name="blockData"></param>
    /// <returns></returns>
    public static BlockMetaLiquid GetBlockMetaLiquid(BlockTypeEnum blockType, Vector3Int localPosition, ref BlockBean blockData)
    {
        BlockMetaLiquid blockMetaLiquid;
        if (blockData == null || blockData.meta.IsNull())
        {
            //设置默认水的数据
            blockMetaLiquid = new BlockMetaLiquid();
            blockMetaLiquid.volume = maxLiquidVolume;
            blockData = new BlockBean(localPosition, blockType);
        }
        else
        {
            blockMetaLiquid = FromMetaData<BlockMetaLiquid>(blockData.meta);
        }
        return blockMetaLiquid;
    }
}