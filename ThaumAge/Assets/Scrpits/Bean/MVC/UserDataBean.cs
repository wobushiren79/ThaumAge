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

    //��Ϸʱ��
    public TimeBean timeForGame = new TimeBean();
    //����ʱ��
    public TimeBean timeForPlay = new TimeBean();
}