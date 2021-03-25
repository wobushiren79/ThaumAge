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
    public WorldDataBean QueryData(string userId, WorldTypeEnum worldType, Vector3Int position)
    {
        string worldName = saveFileName + "_" + EnumUtil.GetEnumName(worldType);
        string fileName = "w_" + position.x + "_" + position.z;
        return BaseLoadData(userId + "/" + worldName + "/" + fileName);
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="gameConfig"></param>
    public void UpdateData(WorldDataBean data)
    {
        WorldTypeEnum worldType = data.GetWorkType();
        string worldName = saveFileName + "_" + EnumUtil.GetEnumName(worldType);
        string fileName = "w_" + data.chunkData.position.x + "_" + data.chunkData.position.z;
        FileUtil.CreateDirectory(dataStoragePath + "/" + data.userId);
        FileUtil.CreateDirectory(dataStoragePath + "/" + data.userId + "/" + worldName);
        if (data.userId != null)
            BaseSaveData(data.userId + "/" + worldName + "/" + fileName, data);
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    public void DeleteData(string userId, WorldTypeEnum worldType, Vector3Int position)
    {
        string worldName = saveFileName + "_" + EnumUtil.GetEnumName(worldType);
        string fileName = "w_" + position.x + "_" + position.z;
        BaseDeleteFile(userId + "/" + worldName + "/" + fileName);
    }
}