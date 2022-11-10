/*
* FileName: MagicInstrumentInfo 
* Author: AppleCoffee 
* CreateTime: 2022-11-10-18:16:13 
*/

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class MagicInstrumentInfoService : BaseDataRead<MagicInstrumentInfoBean>
{
    protected readonly string saveFileName;

    public MagicInstrumentInfoService()
    {
        saveFileName = "MagicInstrumentInfo";
    }

    /// <summary>
    /// 查询所有数据
    /// </summary>
    /// <returns></returns>
    public List<MagicInstrumentInfoBean> QueryAllData()
    {
        return BaseLoadDataForList(saveFileName); 
    }
        
    /// <summary>
    /// 通过ID查询数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<MagicInstrumentInfoBean> QueryDataById(long id)
    {
        return null;
    }

        /// <summary>
    /// 查询游戏配置数据
    /// </summary>
    /// <returns></returns>
    public MagicInstrumentInfoBean QueryData()
    {
        return null;
    }
        

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="data"></param>
    public void UpdateData(MagicInstrumentInfoBean data)
    {

    }
}