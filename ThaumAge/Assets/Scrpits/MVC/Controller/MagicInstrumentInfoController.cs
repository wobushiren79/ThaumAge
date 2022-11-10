/*
* FileName: MagicInstrumentInfo 
* Author: AppleCoffee 
* CreateTime: 2022-11-10-18:16:13 
*/

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MagicInstrumentInfoController : BaseMVCController<MagicInstrumentInfoModel, IMagicInstrumentInfoView>
{

    public MagicInstrumentInfoController(BaseMonoBehaviour content, IMagicInstrumentInfoView view) : base(content, view)
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
    public MagicInstrumentInfoBean GetMagicInstrumentInfoData(Action<MagicInstrumentInfoBean> action)
    {
        MagicInstrumentInfoBean data = GetModel().GetMagicInstrumentInfoData();
        if (data == null) {
            GetView().GetMagicInstrumentInfoFail("没有数据",null);
            return null;
        }
        GetView().GetMagicInstrumentInfoSuccess<MagicInstrumentInfoBean>(data,action);
        return data;
    }

    /// <summary>
    /// 获取所有数据
    /// </summary>
    /// <param name="action"></param>
    public void GetAllMagicInstrumentInfoData(Action<List<MagicInstrumentInfoBean>> action)
    {
        List<MagicInstrumentInfoBean> listData = GetModel().GetAllMagicInstrumentInfoData();
        if (listData.IsNull())
        {
            GetView().GetMagicInstrumentInfoFail("没有数据", null);
        }
        else
        {
            GetView().GetMagicInstrumentInfoSuccess<List<MagicInstrumentInfoBean>>(listData, action);
        }
    }

    /// <summary>
    /// 根据ID获取数据
    /// </summary>
    /// <param name="action"></param>
    public void GetMagicInstrumentInfoDataById(long id,Action<MagicInstrumentInfoBean> action)
    {
        List<MagicInstrumentInfoBean> listData = GetModel().GetMagicInstrumentInfoDataById(id);
        if (listData.IsNull())
        {
            GetView().GetMagicInstrumentInfoFail("没有数据", null);
        }
        else
        {
            GetView().GetMagicInstrumentInfoSuccess(listData[0], action);
        }
    }
} 