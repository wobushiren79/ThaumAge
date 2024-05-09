/*
* FileName: BiomeSave 
* Author: AppleCoffee 
* CreateTime: 2022-11-01-14:57:35 
*/

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class BiomeSaveService : BaseDataStorage
{
    protected readonly string saveFileName;

    public BiomeSaveService()
    {
        saveFileName = "WorldData";
    }

    /// <summary>
    /// 查询所有数据
    /// </summary>
    /// <returns></returns>
    public List<BiomeSaveBean> QueryAllData()
    {
        return null; 
    }

    /// <summary>
    /// 查询游戏配置数据
    /// </summary>
    /// <returns></returns>
    public BiomeSaveBean QueryData(string userId, WorldTypeEnum worldType)
    {
        string worldName = saveFileName + "_" + EnumExtension.GetEnumName(worldType);
        string fileName = "BiomeData";
        return BaseLoadData<BiomeSaveBean>(userId + "/" + worldName + "/" + fileName);
    }
        
    /// <summary>
    /// 通过ID查询数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<BiomeSaveBean> QueryDataById(long id)
    {
        return null;
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="data"></param>
    public void UpdateData(BiomeSaveBean data)
    {
        WorldTypeEnum worldType = data.GetWorldType();
        string worldName = saveFileName + "_" + EnumExtension.GetEnumName(worldType);
        string fileName = "BiomeData";
        FileUtil.CreateDirectory(dataStoragePath + "/" + data.userId);
        FileUtil.CreateDirectory(dataStoragePath + "/" + data.userId + "/" + worldName);
        if (data.userId != null)
            BaseSaveData(data.userId + "/" + worldName + "/" + fileName, data);
    }
}