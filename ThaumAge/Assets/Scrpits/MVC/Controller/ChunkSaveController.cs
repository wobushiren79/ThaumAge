/*
* FileName: WorldData 
* Author: AppleCoffee 
* CreateTime: 2021-03-24-13:49:11 
*/

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChunkSaveController : BaseMVCController<ChunkSaveModel, IChunkSaveView>
{

    public ChunkSaveController(BaseMonoBehaviour content, IChunkSaveView view) : base(content, view)
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
    public ChunkSaveBean GetChunkSaveData(string userId, WorldTypeEnum worldType, Vector3Int position, Action<ChunkSaveBean> action)
    {
        ChunkSaveBean data = GetModel().GetChunkSaveData(userId, worldType, position);
        if (data == null)
        {
            GetView().GetChunkSaveFail("没有数据", null);
            return null;
        }
        GetView().GetChunkSaveSuccess(data, action);
        return data;
    }

    /// <summary>
    /// 保存世界数据
    /// </summary>
    /// <param name="worldData"></param>
    /// <param name="action"></param>
    public void SetChunkSaveData(ChunkSaveBean chunkSaveData, Action<ChunkSaveBean> action)
    {
        GetModel().SetChunkSaveData(chunkSaveData);
    }
}