/*
* FileName: UserData 
* Author: AppleCoffee 
* CreateTime: 2021-03-24-14:49:52 
*/

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UserDataController : BaseMVCController<UserDataModel, IUserDataView>
{

    public UserDataController(BaseMonoBehaviour content, IUserDataView view) : base(content, view)
    {

    }

    public override void InitData()
    {

    }

    /// <summary>
    /// 获取数据
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public UserDataBean GetUserDataData(string usreId,Action<UserDataBean> action)
    {
        UserDataBean data = GetModel().GetUserDataData(usreId);
        if (data == null) {
            GetView().GetUserDataFail("没有数据",null);
            return null;
        }
        GetView().GetUserDataSuccess(data,action);
        return data;
    }

    /// <summary>
    /// 获取所有数据
    /// </summary>
    /// <param name="action"></param>
    public void GetAllUserDataData(Action<List<UserDataBean>> action)
    {
        List<UserDataBean> listData = GetModel().GetAllUserDataData();
        if (listData.IsNull())
        {
            action?.Invoke(new List<UserDataBean>());
            GetView().GetUserDataFail("没有数据", null);
        }
        else
        {
            GetView().GetUserDataSuccess(listData, action);
        }
    }

    /// <summary>
    /// 保存用户数据
    /// </summary>
    /// <param name="userData"></param>
    public void SetUserData(UserDataBean userData)
    {
        GetModel().SetUserDataData(userData);
    }

    /// <summary>
    /// 删除游戏数据
    /// </summary>
    /// <param name="userId"></param>
    public void RemoveUserData(string userId)
    {
        GetModel().RemoveUserData(userId);
    }
} 