using UnityEditor;
using UnityEngine;

public class BlockBaseLiquid : Block
{
    public override void InitBlock(Chunk chunk, Vector3Int localPosition, int state)
    {
        base.InitBlock(chunk, localPosition, state);
        if (state == 1)
        {
            //刷新的时候注册事件 
            chunk.WaitExecuteEndOfFrame(1, () =>
            {
                chunk.RegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
            });
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
        EventBlockUpdateForSec(chunk, localPosition);
        //chunk.RegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
    }

    /// <summary>
    /// 每秒刷新
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    public override void EventBlockUpdateForSec(Chunk chunk, Vector3Int localPosition)
    {
        base.EventBlockUpdateForSec(chunk, localPosition);
        //取消注册
        chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
        Vector3Int worldPosition = localPosition + chunk.chunkData.positionForWorld;
        //设置周围方块
        SetCloseBlock(chunk, worldPosition);
    }

    /// <summary>
    /// 设置靠近的方块为水方块
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <param name="contactLevel"></param>
    /// <returns></returns>
    public void SetCloseBlock(Chunk chunk, Vector3Int worldPosition)
    {
        Vector3Int downWorldPosition = worldPosition + Vector3Int.down;
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(downWorldPosition, out Block downBlock, out BlockDirectionEnum downBlockDirection, out Chunk downChunk);
        if (downBlock == null || downBlock.blockType == BlockTypeEnum.None)
        {
            //如果是空方块 替换成当前方块
            downChunk.SetBlockForWorld(downWorldPosition, blockType);
            return;
        }
        else
        {
            Vector3Int upWorldPosition = worldPosition + Vector3Int.up;
            WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(upWorldPosition, out Block upBlock, out BlockDirectionEnum upBlockDirection, out Chunk upChunk);
            if (downBlock.blockInfo.weight == 1)
            {
                //如果下方方块的重量是1 则冲掉下方方块
                downChunk.SetBlockForWorld(downWorldPosition, blockType);
                //创建掉落
                ItemsHandler.Instance.CreateItemCptDrop(downBlock, downChunk, downWorldPosition);
            }
            else if (downBlock.blockType == blockType)
            {
                //下方有水方块并且还不是基础方块 则设置成基础水方块
                BlockBean downBlockData = downChunk.GetBlockData(downWorldPosition - downChunk.chunkData.positionForWorld);
                if (downBlockData != null)
                {
                    BlockLiquidBean downBlockLiquid = FromMetaData<BlockLiquidBean>(downBlockData.meta);
                    if (downBlockLiquid != null && downBlockLiquid.level == 1)
                    {
                        downChunk.SetBlockForWorld(downWorldPosition, blockType);
                    }
                }
            }
            else
            {
                BlockBean blockData = chunk.GetBlockData(worldPosition - chunk.chunkData.positionForWorld);
                if (blockData != null)
                {
                    //如果有数据 需要判断是几级水流
                    BlockLiquidBean blockLiquid = FromMetaData<BlockLiquidBean>(blockData.meta);
                    if (blockLiquid != null)
                    {
                        //如果是1子级 则不再向外扩散
                        if (blockLiquid.level == 1)
                        {

                            //检测四周的基础水方块
                            int number = 0;
                            number = CheckRangeBlockWater(chunk, worldPosition + Vector3Int.left, number);
                            number = CheckRangeBlockWater(chunk, worldPosition + Vector3Int.right, number);
                            number = CheckRangeBlockWater(chunk, worldPosition + Vector3Int.forward, number);
                            number = CheckRangeBlockWater(chunk, worldPosition + Vector3Int.back, number);
                            //如果有2块以上的基础水方块 则自身变为一个基础水方块
                            if (number >= 2)
                            {
                                chunk.SetBlockForWorld(worldPosition, blockType, BlockDirectionEnum.UpForward);
                            }
                            //如果一个基础水方块都没有 则设置为空气
                            if (number == 0)
                            {
                                chunk.SetBlockForWorld(worldPosition, BlockTypeEnum.None, BlockDirectionEnum.UpForward);
                            }
                            return;
                        }
                    }
                }
                //如果是实体方块 则判断周围是否有方块，如果周围都有镂空的方块 则水流失
                CheckRangeBlockLeak(worldPosition + Vector3Int.left);
                CheckRangeBlockLeak(worldPosition + Vector3Int.right);
                CheckRangeBlockLeak(worldPosition + Vector3Int.forward);
                CheckRangeBlockLeak(worldPosition + Vector3Int.back);
            }
        }
    }

    /// <summary>
    /// 检测四周方块是否漏水
    /// </summary>
    /// <returns></returns>
    protected void CheckRangeBlockLeak(Vector3Int worldPosition)
    {
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition, out Block closeBlock, out BlockDirectionEnum closeBlockDirection, out Chunk closeChunk);
        if (closeChunk == null)
        {
            //如果没有区块 则不处理
            return;
        }
        else
        {
            if (closeBlock == null || closeBlock.blockType == BlockTypeEnum.None)
            {
                BlockLiquidBean blockLiquid = new BlockLiquidBean();
                blockLiquid.level = 1;
                //如果没有方块 则漏水
                closeChunk.SetBlockForWorld(worldPosition, blockType, BlockDirectionEnum.UpForward, ToMetaData(blockLiquid));
                return;
            }
            else
            {
                //如果是重量为1的 则冲散
                if (closeBlock.blockInfo.weight == 1)
                {
                    BlockLiquidBean blockLiquid = new BlockLiquidBean();
                    blockLiquid.level = 1;

                    closeChunk.SetBlockForWorld(worldPosition, blockType, BlockDirectionEnum.UpForward, ToMetaData(blockLiquid));
                    //创建掉落
                    ItemsHandler.Instance.CreateItemCptDrop(closeBlock, closeChunk, worldPosition);
                    return;
                }
            }
        }
        return;
    }

    /// <summary>
    /// 检测四周是否有基础水方块
    /// </summary>
    protected int CheckRangeBlockWater(Chunk chunk, Vector3Int worldPosition, int number)
    {
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition, out Block closeBlock, out BlockDirectionEnum closeBlockDirection, out Chunk closeChunk);
        if (closeChunk == null || closeBlock == null || closeBlock.blockType != blockType)
        {
            return number;
        }
        BlockBean blockData = closeChunk.GetBlockData(worldPosition - closeChunk.chunkData.positionForWorld);
        if (blockData == null)
        {
            return (number + 1);
        }
        BlockLiquidBean blockLiquid = FromMetaData<BlockLiquidBean>(blockData.meta);
        if (blockLiquid == null || blockLiquid.level == 0)
        {
            return (number + 1);
        }
        return number;
    }

    /// <summary>
    /// 获取meta数据
    /// </summary>
    /// <returns></returns>
    public static string ToMetaData<T>(T blockData) where T : BlockLiquidBean
    {
        return JsonUtil.ToJson(blockData);
    }

    public static T FromMetaData<T>(string data) where T : BlockLiquidBean
    {
        return JsonUtil.FromJson<T>(data);
    }
}