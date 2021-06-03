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

    //游戏时间
    public TimeBean timeForGame = new TimeBean();
    //游玩时间
    public TimeBean timeForPlay = new TimeBean();

    //快捷栏位置
    public byte indexForShortcuts = 0;
    //快捷栏道具
    public ItemsBean[] listShortcutsItems = new ItemsBean[10];
    //背包道具
    public ItemsBean[] listBackpack = new ItemsBean[10 * 5];

    /// <summary>
    /// 获取快捷栏道具
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public ItemsBean GetItemsFromShortcut(int index)
    {
        return listShortcutsItems[index];
    }

    /// <summary>
    /// 获取背包道具
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public ItemsBean GetItemsFromBackpack(int index)
    {
        return listBackpack[index];
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