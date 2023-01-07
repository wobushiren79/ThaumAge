using UnityEditor;
using UnityEngine;

public class BlockTypeArcanePedestal : Block
{
    public override void CreateBlockModelSuccess(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum blockDirection, GameObject obj)
    {
        base.CreateBlockModelSuccess(chunk, localPosition, blockDirection, obj);
        BlockBean blockData = chunk.GetBlockData(localPosition);
        BlockMetaArcanePedestal blockMetaArcanePedestal = null;
        if (blockData == null)
        {

        }
        else
        {
            blockMetaArcanePedestal = blockData.GetBlockMeta<BlockMetaArcanePedestal>();
        }
        if (blockMetaArcanePedestal == null)
        {
            blockMetaArcanePedestal = new BlockMetaArcanePedestal();
        }
        SetShowItem(chunk, localPosition, blockMetaArcanePedestal.itemsShow);
    }

    public override bool TargetUseBlock(GameObject user, ItemsBean itemData, Chunk targetChunk, Vector3Int targetWorldPosition)
    {
        Vector3Int blockLocalPosition = targetWorldPosition - targetChunk.chunkData.positionForWorld;
        BlockBean blockData = targetChunk.GetBlockData(blockLocalPosition);
        targetChunk.GetBlockForLocal(blockLocalPosition, out Block targetBlock, out BlockDirectionEnum targetBlockDirection, out targetChunk);
        BlockMetaArcanePedestal blockMetaArcanePedestal = null;
        if (blockData == null)
        {
            blockData = new BlockBean(blockLocalPosition, blockType, targetBlockDirection);
        }
        else
        {
            blockMetaArcanePedestal = blockData.GetBlockMeta<BlockMetaArcanePedestal>();
        }
        if (blockMetaArcanePedestal == null)
        {
            blockMetaArcanePedestal = new BlockMetaArcanePedestal();
        }
        //如果基座上没有物品
        if (blockMetaArcanePedestal.itemsShow == null || blockMetaArcanePedestal.itemsShow.itemId == 0)
        {
            //如果是空手
            if (itemData == null || itemData.itemId == 0)
            {
                return false;
            }
            //如果不是空手
            else
            {
                //如果能放置
                blockMetaArcanePedestal.itemsShow = new ItemsBean(itemData);
                //扣除道具
                UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
                userData.AddItems(itemData, -1);
                EventHandler.Instance.TriggerEvent(EventsInfo.ItemsBean_MetaChange, itemData);
            }
        }
        //如果基座上有物品
        else
        {
            //先让基座上的物品掉落
            ItemDropBean itemDropData = new ItemDropBean(blockMetaArcanePedestal.itemsShow, ItemDropStateEnum.DropPick, targetWorldPosition + new Vector3(0.5f, 1.5f, 0.5f), Vector3.up * 1.5f);
            ItemsHandler.Instance.CreateItemCptDrop(itemDropData);
            blockMetaArcanePedestal.itemsShow = null;
        }

        //保存数据
        blockData.SetBlockMeta(blockMetaArcanePedestal);
        targetChunk.SetBlockData(blockData);

        SetShowItem(targetChunk, blockLocalPosition, blockMetaArcanePedestal.itemsShow);
        return true;
    }

    /// <summary>
    /// 设置展示的物品
    /// </summary>
    public void SetShowItem(Chunk chunk, Vector3Int localPosition, ItemsBean itemsData)
    {
        GameObject objBlock = chunk.GetBlockObjForLocal(localPosition);
        if (objBlock == null)
            return;
        Transform tfItemShow = objBlock.transform.Find("ItemShow");
        if (itemsData == null || itemsData.itemId == 0)
        {
            tfItemShow.ShowObj(false);
        }
        else
        {
            tfItemShow.ShowObj(true);
            ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemsData.itemId);
            ItemCptShow itemCpt = tfItemShow.GetComponent<ItemCptShow>();
            itemCpt.SetItem(itemsData, itemsInfo, 1);
        }
    }
}