/*
* FileName: WorldData 
* Author: AppleCoffee 
* CreateTime: 2021-03-24-13:49:11 
*/

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WorldDataController : BaseMVCController<WorldDataModel, IWorldDataView>
{

    public WorldDataController(BaseMonoBehaviour content, IWorldDataView view) : base(content, view)
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
    public WorldDataBean GetWorldData(string userId, WorldTypeEnum worldType, Vector3Int position, Action<WorldDataBean> action)
    {
        WorldDataBean data = GetModel().GetWorldDataData(userId, worldType, position);
        if (data == null)
        {
            GetView().GetWorldDataFail("没有数据", null);
            return null;
        }
        GetView().GetWorldDataSuccess(data, action);
        return data;
    }

    /// <summary>
    /// 保存世界数据
    /// </summary>
    /// <param name="worldData"></param>
    /// <param name="action"></param>
    public void SetWorldData(WorldDataBean worldData, Action<WorldDataBean> action)
    {
        GetModel().SetWorldDataData(worldData);
    }
}