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

    //�������
    public int dataIndex;

    //��Ϸʱ��
    public TimeBean timeForGame = new TimeBean();
    //����ʱ��
    public TimeBean timeForPlay = new TimeBean();
    //��������
    public CharacterBean characterData = new CharacterBean();

    //�����λ��
    public byte indexForShortcuts = 0;
    //���������
    public ItemsBean[] listShortcutsItems = new ItemsBean[10];
    //��������
    public ItemsBean[] listBackpack = new ItemsBean[10 * 5];

    /// <summary>
    /// ���ӵ���
    /// </summary>
    public int AddItems(long itemId, int itemNumber)
    {
        //���Ȳ�ѯ�����Ϳ�������Ƿ���ͬ���ĵ���
        //����������Ӧ���ߵ����� ֱ���õ��ߵ�����
        itemNumber = AddOldItems(listShortcutsItems,  itemId,  itemNumber);
        if (itemNumber <= 0) return itemNumber;
        itemNumber = AddOldItems(listBackpack,  itemId,  itemNumber);
        if (itemNumber <= 0) return itemNumber;

        //�����û�е�������� �������µ���������
        itemNumber = AddNewItems(listShortcutsItems, itemId, itemNumber);
        if (itemNumber <= 0) return itemNumber;
        itemNumber = AddNewItems(listBackpack, itemId, itemNumber);
        return itemNumber;
    }

    /// <summary>
    /// ���������е�itemdata����������
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
        return GetItemsFromBackpack((x - 1) + (y - 1) * 10);
    }
}