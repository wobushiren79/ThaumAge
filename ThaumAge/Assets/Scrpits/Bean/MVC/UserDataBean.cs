/*
* FileName: UserData 
* Author: AppleCoffee 
* CreateTime: 2021-03-24-14:49:52 
*/

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class UserDataBean
{
    public string userId;

    //数据序号
    public int dataIndex;

    //游戏时间
    public TimeBean timeForGame = new TimeBean();
    //游玩时间
    public TimeBean timeForPlay = new TimeBean();
    //主角数据
    public CharacterBean characterData = new CharacterBean();

    //快捷栏位置
    public byte indexForShortcuts = 0;
    //快捷栏道具
    public ItemsBean[] listShortcutsItems = new ItemsBean[10];
    //背包道具
    public ItemsBean[] listBackpack = new ItemsBean[10 * 5];

    /// <summary>
    /// 增加道具
    /// </summary>
    public int AddItems(long itemId, int itemNumber)
    {
        //首先查询背包和快捷栏里是否有同样的道具
        //依次增加相应道具的数量 直到该道具的上限
        itemNumber = AddOldItems(listShortcutsItems,  itemId,  itemNumber);
        if (itemNumber <= 0) return itemNumber;
        itemNumber = AddOldItems(listBackpack,  itemId,  itemNumber);
        if (itemNumber <= 0) return itemNumber;

        //如果还没有叠加完道具 曾创建新的用以增加
        itemNumber = AddNewItems(listShortcutsItems, itemId, itemNumber);
        if (itemNumber <= 0) return itemNumber;
        itemNumber = AddNewItems(listBackpack, itemId, itemNumber);
        return itemNumber;
    }

    /// <summary>
    /// 在容器里有的itemdata中增加数据
    /// </summary>
    /// <param name="arrayContainer"></param>
    /// <param name="itemId"></param>
    /// <param name="itemNumber"></param>
    /// <returns></returns>
    public int AddOldItems(ItemsBean[] arrayContainer, long itemId, int itemNumber)
    {
        ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemId);
        for (int i = 0; i < arrayContainer.Length; i++)
        {
            ItemsBean itemData = arrayContainer[i];
            if (itemData != null && itemData.itemId == itemId)
            {
                if (itemData.number < itemsInfo.max_number)
                {
                    int subNumber = itemsInfo.max_number - itemData.number;
                    //如果增加的数量在该道具的上限之内
                    if (subNumber >= itemNumber)
                    {
                        itemData.number += itemNumber;
                        itemNumber = 0;
                        return itemNumber;
                    }
                    //如果增加的数量在该道具的上限之外
                    else
                    {
                        itemData.number = itemsInfo.max_number;
                        itemNumber -= subNumber;
                    }
                }
            }
        }
        return itemNumber;
    }

    /// <summary>
    /// 在容器中增加新的itemdata
    /// </summary>
    /// <param name="arrayContainer"></param>
    /// <param name="itemId"></param>
    /// <param name="itemNumber"></param>
    /// <returns></returns>
    public int AddNewItems(ItemsBean[] arrayContainer, long itemId, int itemNumber)
    {
        ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemId);
        for (int i = 0; i < arrayContainer.Length; i++)
        {
            ItemsBean itemData = arrayContainer[i];
            if (itemData == null||itemData.itemId == 0)
            {
                ItemsBean newItemData = new ItemsBean(itemId);
                listShortcutsItems[i] = newItemData;
                int subNumber = itemsInfo.max_number;
                //如果增加的数量在该道具的上限之内
                if (subNumber >= itemNumber)
                {
                    newItemData.number += itemNumber;
                    itemNumber = 0;
                    return itemNumber;
                }
                //如果增加的数量在该道具的上限之外
                else
                {
                    newItemData.number = itemsInfo.max_number;
                    itemNumber -= subNumber;
                }
            }
        }
        return itemNumber;
    }

    /// <summary>
    /// 获取快捷栏道具
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public ItemsBean GetItemsFromShortcut(int index)
    {
        ItemsBean itemsData = listShortcutsItems[index];
        if (itemsData == null)
        {
            itemsData = new ItemsBean();
            listShortcutsItems[index] = itemsData;
        }
        return itemsData;
    }

    public ItemsBean GetItemsFromShortcut()
    {
        return GetItemsFromShortcut(indexForShortcuts);
    }

    /// <summary>
    /// 获取背包道具
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public ItemsBean GetItemsFromBackpack(int index)
    {
        ItemsBean itemsData = listBackpack[index];
        if (itemsData == null)
        {
            itemsData = new ItemsBean();
            listBackpack[index] = itemsData;
        }
        return itemsData;
    }

    /// <summary>
    /// 获取背包道具
    /// </summary>
    /// <param name="x">横排</param>
    /// <param name="y">竖排</param>
    /// <returns></returns>
    public ItemsBean GetItemsFromBackpack(int x, int y)
    {
        return GetItemsFromBackpack((x - 1) + (y - 1) * 10);
    }
}