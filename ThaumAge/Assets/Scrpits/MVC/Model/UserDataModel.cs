/*
* FileName: UserData 
* Author: AppleCoffee 
* CreateTime: 2021-03-24-14:49:52 
*/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class UserDataModel : BaseMVCModel
{
    protected UserDataService serviceUserData;

    public override void InitData()
    {
        serviceUserData = new UserDataService();
    }

    /// <summary>
    /// 获取所有数据
    /// </summary>
    /// <returns></returns>
    public List<UserDataBean> GetAllUserDataData()
    {
        List<UserDataBean> listData = serviceUserData.QueryAllData();
        return listData;
    }

    /// <summary>
    /// 获取游戏数据
    /// </summary>
    /// <returns></returns>
    public UserDataBean GetUserDataData(string userId)
    {
        UserDataBean data = serviceUserData.QueryDataByUserId(userId);
        if (data == null)
            data = new UserDataBean(userId);
        return data;
    }

    /// <summary>
    /// 保存游戏数据
    /// </summary>
    /// <param name="data"></param>
    public void SetUserDataData(UserDataBean data)
    {
        serviceUserData.UpdateDataByUserId(data);
    }

    /// <summary>
    /// 移除游戏数据
    /// </summary>
    /// <param name="userId"></param>
    public void RemoveUserData(string userId)
    {
        serviceUserData.DeleteDataByUserId(userId);
    }

}