/*
* FileName: Sheet1 
* Author: AppleCoffee 
* CreateTime: 2021-05-26-18:49:53 
*/

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Sheet1Controller : BaseMVCController<Sheet1Model, ISheet1View>
{

    public Sheet1Controller(BaseMonoBehaviour content, ISheet1View view) : base(content, view)
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
    public Sheet1Bean GetSheet1Data(Action<Sheet1Bean> action)
    {
        Sheet1Bean data = GetModel().GetSheet1Data();
        if (data == null) {
            GetView().GetSheet1Fail("没有数据",null);
            return null;
        }
        GetView().GetSheet1Success<Sheet1Bean>(data,action);
        return data;
    }

    /// <summary>
    /// 获取所有数据
    /// </summary>
    /// <param name="action"></param>
    public void GetAllSheet1Data(Action<List<Sheet1Bean>> action)
    {
        List<Sheet1Bean> listData = GetModel().GetAllSheet1Data();
        if (listData.IsNull())
        {
            GetView().GetSheet1Fail("没有数据", null);
        }
        else
        {
            GetView().GetSheet1Success<List<Sheet1Bean>>(listData, action);
        }
    }

    /// <summary>
    /// 根据ID获取数据
    /// </summary>
    /// <param name="action"></param>
    public void GetSheet1DataById(long id,Action<Sheet1Bean> action)
    {
        List<Sheet1Bean> listData = GetModel().GetSheet1DataById(id);
        if (listData.IsNull())
        {
            GetView().GetSheet1Fail("没有数据", null);
        }
        else
        {
            GetView().GetSheet1Success(listData[0], action);
        }
    }
} 