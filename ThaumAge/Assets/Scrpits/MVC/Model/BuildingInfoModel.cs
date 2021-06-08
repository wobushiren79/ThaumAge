/*
* FileName: BuildingInfo 
* Author: AppleCoffee 
* CreateTime: 2021-06-08-14:40:47 
*/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BuildingInfoModel : BaseMVCModel
{
    protected BuildingInfoService serviceBuildingInfo;

    public override void InitData()
    {
        serviceBuildingInfo = new BuildingInfoService();
    }

    /// <summary>
    /// 获取所有数据
    /// </summary>
    /// <returns></returns>
    public List<BuildingInfoBean> GetAllBuildingInfoData()
    {
        List<BuildingInfoBean> listData = serviceBuildingInfo.QueryAllData();
        return listData;
    }

    /// <summary>
    /// 获取游戏数据
    /// </summary>
    /// <returns></returns>
    public BuildingInfoBean GetBuildingInfoData()
    {
        BuildingInfoBean data = serviceBuildingInfo.QueryData();
        if (data == null)
            data = new BuildingInfoBean();
        return data;
    }

    /// <summary>
    /// 根据ID获取数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<BuildingInfoBean> GetBuildingInfoDataById(long id)
    {
        List<BuildingInfoBean> listData = serviceBuildingInfo.QueryDataById(id);
        return listData;
    }

    /// <summary>
    /// 保存游戏数据
    /// </summary>
    /// <param name="data"></param>
    public void SetBuildingInfoData(BuildingInfoBean data)
    {
        serviceBuildingInfo.UpdateData(data);
    }

}