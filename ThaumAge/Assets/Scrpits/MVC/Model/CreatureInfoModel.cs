/*
* FileName: CreatureInfo 
* Author: AppleCoffee 
* CreateTime: 2021-12-07-10:59:41 
*/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CreatureInfoModel : BaseMVCModel
{
    protected CreatureInfoService serviceCreatureInfo;

    public override void InitData()
    {
        serviceCreatureInfo = new CreatureInfoService();
    }

    /// <summary>
    /// 获取所有数据
    /// </summary>
    /// <returns></returns>
    public List<CreatureInfoBean> GetAllCreatureInfoData()
    {
        List<CreatureInfoBean> listData = serviceCreatureInfo.QueryAllData();
        return listData;
    }

    /// <summary>
    /// 获取游戏数据
    /// </summary>
    /// <returns></returns>
    public CreatureInfoBean GetCreatureInfoData()
    {
        CreatureInfoBean data = serviceCreatureInfo.QueryData();
        if (data == null)
            data = new CreatureInfoBean();
        return data;
    }

    /// <summary>
    /// 根据ID获取数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<CreatureInfoBean> GetCreatureInfoDataById(long id)
    {
        List<CreatureInfoBean> listData = serviceCreatureInfo.QueryDataById(id);
        return listData;
    }

    /// <summary>
    /// 保存游戏数据
    /// </summary>
    /// <param name="data"></param>
    public void SetCreatureInfoData(CreatureInfoBean data)
    {
        serviceCreatureInfo.UpdateData(data);
    }

}