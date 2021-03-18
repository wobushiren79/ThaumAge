/*
* FileName: BiomeInfo 
* Author: AppleCoffee 
* CreateTime: 2021-03-18-17:53:13 
*/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BiomeInfoModel : BaseMVCModel
{
    protected BiomeInfoService serviceBiomeInfo;

    public override void InitData()
    {
        serviceBiomeInfo = new BiomeInfoService();
    }

    /// <summary>
    /// 获取所有数据
    /// </summary>
    /// <returns></returns>
    public List<BiomeInfoBean> GetAllBiomeInfoData()
    {
        List<BiomeInfoBean> listData = serviceBiomeInfo.QueryAllData();
        return listData;
    }

    /// <summary>
    /// 获取游戏数据
    /// </summary>
    /// <returns></returns>
    public BiomeInfoBean GetBiomeInfoData()
    {
        BiomeInfoBean data = serviceBiomeInfo.QueryData();
        if (data == null)
            data = new BiomeInfoBean();
        return data;
    }

    /// <summary>
    /// 根据ID获取数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<BiomeInfoBean> GetBiomeInfoDataById(long id)
    {
        List<BiomeInfoBean> listData = serviceBiomeInfo.QueryDataById(id);
        return listData;
    }

    /// <summary>
    /// 保存游戏数据
    /// </summary>
    /// <param name="data"></param>
    public void SetBiomeInfoData(BiomeInfoBean data)
    {
        serviceBiomeInfo.UpdateData(data);
    }

}