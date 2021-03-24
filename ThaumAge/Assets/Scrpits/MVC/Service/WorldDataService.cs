/*
* FileName: WorldData 
* Author: AppleCoffee 
* CreateTime: 2021-03-24-13:49:11 
*/

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class WorldDataService : BaseDataStorage<WorldDataBean>
{
    protected readonly string saveFileName = "WorldData";

    public WorldDataService()
    {
       
    }

    /// <summary>
    /// 查询数据
    /// </summary>
    /// <returns></returns>
    public WorldDataBean QueryData(string userId, WorldTypeEnum worldType)
    {
        string filePath = saveFileName + "_" + EnumUtil.GetEnumName(worldType);
        return BaseLoadData(userId + "/" + filePath);
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="gameConfig"></param>
    public void UpdateData(WorldDataBean data)
    {
        WorldTypeEnum worldType = data.GetWorkType();
        string filePath = saveFileName + "_" + EnumUtil.GetEnumName(worldType);
        FileUtil.CreateDirectory(dataStoragePath + "/" + data.userId);
        BaseSaveData(data.userId + "/" + filePath, data);
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    public void DeleteData(string userId, WorldTypeEnum worldType)
    {
        string filePath = saveFileName + "_" + EnumUtil.GetEnumName(worldType);
        BaseDeleteFile(userId + "/" + filePath);
    }
}