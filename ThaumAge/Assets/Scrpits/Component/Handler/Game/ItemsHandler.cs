﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ItemsHandler : BaseHandler<ItemsHandler, ItemsManager>
{

    /// <summary>
    /// 使用物品
    /// </summary>
    /// <param name="itemsData"></param>
    public void UseItem(GameObject user, ItemsBean itemsData)
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
            item = manager.GetRegisterItem((ItemsTypeEnum)itemsInfo.items_type);
        }
        item.Use(user, itemsData);
        item.UseForAnim(user, itemsData);
    }

    /// <summary>
    /// 使用物品目标
    /// </summary>
    public void UseItemTarget(ItemsBean itemsData)
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
            item = manager.GetRegisterItem((ItemsTypeEnum)itemsInfo.items_type);
        }
        item.UseTarget(itemsData);
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
    public void CreateItemCptDrop(long itemId, int itemsNumber, Vector3 position, ItemDropStateEnum ItemCptDropState, Vector3 dropDirection)
    {
        CreateItemCptDrop(new ItemsBean(itemId, itemsNumber), position, ItemCptDropState, dropDirection);
    }
    public void CreateItemCptDrop(long itemId, int itemsNumber, Vector3 position, ItemDropStateEnum ItemCptDropState)
    {
        CreateItemCptDrop(new ItemsBean(itemId, itemsNumber), position, ItemCptDropState, Vector3.zero);
    }

    /// <summary>
    ///  创建掉落道具实例
    /// </summary>
    public void CreateItemCptDrop(BlockTypeEnum blockType, int itemsNumber, Vector3 position, ItemDropStateEnum ItemCptDropState, Vector3 dropDirection)
    {
        ItemsInfoBean itemsInfo = manager.GetItemsInfoByBlockType(blockType);
        CreateItemCptDrop(itemsInfo.id, itemsNumber, position, ItemCptDropState, dropDirection);
    }
    public void CreateItemCptDrop(BlockTypeEnum blockType, int itemsNumber, Vector3 position, ItemDropStateEnum ItemCptDropState)
    {
        CreateItemCptDrop(blockType, itemsNumber, position, ItemCptDropState, Vector3.zero);
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
                IconHandler.Instance.GetUnKnowSprite((spIcon) =>
                {
                    image.sprite = spIcon;
                    complete?.Invoke(spIcon);
                });
            }
            else
            {
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
                IconHandler.Instance.GetUnKnowSprite((spIcon) =>
                {
                    image.sprite = spIcon;
                    complete?.Invoke(spIcon);
                });
            }
            else
            {
                image.sprite = spIcon;
                complete?.Invoke(spIcon);
            }
        });
    }
}
