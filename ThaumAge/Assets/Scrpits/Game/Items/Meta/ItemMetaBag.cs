using UnityEditor;
using UnityEngine;
using System;

[Serializable]
public class ItemMetaBag : BlockMetaChest
{
    public ItemMetaBag() : base()
    {

    }

    public ItemMetaBag(int number) : base(number)
    {

    }

    public ItemMetaBag(int number, ItemsBean[] listData) : base(number, listData)
    {


    }

    /// <summary>
    /// 检测背包是否满了
    /// </summary>
    public bool CheckIsFull()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].itemId == 0)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 检测背包是否是空的
    /// </summary>
    /// <returns></returns>
    public bool CheckIsEmpty()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].itemId != 0)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 获取道具
    /// </summary>
    /// <returns></returns>
    public ItemsBean GetItem()
    {
        for (int i = 0; i < items.Length; i++)
        {
            var itemData = items[i];
            if (itemData.itemId != 0 && itemData.number != 0)
            {
                return itemData;
            }
        }
        return null;
    }

    /// <summary>
    /// 增加道具到背包
    /// </summary>
    /// <param name="itemsData"></param>
    public bool AddItemForBag(ItemsBean itemsData)
    {
        for (int i = 0; i < items.Length; i++)
        {
            ItemsBean batItemData = items[i];
            if (batItemData.itemId == 0 || batItemData.number == 0)
            {
                items[i] = itemsData;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 删除道具
    /// </summary>
    /// <returns></returns>
    public bool RemoveItem(ItemsBean targetItemData)
    {
        for (int i = 0; i < items.Length; i++)
        {
            var itemData = items[i];
            if (itemData == targetItemData)
            {
                itemData.ClearData();
                return true;
            }
        }
        return false;
    }
}