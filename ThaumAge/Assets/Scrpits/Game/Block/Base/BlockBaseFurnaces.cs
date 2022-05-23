using UnityEditor;
using UnityEngine;

public class BlockBaseFurnaces : Block
{
    public override void InitBlock(Chunk chunk, Vector3Int localPosition, int state)
    {
        base.InitBlock(chunk, localPosition, state);
        //刷新的时候注册事件 
        chunk.chunkComponent.WaitExecuteEndOfFrame(1, () =>
        {
            StartWork(chunk, localPosition);
        });
    }

    /// <summary>
    /// 开始工作
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    public void StartWork(Chunk chunk, Vector3Int localPosition)
    {
        chunk.RegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
    }

    public void SetFurnacesData(Chunk chunk, Vector3Int localPosition,BlockMetaFurnaces blockMetaFurnacesData)
    {
        //获取数据
        BlockBean blockData = chunk.GetBlockData(localPosition);
        //保存数据
        blockData.meta = ToMetaData(blockMetaFurnacesData);

    }

    /// <summary>
    /// 每秒刷新
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    public override void EventBlockUpdateForSec(Chunk chunk, Vector3Int localPosition)
    {
        base.EventBlockUpdateForSec(chunk, localPosition);
        //获取数据
        BlockBean blockData = chunk.GetBlockData(localPosition);
        BlockMetaFurnaces blockMetaData = FromMetaData<BlockMetaFurnaces>(blockData.meta);

        //如果没有烧制的物品 或者剩下的烧制时间为0 则结束
        if (blockMetaData.itemBeforeId == 0 || blockMetaData.fireTimeRemain == 0)
        {
            chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
            return;
        }
        //查询烧制之前的物品能否烧制物品
        ItemsInfoBean itemsInfoBefore = ItemsHandler.Instance.manager.GetItemsInfoById(blockMetaData.itemBeforeId);
        if (itemsInfoBefore.fire_items.IsNull())
        {
            //如果是不能烧制的物品 则结束刷新
            chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
            return;
        }

        //获取烧制的结果
        itemsInfoBefore.GetFireItems(out int[] fireItemsId, out int[] fireItemsNum, out int[] fireTime);

        //检测是否正在烧制物品
        if (blockMetaData.firePro != 0)
        {

        }
        else
        {
            //如果已经有烧制的物品 并且该物品不等于当前物品烧制后的物品 则也不进行烧制
            if (blockMetaData.itemAfterId != 0 && blockMetaData.itemAfterId != fireItemsId[0])
            {
                chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
                return;
            }
            int itemFireTime = fireTime[0];
            blockMetaData.firePro = 1f / itemFireTime;
            blockMetaData.fireTimeRemain--;
        }
        //保存数据
        blockData.meta = ToMetaData(blockMetaData);
    }


}