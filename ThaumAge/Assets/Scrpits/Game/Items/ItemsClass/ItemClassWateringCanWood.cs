using UnityEditor;
using UnityEngine;

public class ItemClassWateringCanWood : ItemBaseTool
{
    public override void ItemUseHandle(ItemsBean itemsData, Vector3Int targetPosition, Block targetBlock, BlockDirectionEnum targetBlockDirection, Chunk targetChunk)
    {
        if (targetBlock == null)
            return;
        if (targetBlock.blockInfo.GetBlockShape() != BlockShapeEnum.Plough)
            return;
        ItemsDetailsToolBean itemsDetailsTool = itemsData.GetMetaData<ItemsDetailsToolBean>();
        //如果没有耐久了 则不执行
        if (itemsDetailsTool.life <= 0)
        {
            return;
        }
        //扣除耐久
        itemsDetailsTool.AddLife(-1);
        //保存数据 道具数据
        itemsData.SetMetaData(itemsDetailsTool);
        //回调
        EventHandler.Instance.TriggerEvent(EventsInfo.ItemsBean_MetaChange, itemsData);

        //修改耕地的状态
        BlockBean blockData = targetChunk.GetBlockData(targetPosition - targetChunk.chunkData.positionForWorld);
    }
}