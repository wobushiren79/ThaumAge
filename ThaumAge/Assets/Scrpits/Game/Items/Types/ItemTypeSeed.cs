using UnityEditor;
using UnityEngine;

public class ItemTypeSeed : Item
{
    public override bool TargetUseR(GameObject user, ItemsBean itemData, Vector3Int targetPosition, Vector3Int closePosition, BlockDirectionEnum direction)
    {
        bool isBlockUseStop = base.TargetUseR(user, itemData, targetPosition, closePosition, direction);
        if (isBlockUseStop)
            return true;

        Chunk targetChunk = WorldCreateHandler.Instance.manager.GetChunkForWorldPosition(targetPosition);
        Vector3Int localPosition = targetPosition - targetChunk.chunkData.positionForWorld;
        //获取原位置方块
        Block tagetBlock = targetChunk.chunkData.GetBlockForLocal(localPosition);

        //如果不能种地
        if (tagetBlock.blockInfo.plant_state == 0)
            return false;

        //种植位置
        Vector3Int upLocalPosition = localPosition + Vector3Int.up;
        //获取上方方块
        Block upBlock = targetChunk.chunkData.GetBlockForLocal(upLocalPosition);

        //如果上方有方块 则无法种植
        if (upBlock != null && upBlock.blockType != BlockTypeEnum.None)
            return false;

        //种植的方块
        ItemsInfoBean itemsInfo = GetItemsInfo(itemData.itemId);
        BlockTypeEnum plantBlockType = (BlockTypeEnum)itemsInfo.type_id;
        //初始化meta数据
        BlockMetaCrop blockCropData = new BlockMetaCrop();
        blockCropData.isStartGrow = false;
        blockCropData.growPro = 0;
        string metaData = BlockBaseCrop.ToMetaData(blockCropData);
        //替换为种植
        targetChunk.SetBlockForLocal(upLocalPosition, plantBlockType, BlockDirectionEnum.UpForward, metaData);

        //扣除道具
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        userData.AddItems(itemData, -1);
        //刷新UI
        EventHandler.Instance.TriggerEvent(EventsInfo.ItemsBean_MetaChange, itemData);
        //播放音效
        PlayItemSoundUse(itemData, ItemUseTypeEnum.Right);

        return false;
    }
}