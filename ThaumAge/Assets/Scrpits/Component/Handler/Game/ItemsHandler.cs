using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ItemsHandler : BaseHandler<ItemsHandler, ItemsManager>
{
    /// <summary>
    /// 使用物品
    /// </summary>
    /// <param name="user"></param>
    /// <param name="itemsData"></param>
    /// <param name="type">0 左键 1右键 2F建</param>
    public void UseItem(GameObject user, ItemsBean itemsData, ItemUseTypeEnum useType)
    {
        Item item;
        if (itemsData == null || itemsData.itemId == 0)
        {
            //如果手上没有东西
            item = manager.GetRegisterItem(itemsData.itemId, ItemsTypeEnum.Block);
        }
        else
        {
            //如果手上有东西
            ItemsInfoBean itemsInfo = manager.GetItemsInfoById(itemsData.itemId);
            //获取对应得处理类
            item = manager.GetRegisterItem(itemsInfo.id, (ItemsTypeEnum)itemsInfo.items_type);
        }
        item.Use(user, itemsData, useType);
        if (useType == ItemUseTypeEnum.E)
        {
            //如果是交互 则不播放动画
        }
        else
        {
            item.UseForAnim(user, itemsData);
        }
    }


    /// <summary>
    /// 瞄准使用的目标
    /// </summary>
    public void UseItemForSightTarget(ItemsBean itemsData)
    {
        Item item;
        if (itemsData == null || itemsData.itemId == 0)
        {
            //如果手上没有东西
            item = manager.GetRegisterItem(ItemsTypeEnum.Block);
        }
        else
        {
            //如果手上有东西
            ItemsInfoBean itemsInfo = manager.GetItemsInfoById(itemsData.itemId);
            //获取对应得处理类
            item = manager.GetRegisterItem(itemsInfo.id, (ItemsTypeEnum)itemsInfo.items_type);
        }
        item.UseForSightTarget(itemsData);
    }

    /// <summary>
    /// 创建掉落道具实例
    /// </summary>
    public void CreateItemCptDropList(List<ItemsBean> itemDatas, Vector3 position, ItemDropStateEnum itemDropState)
    {
        CreateItemCptDropList(itemDatas, position, itemDropState, Vector3.zero);
    }
    public void CreateItemCptDropList(List<ItemsBean> itemDatas, Vector3 position, ItemDropStateEnum ItemCptDropState, Vector3 dropDirection)
    {
        for (int i = 0; i < itemDatas.Count; i++)
        {
            CreateItemCptDrop(itemDatas[i], position, ItemCptDropState, dropDirection);
        }
    }

    /// <summary>
    ///  创建掉落道具实例
    /// </summary>
    public void CreateItemCptDrop(long itemId, int itemsNumber, string meta, Vector3 position, ItemDropStateEnum ItemCptDropState, Vector3 dropDirection)
    {
        CreateItemCptDrop(new ItemsBean(itemId, itemsNumber, meta), position, ItemCptDropState, dropDirection);
    }
    public void CreateItemCptDrop(long itemId, int itemsNumber, string meta, Vector3 position, ItemDropStateEnum ItemCptDropState)
    {
        CreateItemCptDrop(new ItemsBean(itemId, itemsNumber, meta), position, ItemCptDropState, Vector3.zero);
    }

    /// <summary>
    ///  创建掉落道具实例
    /// </summary>
    public void CreateItemCptDrop(BlockTypeEnum blockType, int itemsNumber, string meta, Vector3 position, ItemDropStateEnum ItemCptDropState, Vector3 dropDirection)
    {
        ItemsInfoBean itemsInfo = manager.GetItemsInfoByBlockType(blockType);
        CreateItemCptDrop(itemsInfo.id, itemsNumber, meta, position, ItemCptDropState, dropDirection);
    }
    public void CreateItemCptDrop(BlockTypeEnum blockType, int itemsNumber, string meta, Vector3 position, ItemDropStateEnum ItemCptDropState)
    {
        CreateItemCptDrop(blockType, itemsNumber, meta, position, ItemCptDropState, Vector3.zero);
    }

    /// <summary>
    ///  创建掉落道具实例
    /// </summary>
    public void CreateItemCptDrop(ItemsBean itemData, Vector3 position, ItemDropStateEnum ItemCptDropState, Vector3 dropDirection)
    {
        if (itemData.itemId == 0)
            return;
        manager.GetItemsObjById(-1, (objModel) =>
        {
            GameObject objCommon = Instantiate(gameObject, objModel);
            ItemCptDrop ItemCptDrop = objCommon.GetComponent<ItemCptDrop>();
            ItemCptDrop.SetData(itemData, position, dropDirection);
            ItemCptDrop.SetItemDropState(ItemCptDropState);
        });
    }

    /// <summary>
    /// 创建道具掉落
    /// </summary>
    /// <param name="oldBlock"></param>
    /// <param name="targetChunk"></param>
    /// <param name="targetPosition"></param>
    public void CreateItemCptDrop(Block targetBlock, Chunk targetChunk, Vector3Int targetWorldPosition)
    {
        BlockBean blockData = targetChunk.GetBlockData(targetWorldPosition - targetChunk.chunkData.positionForWorld);
        //如果是种植类物品
        if (targetBlock.blockInfo.GetBlockShape() == BlockShapeEnum.CropCross
            || targetBlock.blockInfo.GetBlockShape() == BlockShapeEnum.CropCrossOblique
            || targetBlock.blockInfo.GetBlockShape() == BlockShapeEnum.CropWell)
        {
            //首先判断生长周期

            //获取种植收货
            List<ItemsBean> listHarvest = targetBlock.GetDropItems(blockData);
            //创建掉落物
            CreateItemCptDropList(listHarvest, targetWorldPosition + Vector3.one * 0.5f, ItemDropStateEnum.DropPick);
        }
        else
        {
            //获取掉落道具
            List<ItemsBean> listDrop = targetBlock.GetDropItems(blockData);
            //如果没有掉落物，则默认掉落本体一个
            if (listDrop.IsNull())
            {
                //创建掉落物
                string blockMeta = blockData == null ? null : blockData.meta;
                CreateItemCptDrop(targetBlock.blockType, 1, blockMeta, targetWorldPosition + Vector3.one * 0.5f, ItemDropStateEnum.DropPick);
            }
            else
            {
                //创建掉落物
                CreateItemCptDropList(listDrop, targetWorldPosition + Vector3.one * 0.5f, ItemDropStateEnum.DropPick);
            }
        }
    }

    /// <summary>
    /// 通过ID设置道具图标
    /// </summary>
    /// <param name="image"></param>
    /// <param name="id"></param>
    public void SetItemsIconById(Image image, long id, Action<Sprite> complete = null)
    {
        manager.GetItemsIconById(id, (spIcon) =>
        {
            if (spIcon == null)
            {
                if (image == null)
                    return;
                IconHandler.Instance.GetUnKnowSprite((spIcon) =>
                {
                    image.sprite = spIcon;
                    complete?.Invoke(spIcon);
                });
            }
            else
            {
                if (image == null)
                    return;
                image.sprite = spIcon;
                complete?.Invoke(spIcon);
            }
        });
    }

    public void SetItemsIconById(SpriteRenderer image, long id, Action<Sprite> complete = null)
    {
        manager.GetItemsIconById(id, (spIcon) =>
        {
            if (spIcon == null)
            {
                if (image == null)
                    return;
                IconHandler.Instance.GetUnKnowSprite((spIcon) =>
                {
                    image.sprite = spIcon;
                    complete?.Invoke(spIcon);
                });
            }
            else
            {
                if (image == null)
                    return;
                image.sprite = spIcon;
                complete?.Invoke(spIcon);
            }
        });
    }



    /// <summary>
    /// 获取道具掉落
    /// </summary>
    /// <returns></returns>
    public List<ItemsBean> GetItemsDrop(string dropData)
    {
        List<ItemsBean> itemsData = new List<ItemsBean>();
        string[] itemListStr = dropData.SplitForArrayStr('|');
        for (int i = 0; i < itemListStr.Length; i++)
        {
            string[] itemDetailsListStr = itemListStr[i].SplitForArrayStr(',');
            long itemsId = long.Parse(itemDetailsListStr[0]);
            int itemsNumber = 1;
            float getRandomRate = 1;
            if (itemDetailsListStr.Length == 1)
            {

            }
            else if (itemDetailsListStr.Length == 2)
            {
                itemsNumber = int.Parse(itemDetailsListStr[1]);
            }
            else if (itemDetailsListStr.Length == 3)
            {
                itemsNumber = int.Parse(itemDetailsListStr[1]);
                getRandomRate = float.Parse(itemDetailsListStr[2]);
                if (UnityEngine.Random.Range(0f, 1f) > getRandomRate)
                {
                    //这里设置0，而不是直接不返回 是因为如果返回的list没有数据的话，会生成本体的掉落。设置成0则直接不掉落物品
                    itemsId = 0;
                }
            }
            itemsData.Add(new ItemsBean(itemsId, itemsNumber));
        }
        return itemsData;
    }
}
