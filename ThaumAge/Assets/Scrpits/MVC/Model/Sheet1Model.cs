/*
* FileName: Sheet1 
* Author: AppleCoffee 
* CreateTime: 2021-05-26-18:49:53 
*/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class Sheet1Model : BaseMVCModel
{
    protected Sheet1Service serviceSheet1;

    public override void InitData()
    {
        serviceSheet1 = new Sheet1Service();
    }

    /// <summary>
    /// 获取所有数据
    /// </summary>
    /// <returns></returns>
    public List<Sheet1Bean> GetAllSheet1Data()
    {
        List<Sheet1Bean> listData = serviceSheet1.QueryAllData();
        return listData;
    }

    /// <summary>
    /// 获取游戏数据
    /// </summary>
    /// <returns></returns>
    public Sheet1Bean GetSheet1Data()
    {
        Sheet1Bean data = serviceSheet1.QueryData();
        if (data == null)
            data = new Sheet1Bean();
        return data;
    }

    /// <summary>
    /// 根据ID获取数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<Sheet1Bean> GetSheet1DataById(long id)
    {
        List<Sheet1Bean> listData = serviceSheet1.QueryDataById(id);
        return listData;
    }

    /// <summary>
    /// 保存游戏数据
    /// </summary>
    /// <param name="data"></param>
    public void SetSheet1Data(Sheet1Bean data)
    {
        serviceSheet1.UpdateData(data);
    }

}