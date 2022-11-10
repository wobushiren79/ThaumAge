/*
* FileName: MagicInstrumentInfo 
* Author: AppleCoffee 
* CreateTime: 2022-11-10-18:16:13 
*/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MagicInstrumentInfoModel : BaseMVCModel
{
    protected MagicInstrumentInfoService serviceMagicInstrumentInfo;

    public override void InitData()
    {
        serviceMagicInstrumentInfo = new MagicInstrumentInfoService();
    }

    /// <summary>
    /// 获取所有数据
    /// </summary>
    /// <returns></returns>
    public List<MagicInstrumentInfoBean> GetAllMagicInstrumentInfoData()
    {
        List<MagicInstrumentInfoBean> listData = serviceMagicInstrumentInfo.QueryAllData();
        return listData;
    }

    /// <summary>
    /// 获取游戏数据
    /// </summary>
    /// <returns></returns>
    public MagicInstrumentInfoBean GetMagicInstrumentInfoData()
    {
        MagicInstrumentInfoBean data = serviceMagicInstrumentInfo.QueryData();
        if (data == null)
            data = new MagicInstrumentInfoBean();
        return data;
    }

    /// <summary>
    /// 根据ID获取数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<MagicInstrumentInfoBean> GetMagicInstrumentInfoDataById(long id)
    {
        List<MagicInstrumentInfoBean> listData = serviceMagicInstrumentInfo.QueryDataById(id);
        return listData;
    }

    /// <summary>
    /// 保存游戏数据
    /// </summary>
    /// <param name="data"></param>
    public void SetMagicInstrumentInfoData(MagicInstrumentInfoBean data)
    {
        serviceMagicInstrumentInfo.UpdateData(data);
    }

}