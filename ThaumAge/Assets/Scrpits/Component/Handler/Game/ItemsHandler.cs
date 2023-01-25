using System;
using System.Collections;
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
            item = manager.GetRegisterItem(itemsData.itemId, ItemsTypeEnum.Empty);
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
            item.UseForAnim(user, itemsData, useType);
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
    /// 创建道具发射
    /// </summary>
    public void CreateItemLaunch(ItemLaunchBean itemLaunchData, Action<ItemCptLaunch> callBackForComplete = null)
    {
        if (itemLaunchData.itemId == 0)
            return;
        manager.GetItemsLaunchObj((objModel) =>
        {
            GameObject objCommon = Instantiate(gameObject, objModel);
            ItemCptLaunch itemLaunch = objCommon.GetComponent<ItemCptLaunch>();
            itemLaunch.SetData(itemLaunchData);
            callBackForComplete?.Invoke(itemLaunch);
        });
    }

    /// <summary>
    /// 创建掉落道具实例
    /// </summary>
    public void CreateItemCptDropList(List<ItemsBean> itemDatas, ItemDropStateEnum itemDropState, Vector3 dropPosition)
    {
        StartCoroutine(CreateItemCptDropList(itemDatas, itemDropState, dropPosition, Vector3.zero));
    }
    protected IEnumerator CreateItemCptDropList(List<ItemsBean> itemDatas, ItemDropStateEnum itemCptDropState, Vector3 dropPosition, Vector3 dropDirection)
    {
        for (int i = 0; i < itemDatas.Count; i++)
        {
            ItemsBean itemData = itemDatas[i];
            if (itemData.itemId == 0)
                continue;
            ItemDropBean itemDropData = new ItemDropBean(itemDatas[i], itemCptDropState, dropPosition, dropDirection);
            CreateItemCptDrop(itemDropData);
            yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary>
    /// 创建道具掉落
    /// </summary>
    public void CreateItemCptDrop(ItemDropBean itemDropData, Action<ItemCptDrop> callBackForComplete = null)
    {
        if (itemDropData.itemData == null || itemDropData.itemData.itemId == 0)
            return;
        manager.GetItemsDropObj((objModel) =>
        {
            GameObject objCommon = Instantiate(gameObject, objModel);
            ItemCptDrop ItemCptDrop = objCommon.GetComponent<ItemCptDrop>();
            ItemCptDrop.SetData(itemDropData);
            callBackForComplete?.Invoke(ItemCptDrop);
        });

        //播放道具掉落音效
        AudioHandler.Instance.PlaySound(503, itemDropData.dropPosition);
    }

    /// <summary>
    /// 创建道具掉落（用于方块）
    /// </summary>
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
            CreateItemCptDropList(listHarvest, ItemDropStateEnum.DropPick, targetWorldPosition + Vector3.one * 0.5f);

            targetBlock.CreateDropItems(blockData);
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
                ItemDropBean itemDropData = new ItemDropBean(targetBlock.blockType, targetWorldPosition + Vector3.one * 0.5f, 1, blockMeta, ItemDropStateEnum.DropPick);
                CreateItemCptDrop(itemDropData);
            }
            else
            {
                //创建掉落物
                CreateItemCptDropList(listDrop, ItemDropStateEnum.DropPick, targetWorldPosition + Vector3.one * 0.5f);
            }
            targetBlock.CreateDropItems(blockData);
        }
    }

    /// <summary>
    /// 通过ID设置道具图标
    /// </summary>
    /// <param name="image"></param>
    /// <param name="id"></param>
    public void SetItemsIconById(Image image, long id, ItemsBean itemsData = null)
    {
        CptUtil.RemoveChildsByActive(image.transform);
        Item targetItem = manager.GetRegisterItem(id);
        ItemsInfoBean itemsInfo = manager.GetItemsInfoById(id);
        targetItem.SetItemIcon(itemsData, itemsInfo, image);
    }

    /// <summary>
    /// 通过ID设置道具的名字
    /// </summary>
    /// <param name="tvName"></param>
    /// <param name="id"></param>
    /// <param name="itemsData"></param>
    public void SetItemsNameById(Text tvName, long id, ItemsBean itemsData = null)
    {
        Item targetItem = manager.GetRegisterItem(id);
        ItemsInfoBean itemsInfo = manager.GetItemsInfoById(id);
        targetItem.SetItemName(tvName, itemsData, itemsInfo);
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
