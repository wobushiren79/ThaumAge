/*
* FileName: CreatureInfo 
* Author: AppleCoffee 
* CreateTime: 2021-12-07-10:59:41 
*/

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class CreatureInfoService : BaseDataRead<CreatureInfoBean>
{
    protected readonly string saveFileName;

    public CreatureInfoService()
    {
        saveFileName = "CreatureInfo";
    }

    /// <summary>
    /// 查询所有数据
    /// </summary>
    /// <returns></returns>
    public List<CreatureInfoBean> QueryAllData()
    {
        return BaseLoadDataForList(saveFileName); 
    }
        
    /// <summary>
    /// 通过ID查询数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<CreatureInfoBean> QueryDataById(long id)
    {
        return null;
    }

        /// <summary>
    /// 查询游戏配置数据
    /// </summary>
    /// <returns></returns>
    public CreatureInfoBean QueryData()
    {
        return null;
    }
        

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="data"></param>
    public void UpdateData(CreatureInfoBean data)
    {

    }
}