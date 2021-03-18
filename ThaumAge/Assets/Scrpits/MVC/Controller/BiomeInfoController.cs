/*
* FileName: BiomeInfo 
* Author: AppleCoffee 
* CreateTime: 2021-03-18-17:53:13 
*/

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BiomeInfoController : BaseMVCController<BiomeInfoModel, IBiomeInfoView>
{

    public BiomeInfoController(BaseMonoBehaviour content, IBiomeInfoView view) : base(content, view)
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
    public BiomeInfoBean GetBiomeInfoData(Action<BiomeInfoBean> action)
    {
        BiomeInfoBean data = GetModel().GetBiomeInfoData();
        if (data == null) {
            GetView().GetBiomeInfoFail("没有数据",null);
            return null;
        }
        GetView().GetBiomeInfoSuccess<BiomeInfoBean>(data,action);
        return data;
    }

    /// <summary>
    /// 获取所有数据
    /// </summary>
    /// <param name="action"></param>
    public void GetAllBiomeInfoData(Action<List<BiomeInfoBean>> action)
    {
        List<BiomeInfoBean> listData = GetModel().GetAllBiomeInfoData();
        if (CheckUtil.ListIsNull(listData))
        {
            GetView().GetBiomeInfoFail("没有数据", null);
        }
        else
        {
            GetView().GetBiomeInfoSuccess<List<BiomeInfoBean>>(listData, action);
        }
    }

    /// <summary>
    /// 根据ID获取数据
    /// </summary>
    /// <param name="action"></param>
    public void GetBiomeInfoDataById(long id,Action<BiomeInfoBean> action)
    {
        List<BiomeInfoBean> listData = GetModel().GetBiomeInfoDataById(id);
        if (CheckUtil.ListIsNull(listData))
        {
            GetView().GetBiomeInfoFail("没有数据", null);
        }
        else
        {
            GetView().GetBiomeInfoSuccess(listData[0], action);
        }
    }
} 