/*
* FileName: ElementalInfo 
* Author: AppleCoffee 
* CreateTime: 2022-10-11-21:25:04 
*/

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class ElementalInfoService : BaseDataRead<ElementalInfoBean>
{
    protected readonly string saveFileName;

    public ElementalInfoService()
    {
        saveFileName = "ElementalInfo";
    }

    /// <summary>
    /// 查询所有数据
    /// </summary>
    /// <returns></returns>
    public List<ElementalInfoBean> QueryAllData()
    {
        return BaseLoadDataForList(saveFileName); 
    }
        
    /// <summary>
    /// 通过ID查询数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<ElementalInfoBean> QueryDataById(long id)
    {
        return null;
    }

        /// <summary>
    /// 查询游戏配置数据
    /// </summary>
    /// <returns></returns>
    public ElementalInfoBean QueryData()
    {
        return null;
    }
        

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="data"></param>
    public void UpdateData(ElementalInfoBean data)
    {

    }
}