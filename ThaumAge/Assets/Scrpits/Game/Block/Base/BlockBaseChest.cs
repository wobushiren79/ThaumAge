using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockBaseChest : Block, IBlockForItemsPutOut
{
    protected int chestSize = 7 * 7;
    public override void Interactive(GameObject user, Vector3Int worldPosition, BlockDirectionEnum blockDirection)
    {
        base.Interactive(user, worldPosition, blockDirection);
        //只有player才能打开
        if (user == null || user.GetComponent<Player>() == null)
            return;
        //打开箱子UI
        UIGameChest uiGameChest = UIHandler.Instance.OpenUIAndCloseOther<UIGameChest>();
        uiGameChest.SetData(worldPosition, chestSize);
        //打开盒子
        OpenChest(worldPosition);
        AudioHandler.Instance.PlaySound(1);
    }

    public override List<ItemsBean> GetDropItems(BlockBean blockData)
    {
        List<ItemsBean> listData = base.GetDropItems(blockData);
        ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoByBlockType(blockData.GetBlockType());
        //加一个自己
        listData.Add(new ItemsBean(itemsInfo.id, 1, null));
        //添加箱子里的物品
        if (blockData == null)
            return listData;
        BlockMetaChest blockBoxData = FromMetaData<BlockMetaChest>(blockData.meta);
        if (blockBoxData == null)
            return listData;
        for (int i = 0; i < blockBoxData.items.Length; i++)
        {
            ItemsBean itemData = blockBoxData.items[i];
            listData.Add(itemData);
        }
        return listData;
    }

    /// <summary>
    /// 打开箱子
    /// </summary>
    /// <param name="worldPosition"></param>
    public virtual void OpenChest(Vector3Int worldPosition)
    {
        AnimForChest(worldPosition, 1);
    }

    /// <summary>
    /// 关闭箱子
    /// </summary>
    /// <param name="worldPosition"></param>
    public virtual void CloseChest(Vector3Int worldPosition)
    {
        AnimForChest(worldPosition, 2);
    }

    /// <summary>
    /// 触发箱子
    /// </summary>
    /// <param name="worldPosition"></param>
    public virtual void TriggerChest(Vector3Int worldPosition) 
    {
        AnimForChest(worldPosition, 3);
    }

    /// <summary>
    /// 箱子动画
    /// </summary>
    /// <param name="state">1打开 2关闭 3打开再关闭</param>
    public virtual void AnimForChest(Vector3Int worldPosition, int state)
    {
        //获取箱子方块实例
        GameObject obj = BlockHandler.Instance.GetBlockObj(worldPosition);
        if (!obj) return;
        //设置关闭动画
        Animator blockAnim = obj.GetComponentInChildren<Animator>();
        if (!blockAnim) return;
        if (state == 1)
        {
            blockAnim.Play("BlockChestOpen");
        }
        else if(state == 2)
        {
            blockAnim.Play("BlockChestClose");
        }
        else if (state == 3)
        {
            blockAnim.Play("BlockChestTrigger");
        }

    }

    #region 道具的放入和拿出
    public void ItemsPut(Chunk chunk, Vector3Int localPosition, ItemsBean putItem)
    {
        GetBlockMetaData(chunk, localPosition, out BlockBean blockData, out BlockMetaChest blockMetaData);

        //首先查询箱子是否有同样的道具                     
        //依次增加相应道具的数量 直到该道具的上限
        int itemNumber = ItemsBean.AddOldItems(blockMetaData.items, putItem.itemId, putItem.number, putItem.meta);
        //如果都放进去了 则结束
        if (itemNumber <= 0)
        {

        }
        else
        {
            //如果还没有叠加完道具 曾创建新的用以增加
            itemNumber = ItemsBean.AddNewItems(blockMetaData.items, putItem.itemId, itemNumber, putItem.meta);
            //如果都放进去了 则结束
            if (itemNumber <= 0)
            {

            }
            else
            {
                //设置新的数量
                putItem.number = itemNumber;
                //如果还有 则吐出
                ItemDropBean itemDrop = new ItemDropBean(putItem, ItemDropStateEnum.DropPick, localPosition + new Vector3(0.5f, 1f, 0.5f), Vector3Int.up);
                ItemsHandler.Instance.CreateItemCptDrop(itemDrop);
            }
        }

        blockData.SetBlockMeta(blockMetaData);
        chunk.SetBlockData(blockData);
    }

    public ItemsBean ItemsOut(Chunk chunk, Vector3Int localPosition)
    {
        return null;
    }
    #endregion
}