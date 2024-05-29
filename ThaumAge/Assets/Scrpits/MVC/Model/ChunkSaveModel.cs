/*
* FileName: WorldData 
* Author: AppleCoffee 
* CreateTime: 2021-03-24-13:49:11 
*/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ChunkSaveModel : BaseMVCModel
{
    protected ChunkSaveService serviceChunkSave;

    public override void InitData()
    {
        serviceChunkSave = new ChunkSaveService();
    }

    /// <summary>
    /// 获取游戏数据
    /// </summary>
    /// <returns></returns>
    public ChunkSaveBean GetChunkSaveData(string userId, WorldTypeEnum worldType, Vector3Int position)
    {
        ChunkSaveBean data = serviceChunkSave.QueryData(userId, worldType, position);
        return data;
    }

    /// <summary>
    /// 保存游戏数据
    /// </summary>
    /// <param name="data"></param>
    public void SetChunkSaveData(ChunkSaveBean data)
    {
        serviceChunkSave.UpdateData(data);
    }

    /// <summary>
    /// 获取游戏数据
    /// </summary>
    /// <returns></returns>
    public ChunkSaveCreatureBean GetChunkSaveCreatureData(string userId, WorldTypeEnum worldType, Vector3Int position)
    {
        ChunkSaveCreatureBean data = serviceChunkSave.QueryDataForCreature(userId, worldType, position);
        return data;
    }

    /// <summary>
    /// 保存游戏数据
    /// </summary>
    /// <param name="data"></param>
    public void SetChunkSaveCreatureData(ChunkSaveCreatureBean data)
    {
        serviceChunkSave.UpdateDataForCreature(data);
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="worldType"></param>
    /// <param name="position"></param>
    public void DeleteChunkSaveCreatureData(string userId, WorldTypeEnum worldType, Vector3Int position)
    {
        serviceChunkSave.DeleteDataForCreature(userId, worldType, position);
    }
}