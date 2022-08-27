/*
* FileName: UserData 
* Author: AppleCoffee 
* CreateTime: 2021-03-24-14:49:52 
*/

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class UserDataBean
{
    public string userId;
    //种子
    public int seed;
    //数据序号
    public int dataIndex;

    //玩家装备数据
    public UserEquipBean userEquip = new UserEquipBean();
    //玩家位置数据
    public UserPositionBean userPosition = new UserPositionBean();
    //玩家离开位置数据
    public UserPositionBean userExitPosition = new UserPositionBean();

    //玩家成就数据
    public UserAchievementBean userAchievement = new UserAchievementBean();
    //玩家设置数据
    public UserSettingBean userSetting = new UserSettingBean();

    //游戏时间
    public TimeBean timeForGame = new TimeBean();
    //游玩时间
    public TimeBean timeForPlay = new TimeBean();
    //主角数据
    public CharacterBean characterData = new CharacterBean();

    //快捷栏位置
    public int indexForShortcuts = 0;
    //快捷栏道具
    public ItemsBean[] listShortcutsItems = new ItemsBean[10];
    //背包道具
    public ItemsBean[] listBackpack = new ItemsBean[7 * 7];

    /// <summary>
    /// 增加道具
    /// </summary>
    /// <param name="itemData"></param>
    /// <param name="itemNumber"></param>
    public int AddItems(ItemsBean itemData ,int itemNumber)
    {
        itemData.number += itemNumber;
        ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemData.itemId);
        if (itemData.number <= 0)
        {
            itemData.itemId = 0;
            itemData.number = 0;
            itemData.meta = null;
        }
        else if (itemData.number > itemsInfo.max_number)
        {
            int moreNumber = itemData.number - itemsInfo.max_number;
            itemData.number = itemsInfo.max_number;
            return AddItems(itemData.itemId, moreNumber, itemData.meta);
        }
        return 0;
    }

    /// <summary>
    /// 增加道具
    /// </summary>
    public int AddItems(long itemId, int itemNumber,string meta)
    {
        //首先查询背包和快捷栏里是否有同样的道具                     
        //依次增加相应道具的数量 直到该道具的上限
        itemNumber = AddOldItems(listShortcutsItems, itemId, itemNumber, meta);
        if (itemNumber <= 0) return itemNumber;
        itemNumber = AddOldItems(listBackpack, itemId, itemNumber, meta);
        if (itemNumber <= 0) return itemNumber;

        //如果还没有叠加完道具 曾创建新的用以增加
        itemNumber = AddNewItems(listShortcutsItems, itemId, itemNumber, meta);
        if (itemNumber <= 0) return itemNumber;
        itemNumber = AddNewItems(listBackpack, itemId, itemNumber, meta);
        return itemNumber;
    }

    /// <summary>
    /// 在容器里有的itemdata中增加数据
    /// </summary>
    /// <param name="arrayContainer"></param>
    /// <param name="itemId"></param>
    /// <param name="itemNumber"></param>
    /// <returns></returns>
    protected int AddOldItems(ItemsBean[] arrayContainer, long itemId, int itemNumber,string meta)
    {
        ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemId);
        for (int i = 0; i < arrayContainer.Length; i++)
        {
            ItemsBean itemData = arrayContainer[i];
            if (itemData != null && itemData.itemId == itemId)
            {
                itemData.meta = meta;
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
    protected int AddNewItems(ItemsBean[] arrayContainer, long itemId, int itemNumber,string meta)
    {
        ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemId);
        for (int i = 0; i < arrayContainer.Length; i++)
        {
            ItemsBean itemData = arrayContainer[i];
            if (itemData == null || itemData.itemId == 0)
            {
                ItemsBean newItemData = new ItemsBean(itemId);
                arrayContainer[i] = newItemData;
                int subNumber = itemsInfo.max_number;
                newItemData.meta = meta;
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
    /// 改变快捷栏
    /// </summary>
    /// <param name="indexForShortcuts"></param>
    public int SetShortcuts(int indexForShortcuts)
    {       
        //如果没有改变 则不处理
        if (this.indexForShortcuts == indexForShortcuts)
            return indexForShortcuts;
        this.indexForShortcuts = indexForShortcuts;
        if (this.indexForShortcuts > 9)
        {
            this.indexForShortcuts = 0;
        }
        else if (this.indexForShortcuts < 0)
        {
            this.indexForShortcuts = 9;
        }
        return this.indexForShortcuts;
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
        return GetItemsFromBackpack((x - 1) + (y - 1) * 7);
    }

    /// <summary>
    /// 获取所有的道具 背包和快捷栏
    /// </summary>
    /// <returns></returns>
    public ItemsBean[] GetAllItems()
    {
        return listShortcutsItems.Concat(listBackpack).ToArray();
    }

    /// <summary>
    /// 是否有足够数量的指定道具
    /// </summary>
    /// <param name="itemsId"></param>
    /// <param name="itemsNum"></param>
    /// <returns></returns>
    public bool HasEnoughItem(long itemsId, long itemsNum)
    {
        ItemsBean[] allItems = GetAllItems();
        int totalNumber = 0;
        for (int i = 0; i < allItems.Length; i++)
        {
            ItemsBean itemData = allItems[i];
            if (itemData == null || itemData.itemId == 0)
                continue;
            if (itemData.itemId == itemsId)
                totalNumber += itemData.number;
        }
        if (totalNumber >= itemsNum)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 移除道具
    /// </summary>
    /// <param name="itemsId"></param>
    /// <param name="itemsNum"></param>
    public int RemoveItem(long itemsId, int itemsNum)
    {
        ItemsBean[] allItems = GetAllItems();
        for (int i = 0; i < allItems.Length; i++)
        {
            ItemsBean itemData = allItems[i];
            if (itemData == null || itemData.itemId == 0)
                continue;
            if (itemData.itemId == itemsId)
            {
                if (itemData.number > itemsNum)
                {
                    itemData.number -= itemsNum;
                    itemsNum = 0;
                }
                else
                {
                    itemsNum -= itemData.number;
                    itemData.number = 0;
                    itemData.meta = null;
                    itemData.itemId = 0;
                }
                if (itemsNum <= 0)
                {
                    //如果都扣除完了
                    break;
                }
            }
        }
        return itemsNum;
    }
}