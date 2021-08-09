/*
* FileName: Sheet1 
* Author: AppleCoffee 
* CreateTime: 2021-05-26-18:49:53 
*/

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class Sheet1Service : BaseDataRead<Sheet1Bean>
{
    protected readonly string saveFileName;

    public Sheet1Service()
    {
        saveFileName = "Sheet1";
    }

    /// <summary>
    /// 查询所有数据
    /// </summary>
    /// <returns></returns>
    public List<Sheet1Bean> QueryAllData()
    {
        return BaseLoadDataForList(saveFileName); 
    }
        
    /// <summary>
    /// 通过ID查询数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<Sheet1Bean> QueryDataById(long id)
    {
        return null;
    }

        /// <summary>
    /// 查询游戏配置数据
    /// </summary>
    /// <returns></returns>
    public Sheet1Bean QueryData()
    {
        return null;
    }
        

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="data"></param>
    public void UpdateData(Sheet1Bean data)
    {

    }
}