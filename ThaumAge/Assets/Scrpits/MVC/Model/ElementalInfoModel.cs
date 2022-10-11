/*
* FileName: ElementalInfo 
* Author: AppleCoffee 
* CreateTime: 2022-10-11-21:25:04 
*/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ElementalInfoModel : BaseMVCModel
{
    protected ElementalInfoService serviceElementalInfo;

    public override void InitData()
    {
        serviceElementalInfo = new ElementalInfoService();
    }

    /// <summary>
    /// 获取所有数据
    /// </summary>
    /// <returns></returns>
    public List<ElementalInfoBean> GetAllElementalInfoData()
    {
        List<ElementalInfoBean> listData = serviceElementalInfo.QueryAllData();
        return listData;
    }

    /// <summary>
    /// 获取游戏数据
    /// </summary>
    /// <returns></returns>
    public ElementalInfoBean GetElementalInfoData()
    {
        ElementalInfoBean data = serviceElementalInfo.QueryData();
        if (data == null)
            data = new ElementalInfoBean();
        return data;
    }

    /// <summary>
    /// 根据ID获取数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<ElementalInfoBean> GetElementalInfoDataById(long id)
    {
        List<ElementalInfoBean> listData = serviceElementalInfo.QueryDataById(id);
        return listData;
    }

    /// <summary>
    /// 保存游戏数据
    /// </summary>
    /// <param name="data"></param>
    public void SetElementalInfoData(ElementalInfoBean data)
    {
        serviceElementalInfo.UpdateData(data);
    }

}