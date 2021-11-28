using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemsHandler : BaseHandler<ItemsHandler, ItemsManager>
{

    /// <summary>
    /// 使用物品
    /// </summary>
    /// <param name="itemsData"></param>
    public void UseItem(ItemsBean itemsData)
    {
        Item item;
        if (itemsData == null || itemsData.itemId == 0)
        {
            //如果手上没有东西
            item = manager.GetRegisterItem(ItemsTypeEnum.Block);
            item.SetItemData(itemsData);
        }
        else
        {
            //如果手上有东西
            ItemsInfoBean itemsInfo = manager.GetItemsInfoById(itemsData.itemId);
            //获取对应得处理类
            item = manager.GetRegisterItem((ItemsTypeEnum)itemsInfo.items_type);
            //设置物品数据
            item.SetItemData(itemsData);
        }
        item.Use();
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
            item.SetItemData(itemsData);
        }
        else
        {
            //如果手上有东西
            ItemsInfoBean itemsInfo = manager.GetItemsInfoById(itemsData.itemId);
            //获取对应得处理类
            item = manager.GetRegisterItem((ItemsTypeEnum)itemsInfo.items_type);
            //设置物品数据
            item.SetItemData(itemsData);
        }
        item.UseTarget();
    }

    /// <summary>
    /// 创建掉落道具实例
    /// </summary>
    public void CreateItemDropList(List<ItemsBean> itemDatas, Vector3 position, ItemDropStateEnum itemDropState)
    {
        CreateItemDropList(itemDatas, position, itemDropState, Vector3.zero);
    }
    public void CreateItemDropList(List<ItemsBean> itemDatas, Vector3 position, ItemDropStateEnum itemDropState, Vector3 dropDirection)
    {
        for (int i = 0; i < itemDatas.Count; i++)
        {
            CreateItemDrop(itemDatas[i], position, itemDropState, dropDirection);
        }
    }

    /// <summary>
    ///  创建掉落道具实例
    /// </summary>
    public void CreateItemDrop(long itemId, int itemsNumber, Vector3 position, ItemDropStateEnum itemDropState, Vector3 dropDirection)
    {
        CreateItemDrop(new ItemsBean(itemId, itemsNumber), position, itemDropState, dropDirection);
    }
    public void CreateItemDrop(long itemId, int itemsNumber, Vector3 position, ItemDropStateEnum itemDropState)
    {
        CreateItemDrop(new ItemsBean(itemId, itemsNumber), position, itemDropState, Vector3.zero);
    }

    /// <summary>
    ///  创建掉落道具实例
    /// </summary>
    public void CreateItemDrop(BlockTypeEnum blockType, int itemsNumber, Vector3 position, ItemDropStateEnum itemDropState, Vector3 dropDirection)
    {
        ItemsInfoBean itemsInfo = manager.GetItemsInfoByBlockType(blockType);
        CreateItemDrop(itemsInfo.id, itemsNumber, position, itemDropState, dropDirection);
    }
    public void CreateItemDrop(BlockTypeEnum blockType, int itemsNumber, Vector3 position, ItemDropStateEnum itemDropState)
    {
        CreateItemDrop(blockType, itemsNumber, position, itemDropState, Vector3.zero);
    }

    /// <summary>
    ///  创建掉落道具实例
    /// </summary>
    public void CreateItemDrop(ItemsBean itemData, Vector3 position, ItemDropStateEnum itemDropState, Vector3 dropDirection)
    {
        manager.GetItemsObjById(-1, (objModel) =>
        {
            GameObject objCommon = Instantiate(gameObject, objModel);
            ItemDrop itemDrop = objCommon.GetComponent<ItemDrop>();
            itemDrop.SetData(itemData, position, dropDirection);
            itemDrop.SetItemDropState(itemDropState);
        });
    }
}
