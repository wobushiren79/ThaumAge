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
    //����
    public int seed;
    //�������
    public int dataIndex;

    //���װ������
    public UserEquipBean userEquip = new UserEquipBean();
    //���λ������
    public UserPositionBean userPosition = new UserPositionBean();
    //����뿪λ������
    public UserPositionBean userExitPosition = new UserPositionBean();

    //��ҳɾ�����
    public UserAchievementBean userAchievement = new UserAchievementBean();
    //�����������
    public UserSettingBean userSetting = new UserSettingBean();

    //��Ϸʱ��
    public TimeBean timeForGame = new TimeBean();
    //����ʱ��
    public TimeBean timeForPlay = new TimeBean();
    //��������
    public CharacterBean characterData = new CharacterBean();

    //�����λ��
    public int indexForShortcuts = 0;
    //���������
    public ItemsBean[] listShortcutsItems = new ItemsBean[10];
    //��������
    public ItemsBean[] listBackpack = new ItemsBean[7 * 7];

    /// <summary>
    /// ���ӵ���
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
    /// ���ӵ���
    /// </summary>
    public int AddItems(long itemId, int itemNumber,string meta)
    {
        //���Ȳ�ѯ�����Ϳ�������Ƿ���ͬ���ĵ���                     
        //����������Ӧ���ߵ����� ֱ���õ��ߵ�����
        itemNumber = AddOldItems(listShortcutsItems, itemId, itemNumber, meta);
        if (itemNumber <= 0) return itemNumber;
        itemNumber = AddOldItems(listBackpack, itemId, itemNumber, meta);
        if (itemNumber <= 0) return itemNumber;

        //�����û�е�������� �������µ���������
        itemNumber = AddNewItems(listShortcutsItems, itemId, itemNumber, meta);
        if (itemNumber <= 0) return itemNumber;
        itemNumber = AddNewItems(listBackpack, itemId, itemNumber, meta);
        return itemNumber;
    }

    /// <summary>
    /// ���������е�itemdata����������
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
                    //������ӵ������ڸõ��ߵ�����֮��
                    if (subNumber >= itemNumber)
                    {
                        itemData.number += itemNumber;
                        itemNumber = 0;
                        return itemNumber;
                    }
                    //������ӵ������ڸõ��ߵ�����֮��
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
    /// �������������µ�itemdata
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
                //������ӵ������ڸõ��ߵ�����֮��
                if (subNumber >= itemNumber)
                {
                    newItemData.number += itemNumber;
                    itemNumber = 0;
                    return itemNumber;
                }
                //������ӵ������ڸõ��ߵ�����֮��
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
    /// �ı�����
    /// </summary>
    /// <param name="indexForShortcuts"></param>
    public int SetShortcuts(int indexForShortcuts)
    {       
        //���û�иı� �򲻴���
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
    /// ��ȡ���������
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
    /// ��ȡ��������
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
    /// ��ȡ��������
    /// </summary>
    /// <param name="x">����</param>
    /// <param name="y">����</param>
    /// <returns></returns>
    public ItemsBean GetItemsFromBackpack(int x, int y)
    {
        return GetItemsFromBackpack((x - 1) + (y - 1) * 7);
    }

    /// <summary>
    /// ��ȡ���еĵ��� �����Ϳ����
    /// </summary>
    /// <returns></returns>
    public ItemsBean[] GetAllItems()
    {
        return listShortcutsItems.Concat(listBackpack).ToArray();
    }

    /// <summary>
    /// �Ƿ����㹻������ָ������
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
    /// �Ƴ�����
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
                    //������۳�����
                    break;
                }
            }
        }
        return itemsNum;
    }
}