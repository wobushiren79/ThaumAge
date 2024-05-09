/*
* FileName: UserData 
* Author: AppleCoffee 
* CreateTime: 2021-03-24-14:49:52 
*/

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;

public class UserDataService : BaseDataStorage
{
    protected readonly string saveFileName;

    public UserDataService()
    {
        saveFileName = "UserData";
    }
    /// <summary>
    /// 查询游戏配置数据
    /// </summary>
    /// <returns></returns>
    public UserDataBean QueryDataByUserId(string userId)
    {
        return BaseLoadData<UserDataBean>(userId + "/Base");
    }
    
    /// <summary>
    /// 加载所有数据
    /// </summary>
    /// <returns></returns>
    public List<UserDataBean> QueryAllData()
    {
        List<UserDataBean> listData = new List<UserDataBean>();
        string[] dirs = Directory.GetDirectories(dataStoragePath);
        for (int i = 0; i < dirs.Length; i++)
        {
            string itemDir = dirs[i];
            string[] files = Directory.GetFiles(itemDir);
            for (int f = 0; f < files.Length; f++)
            {
                string itemFile = files[f];
                if (itemFile.Replace(itemDir, "").Contains("Base"))
                {
                    UserDataBean userData = BaseLoadDataByPath<UserDataBean>(itemFile);
                    if (userData != null)
                        listData.Add(userData);
                    break;
                }
            }
        }
        return listData;
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="gameConfig"></param>
    public void UpdateDataByUserId(UserDataBean gameData)
    {
        FileUtil.CreateDirectory(dataStoragePath + "/" + gameData.userId);
        BaseSaveData(gameData.userId + "/Base", gameData);
    }

    /// <summary>
    /// 删除用户数据
    /// </summary>
    public void DeleteDataByUserId(string userId)
    {
        BaseDeleteFolder(userId);
    }
}