/*
* FileName: ItemsSynthesis 
* Author: AppleCoffee 
* CreateTime: 2021-12-28-17:06:25 
*/

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemsSynthesisController : BaseMVCController<ItemsSynthesisModel, IItemsSynthesisView>
{

    public ItemsSynthesisController(BaseMonoBehaviour content, IItemsSynthesisView view) : base(content, view)
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
    public ItemsSynthesisBean GetItemsSynthesisData(Action<ItemsSynthesisBean> action)
    {
        ItemsSynthesisBean data = GetModel().GetItemsSynthesisData();
        if (data == null) {
            GetView().GetItemsSynthesisFail("没有数据",null);
            return null;
        }
        GetView().GetItemsSynthesisSuccess<ItemsSynthesisBean>(data,action);
        return data;
    }

    /// <summary>
    /// 获取所有数据
    /// </summary>
    /// <param name="action"></param>
    public void GetAllItemsSynthesisData(Action<List<ItemsSynthesisBean>> action)
    {
        List<ItemsSynthesisBean> listData = GetModel().GetAllItemsSynthesisData();
        if (listData.IsNull())
        {
            GetView().GetItemsSynthesisFail("没有数据", null);
        }
        else
        {
            GetView().GetItemsSynthesisSuccess<List<ItemsSynthesisBean>>(listData, action);
        }
    }

    /// <summary>
    /// 根据ID获取数据
    /// </summary>
    /// <param name="action"></param>
    public void GetItemsSynthesisDataById(long id,Action<ItemsSynthesisBean> action)
    {
        List<ItemsSynthesisBean> listData = GetModel().GetItemsSynthesisDataById(id);
        if (listData.IsNull())
        {
            GetView().GetItemsSynthesisFail("没有数据", null);
        }
        else
        {
            GetView().GetItemsSynthesisSuccess(listData[0], action);
        }
    }
} 