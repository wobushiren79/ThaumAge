/*
* FileName: WorldData 
* Author: AppleCoffee 
* CreateTime: 2021-03-24-13:49:11 
*/

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

public class ChunkSaveService : BaseDataStorage
{
    protected readonly string saveFileName = "WorldData";
    protected readonly string saveFileNameForCreature = "WorldDataCreature";
    public ChunkSaveService()
    {

    }

    /// <summary>
    /// 查询数据
    /// </summary>
    /// <returns></returns>
    public ChunkSaveBean QueryData(string userId, WorldTypeEnum worldType, Vector3Int position)
    {
        string worldName = $"{saveFileName}_{EnumExtension.GetEnumName(worldType)}";
        string fileName = $"w_{position.x}_{position.z}";
        return BaseLoadData<ChunkSaveBean>($"{userId}/{worldName}/{fileName}");
    }

    /// <summary>
    /// 查询数据
    /// </summary>
    /// <returns></returns>
    public ChunkSaveCreatureBean QueryDataForCreature(string userId, WorldTypeEnum worldType, Vector3Int position)
    {
        string worldName = $"{saveFileNameForCreature}_{EnumExtension.GetEnumName(worldType)}";
        string fileName = $"w_{position.x}_{position.z}_creature";
        return BaseLoadData<ChunkSaveCreatureBean>($"{userId}/{worldName}/{fileName}",false);
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="gameConfig"></param>
    public void UpdateData(ChunkSaveBean data)
    {
        WorldTypeEnum worldType = data.GetWorldType();
        string worldName = $"{saveFileName}_{EnumExtension.GetEnumName(worldType)}";
        string fileName = $"w_{data.position.x}_{data.position.z}";
        FileUtil.CreateDirectory($"{dataStoragePath}/{data.userId}");
        FileUtil.CreateDirectory($"{dataStoragePath}/{data.userId}/{worldName}");
        if (data.userId != null)
            BaseSaveData($"{data.userId}/{worldName}/{fileName}", data);
    }
    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="gameConfig"></param>
    public void UpdateDataForCreature(ChunkSaveCreatureBean data)
    {
        WorldTypeEnum worldType = data.GetWorldType();
        string worldName = $"{saveFileNameForCreature}_{EnumExtension.GetEnumName(worldType)}";
        string fileName = $"w_{data.position.x}_{data.position.z}_creature";
        FileUtil.CreateDirectory($"{dataStoragePath}/{data.userId}");
        FileUtil.CreateDirectory($"{dataStoragePath}/{data.userId}/{worldName}");
        if (data.userId != null)
            BaseSaveData($"{data.userId}/{worldName}/{fileName}", data);
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    public void DeleteData(string userId, WorldTypeEnum worldType, Vector3Int position)
    {
        string worldName = $"{saveFileName}_{EnumExtension.GetEnumName(worldType)}";
        string fileName = $"w_{position.x}_{position.z}";
        BaseDeleteFile($"{userId}/{worldName}/{fileName}");
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    public void DeleteDataForCreature(string userId, WorldTypeEnum worldType, Vector3Int position)
    {
        string worldName = $"{saveFileNameForCreature}_{EnumExtension.GetEnumName(worldType)}";
        string fileName = $"w_{position.x}_{position.z}_creature";
        BaseDeleteFile($"{userId}/{worldName}/{fileName}");
    }
}