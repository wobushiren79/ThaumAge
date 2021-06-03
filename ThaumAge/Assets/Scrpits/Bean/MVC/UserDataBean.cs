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
public class UserDataBean : BaseBean
{
    public string userId;

    //��Ϸʱ��
    public TimeBean timeForGame = new TimeBean();
    //����ʱ��
    public TimeBean timeForPlay = new TimeBean();

    //�����λ��
    public byte indexForShortcuts = 0;
    //���������
    public ItemsBean[] listShortcutsItems = new ItemsBean[10];
    //��������
    public ItemsBean[] listBackpack = new ItemsBean[10 * 5];

    /// <summary>
    /// ��ȡ���������
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public ItemsBean GetItemsFromShortcut(int index)
    {
        return listShortcutsItems[index];
    }

    /// <summary>
    /// ��ȡ��������
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public ItemsBean GetItemsFromBackpack(int index)
    {
        return listBackpack[index];
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