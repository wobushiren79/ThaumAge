/*
* FileName: UserData 
* Author: AppleCoffee 
* CreateTime: 2021-03-24-14:49:52 
*/

using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class UserDataBean : BaseBean
{
    public string userId;

    //游戏时间
    public TimeBean timeForGame = new TimeBean();
    //游玩时间
    public TimeBean timeForPlay = new TimeBean();
}