/*
* FileName: WorldData 
* Author: AppleCoffee 
* CreateTime: 2021-03-24-13:49:11 
*/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class WorldDataModel : BaseMVCModel
{
    protected WorldDataService serviceWorldData;

    public override void InitData()
    {
        serviceWorldData = new WorldDataService();
    }

    /// <summary>
    /// 获取游戏数据
    /// </summary>
    /// <returns></returns>
    public WorldDataBean GetWorldDataData(string userId, WorldTypeEnum worldType, Vector3Int position)
    {
        WorldDataBean data = serviceWorldData.QueryData(userId, worldType, position);
        return data;
    }

    /// <summary>
    /// 保存游戏数据
    /// </summary>
    /// <param name="data"></param>
    public void SetWorldDataData(WorldDataBean data)
    {
        serviceWorldData.UpdateData(data);
    }

}