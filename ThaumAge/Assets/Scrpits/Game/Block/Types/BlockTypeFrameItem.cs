using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockTypeFrameItem : Block
{

    public override void Interactive(GameObject user, Vector3Int worldPosition, BlockDirectionEnum direction)
    {
        Chunk chunk = GetChunk(worldPosition);
        Vector3Int localPosition = worldPosition - chunk.chunkData.positionForWorld;
        BlockBean blockData = chunk.GetBlockData(localPosition);

        BlockMetaFrameItem frameItemData = GetFrameItemMetaData(blockData);
        //如果没有物品 则把手上的物品放上去
        if (frameItemData == null || frameItemData.itemId == 0)
        {
            UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
            ItemsBean itemsData = userData.GetItemsFromShortcut();
            if (itemsData != null && itemsData.itemId != 0)
            {
                //保存数据
                frameItemData = new BlockMetaFrameItem();
                frameItemData.itemId = (int)itemsData.itemId;
                frameItemData.itemNum = 1;
                frameItemData.itemMeta = itemsData.meta;
                blockData.meta = ToMetaData(frameItemData);
                chunk.isSaveData = true;
                //减去身上的道具
                userData.AddItems(itemsData, -1);
                EventHandler.Instance.TriggerEvent(EventsInfo.ItemsBean_MetaChange, itemsData);
            }
        }
        //如果有物品 则掉落
        else
        {
            ItemDropBean itemDropData = new ItemDropBean(frameItemData.itemId, worldPosition + Vector3.one * 0.5f, frameItemData.itemNum, frameItemData.itemMeta, ItemDropStateEnum.DropPick);
            ItemsHandler.Instance.CreateItemCptDrop(itemDropData);

            frameItemData = new BlockMetaFrameItem();
            blockData.meta = ToMetaData(frameItemData);
            chunk.isSaveData = true;
        }
        //刷新预制
        RefreshObjModel(chunk, localPosition, frameItemData);
    }

    /// <summary>
    /// 刷新预制
    /// </summary>
    public override void RefreshObjModel(Chunk chunk, Vector3Int localPosition)
    {
        BlockBean blockData = chunk.GetBlockData(localPosition);
        BlockMetaFrameItem frameItemData = GetFrameItemMetaData(blockData);
        RefreshObjModel(chunk, localPosition, frameItemData);
    }

    protected void RefreshObjModel(Chunk chunk, Vector3Int localPosition, BlockMetaFrameItem frameItemData)
    {
        GameObject objFrame = chunk.GetBlockObjForLocal(localPosition);
        Transform tfFrame = objFrame.transform.Find("Model/Item");
        SpriteRenderer srFrame = tfFrame.GetComponent<SpriteRenderer>();

        if (frameItemData == null || frameItemData.itemId == 0)
        {
            tfFrame.ShowObj(false);
        }
        else
        {
            tfFrame.ShowObj(true);
            ItemsHandler.Instance.manager.GetItemsIconById(frameItemData.itemId, (spIcon) =>
            {
                srFrame.sprite = spIcon;
            });
        }
    }


    public override void ItemUseForSightTarget(Vector3Int targetWorldPosition)
    {
        //展示目标位置
        GameHandler.Instance.manager.playerTargetBlock.Show(targetWorldPosition, this, blockInfo.interactive_state == 1);
    }


    /// <summary>
    /// 获取相框物品ID
    /// </summary>
    protected BlockMetaFrameItem GetFrameItemMetaData(BlockBean blockData)
    {
        if (blockData != null)
        {
            BlockMetaFrameItem blockMetaFrame = FromMetaData<BlockMetaFrameItem>(blockData.meta);
            if (blockMetaFrame != null && blockMetaFrame.itemId != 0)
            {
                return blockMetaFrame;
            }
        }
        return null;
    }

    /// <summary>
    /// 获取掉落物品
    /// </summary>
    public override List<ItemsBean> GetDropItems(BlockBean blockData)
    {
        List<ItemsBean> listDropItems = base.GetDropItems(blockData);
        BlockMetaFrameItem blockMetaFrame = GetFrameItemMetaData(blockData);
        if (blockMetaFrame != null && blockMetaFrame.itemId != 0)
        {
            listDropItems.Add(new ItemsBean(blockMetaFrame.itemId, blockMetaFrame.itemNum, blockMetaFrame.itemMeta));
        }
        return listDropItems;
    }
}