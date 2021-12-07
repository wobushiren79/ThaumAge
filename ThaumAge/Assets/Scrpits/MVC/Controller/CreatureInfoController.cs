/*
* FileName: CreatureInfo 
* Author: AppleCoffee 
* CreateTime: 2021-12-07-10:59:41 
*/

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreatureInfoController : BaseMVCController<CreatureInfoModel, ICreatureInfoView>
{

    public CreatureInfoController(BaseMonoBehaviour content, ICreatureInfoView view) : base(content, view)
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
    public CreatureInfoBean GetCreatureInfoData(Action<CreatureInfoBean> action)
    {
        CreatureInfoBean data = GetModel().GetCreatureInfoData();
        if (data == null) {
            GetView().GetCreatureInfoFail("没有数据",null);
            return null;
        }
        GetView().GetCreatureInfoSuccess<CreatureInfoBean>(data,action);
        return data;
    }

    /// <summary>
    /// 获取所有数据
    /// </summary>
    /// <param name="action"></param>
    public void GetAllCreatureInfoData(Action<List<CreatureInfoBean>> action)
    {
        List<CreatureInfoBean> listData = GetModel().GetAllCreatureInfoData();
        if (listData.IsNull())
        {
            GetView().GetCreatureInfoFail("没有数据", null);
        }
        else
        {
            GetView().GetCreatureInfoSuccess<List<CreatureInfoBean>>(listData, action);
        }
    }

    /// <summary>
    /// 根据ID获取数据
    /// </summary>
    /// <param name="action"></param>
    public void GetCreatureInfoDataById(long id,Action<CreatureInfoBean> action)
    {
        List<CreatureInfoBean> listData = GetModel().GetCreatureInfoDataById(id);
        if (listData.IsNull())
        {
            GetView().GetCreatureInfoFail("没有数据", null);
        }
        else
        {
            GetView().GetCreatureInfoSuccess(listData[0], action);
        }
    }
} 