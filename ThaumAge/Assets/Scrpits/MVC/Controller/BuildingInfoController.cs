/*
* FileName: BuildingInfo 
* Author: AppleCoffee 
* CreateTime: 2021-06-08-14:40:47 
*/

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildingInfoController : BaseMVCController<BuildingInfoModel, IBuildingInfoView>
{

    public BuildingInfoController(BaseMonoBehaviour content, IBuildingInfoView view) : base(content, view)
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
    public BuildingInfoBean GetBuildingInfoData(Action<BuildingInfoBean> action)
    {
        BuildingInfoBean data = GetModel().GetBuildingInfoData();
        if (data == null) {
            GetView().GetBuildingInfoFail("没有数据",null);
            return null;
        }
        GetView().GetBuildingInfoSuccess<BuildingInfoBean>(data,action);
        return data;
    }

    /// <summary>
    /// 获取所有数据
    /// </summary>
    /// <param name="action"></param>
    public void GetAllBuildingInfoData(Action<List<BuildingInfoBean>> action)
    {
        List<BuildingInfoBean> listData = GetModel().GetAllBuildingInfoData();
        if (CheckUtil.ListIsNull(listData))
        {
            GetView().GetBuildingInfoFail("没有数据", null);
        }
        else
        {
            GetView().GetBuildingInfoSuccess<List<BuildingInfoBean>>(listData, action);
        }
    }

    /// <summary>
    /// 根据ID获取数据
    /// </summary>
    /// <param name="action"></param>
    public void GetBuildingInfoDataById(long id,Action<BuildingInfoBean> action)
    {
        List<BuildingInfoBean> listData = GetModel().GetBuildingInfoDataById(id);
        if (CheckUtil.ListIsNull(listData))
        {
            GetView().GetBuildingInfoFail("没有数据", null);
        }
        else
        {
            GetView().GetBuildingInfoSuccess(listData[0], action);
        }
    }
} 