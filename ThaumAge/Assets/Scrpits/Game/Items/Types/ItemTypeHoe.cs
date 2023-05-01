using UnityEditor;
using UnityEngine;

public class ItemTypeHoe : ItemBaseTool
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

        //如果不能锄地
        if (tagetBlock.blockInfo.plough_state == 0)
            return false;

        //获取上方方块
        Block upBlock = targetChunk.chunkData.GetBlockForLocal(localPosition + Vector3Int.up);

        //如果上方有方块 则无法使用锄头
        if (upBlock != null && upBlock.blockType != BlockTypeEnum.None)
            return false;
        //如果不是锄的正上方 也无法使用

        if (direction != BlockDirectionEnum.UpBack && direction != BlockDirectionEnum.UpForward && direction != BlockDirectionEnum.UpLeft && direction != BlockDirectionEnum.UpRight)
            return false;

        //扣除道具耐久
        if (this is ItemBaseTool itemTool)
        {
            ItemMetaTool itemsDetailsTool = itemData.GetMetaData<ItemMetaTool>();
            //如果没有耐久 不能锄地
            if (itemsDetailsTool.curDurability <= 0)
            {
                return false;
            }
            itemsDetailsTool.AddLife(-1);
            //保存数据
            itemData.SetMetaData(itemsDetailsTool);
            //回调
            EventHandler.Instance.TriggerEvent(EventsInfo.ItemsBean_MetaChange, itemData);
        }

        BlockTypeEnum ploughBlockType = (BlockTypeEnum)tagetBlock.blockInfo.remark_int;
        //替换为耕地方块
        targetChunk.SetBlockForLocal(localPosition, ploughBlockType, direction);

        //播放粒子特效
        BlockCptBreak.PlayBlockCptBreakEffect(ploughBlockType, targetPosition + new Vector3(0.5f, 0.5f, 0.5f));

        //播放音效
        PlayItemSoundUse(itemData, ItemUseTypeEnum.Right);
        return false;
    }
}