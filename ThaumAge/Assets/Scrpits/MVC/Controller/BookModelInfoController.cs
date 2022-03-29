/*
* FileName: BookModelInfo 
* Author: AppleCoffee 
* CreateTime: 2022-03-29-22:32:49 
*/

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BookModelInfoController : BaseMVCController<BookModelInfoModel, IBookModelInfoView>
{

    public BookModelInfoController(BaseMonoBehaviour content, IBookModelInfoView view) : base(content, view)
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
    public BookModelInfoBean GetBookModelInfoData(Action<BookModelInfoBean> action)
    {
        BookModelInfoBean data = GetModel().GetBookModelInfoData();
        if (data == null) {
            GetView().GetBookModelInfoFail("没有数据",null);
            return null;
        }
        GetView().GetBookModelInfoSuccess<BookModelInfoBean>(data,action);
        return data;
    }

    /// <summary>
    /// 获取所有数据
    /// </summary>
    /// <param name="action"></param>
    public void GetAllBookModelInfoData(Action<List<BookModelInfoBean>> action)
    {
        List<BookModelInfoBean> listData = GetModel().GetAllBookModelInfoData();
        if (listData.IsNull())
        {
            GetView().GetBookModelInfoFail("没有数据", null);
        }
        else
        {
            GetView().GetBookModelInfoSuccess<List<BookModelInfoBean>>(listData, action);
        }
    }

    /// <summary>
    /// 根据ID获取数据
    /// </summary>
    /// <param name="action"></param>
    public void GetBookModelInfoDataById(long id,Action<BookModelInfoBean> action)
    {
        List<BookModelInfoBean> listData = GetModel().GetBookModelInfoDataById(id);
        if (listData.IsNull())
        {
            GetView().GetBookModelInfoFail("没有数据", null);
        }
        else
        {
            GetView().GetBookModelInfoSuccess(listData[0], action);
        }
    }
} 