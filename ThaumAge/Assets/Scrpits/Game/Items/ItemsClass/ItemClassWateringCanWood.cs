using UnityEditor;
using UnityEngine;

public class ItemClassWateringCanWood : ItemBaseTool
{
    public override void ItemUseHandle(ItemsBean itemsData, Vector3Int targetPosition, Block targetBlock, BlockDirectionEnum targetBlockDirection, Chunk targetChunk)
    {
        if (targetBlock == null)
            return;
        if (targetBlock.blockInfo.GetBlockShape() == BlockShapeEnum.Plough
            || targetBlock is BlockBaseCrop)
        {

        }
        else
        {
            return;
        }
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

        Vector3Int ploughLocalPosition = targetPosition - targetChunk.chunkData.positionForWorld;
        //修改耕地的状态
        if (targetBlock.blockInfo.GetBlockShape() == BlockShapeEnum.Plough)
        {

        }
        else if (targetBlock is BlockBaseCrop)
        {
            //如果是植物 则获取他下方的一个方块 
            targetBlock.GetCloseBlockByDirection(targetChunk, ploughLocalPosition,DirectionEnum.Down,out Block blockDown,out Chunk blockChunkDown,out Vector3Int localPositionDown);
            ploughLocalPosition = localPositionDown;
        }

        BlockBean blockData = targetChunk.GetBlockData(ploughLocalPosition);
        BlockMetaPlough blockMetaPlough = Block.FromMetaData<BlockMetaPlough>(blockData.meta);
        if (blockMetaPlough == null)
            blockMetaPlough = new BlockMetaPlough();
        blockMetaPlough.waterState = 1;
        blockData.meta = Block.ToMetaData(blockMetaPlough);
        targetChunk.SetBlockData(blockData);
        //更新区块
        WorldCreateHandler.Instance.manager.AddUpdateChunk(targetChunk, 1);
    }
}