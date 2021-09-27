/*
* FileName: ItemsInfo 
* Author: AppleCoffee 
* CreateTime: 2021-05-31-15:59:03 
*/

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemsInfoController : BaseMVCController<ItemsInfoModel, IItemsInfoView>
{

    public ItemsInfoController(BaseMonoBehaviour content, IItemsInfoView view) : base(content, view)
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
    public ItemsInfoBean GetItemsInfoData(Action<ItemsInfoBean> action)
    {
        ItemsInfoBean data = GetModel().GetItemsInfoData();
        if (data == null) {
            GetView().GetItemsInfoFail("没有数据",null);
            return null;
        }
        GetView().GetItemsInfoSuccess<ItemsInfoBean>(data,action);
        return data;
    }

    /// <summary>
    /// 获取所有数据
    /// </summary>
    /// <param name="action"></param>
    public void GetAllItemsInfoData(Action<List<ItemsInfoBean>> action)
    {
        List<ItemsInfoBean> listData = GetModel().GetAllItemsInfoData();
        if (listData.IsNull())
        {
            GetView().GetItemsInfoFail("没有数据", null);
        }
        else
        {
            GetView().GetItemsInfoSuccess<List<ItemsInfoBean>>(listData, action);
        }
    }

    /// <summary>
    /// 根据ID获取数据
    /// </summary>
    /// <param name="action"></param>
    public void GetItemsInfoDataById(long id,Action<ItemsInfoBean> action)
    {
        List<ItemsInfoBean> listData = GetModel().GetItemsInfoDataById(id);
        if (listData.IsNull())
        {
            GetView().GetItemsInfoFail("没有数据", null);
        }
        else
        {
            GetView().GetItemsInfoSuccess(listData[0], action);
        }
    }
} 