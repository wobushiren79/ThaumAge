using UnityEditor;
using UnityEngine;

public class BlockTypeArcanePedestal : Block
{
    public override void CreateBlockModelSuccess(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum blockDirection, GameObject obj)
    {
        base.CreateBlockModelSuccess(chunk, localPosition, blockDirection, obj);

        GetBlockMetaData(chunk, localPosition, out BlockBean blockData, out BlockMetaArcanePedestal blockMetaData);
        RefreshObjModel(chunk, localPosition, blockMetaData.itemsShow);
    }

    public override bool TargetUseBlock(GameObject user, ItemsBean itemData, Chunk targetChunk, Vector3Int targetWorldPosition)
    {
        Vector3Int blockLocalPosition = targetWorldPosition - targetChunk.chunkData.positionForWorld;
        GetBlockMetaData(targetChunk, blockLocalPosition, out BlockBean blockData, out BlockMetaArcanePedestal blockMetaData);
        //如果基座上没有物品
        if (blockMetaData.itemsShow == null || blockMetaData.itemsShow.itemId == 0)
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
                blockMetaData.itemsShow = new ItemsBean(itemData.itemId, 1, itemData.meta);
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
            ItemDropBean itemDropData = new ItemDropBean(blockMetaData.itemsShow, ItemDropStateEnum.DropPick, targetWorldPosition + new Vector3(0.5f, 1.5f, 0.5f), Vector3.up * 1.5f);
            ItemsHandler.Instance.CreateItemCptDrop(itemDropData);
            blockMetaData.itemsShow = null;
        }

        //保存数据
        blockData.SetBlockMeta(blockMetaData);
        targetChunk.SetBlockData(blockData);

        RefreshObjModel(targetChunk, blockLocalPosition, blockMetaData.itemsShow);
        return true;
    }

    /// <summary>
    /// 设置展示的物品
    /// </summary>
    public virtual void RefreshObjModel(Chunk chunk, Vector3Int localPosition, ItemsBean itemsData)
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