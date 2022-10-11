/*
* FileName: ElementalInfo 
* Author: AppleCoffee 
* CreateTime: 2022-10-11-21:25:04 
*/

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ElementalInfoController : BaseMVCController<ElementalInfoModel, IElementalInfoView>
{

    public ElementalInfoController(BaseMonoBehaviour content, IElementalInfoView view) : base(content, view)
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
    public ElementalInfoBean GetElementalInfoData(Action<ElementalInfoBean> action)
    {
        ElementalInfoBean data = GetModel().GetElementalInfoData();
        if (data == null) {
            GetView().GetElementalInfoFail("没有数据",null);
            return null;
        }
        GetView().GetElementalInfoSuccess<ElementalInfoBean>(data,action);
        return data;
    }

    /// <summary>
    /// 获取所有数据
    /// </summary>
    /// <param name="action"></param>
    public void GetAllElementalInfoData(Action<List<ElementalInfoBean>> action)
    {
        List<ElementalInfoBean> listData = GetModel().GetAllElementalInfoData();
        if (listData.IsNull())
        {
            GetView().GetElementalInfoFail("没有数据", null);
        }
        else
        {
            GetView().GetElementalInfoSuccess<List<ElementalInfoBean>>(listData, action);
        }
    }

    /// <summary>
    /// 根据ID获取数据
    /// </summary>
    /// <param name="action"></param>
    public void GetElementalInfoDataById(long id,Action<ElementalInfoBean> action)
    {
        List<ElementalInfoBean> listData = GetModel().GetElementalInfoDataById(id);
        if (listData.IsNull())
        {
            GetView().GetElementalInfoFail("没有数据", null);
        }
        else
        {
            GetView().GetElementalInfoSuccess(listData[0], action);
        }
    }
} 