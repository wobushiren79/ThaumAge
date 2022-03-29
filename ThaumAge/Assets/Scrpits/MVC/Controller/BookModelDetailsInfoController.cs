/*
* FileName: BookModelDetailsInfo 
* Author: AppleCoffee 
* CreateTime: 2022-03-29-22:33:00 
*/

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BookModelDetailsInfoController : BaseMVCController<BookModelDetailsInfoModel, IBookModelDetailsInfoView>
{

    public BookModelDetailsInfoController(BaseMonoBehaviour content, IBookModelDetailsInfoView view) : base(content, view)
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
    public BookModelDetailsInfoBean GetBookModelDetailsInfoData(Action<BookModelDetailsInfoBean> action)
    {
        BookModelDetailsInfoBean data = GetModel().GetBookModelDetailsInfoData();
        if (data == null) {
            GetView().GetBookModelDetailsInfoFail("没有数据",null);
            return null;
        }
        GetView().GetBookModelDetailsInfoSuccess<BookModelDetailsInfoBean>(data,action);
        return data;
    }

    /// <summary>
    /// 获取所有数据
    /// </summary>
    /// <param name="action"></param>
    public void GetAllBookModelDetailsInfoData(Action<List<BookModelDetailsInfoBean>> action)
    {
        List<BookModelDetailsInfoBean> listData = GetModel().GetAllBookModelDetailsInfoData();
        if (listData.IsNull())
        {
            GetView().GetBookModelDetailsInfoFail("没有数据", null);
        }
        else
        {
            GetView().GetBookModelDetailsInfoSuccess<List<BookModelDetailsInfoBean>>(listData, action);
        }
    }

    /// <summary>
    /// 根据ID获取数据
    /// </summary>
    /// <param name="action"></param>
    public void GetBookModelDetailsInfoDataById(long id,Action<BookModelDetailsInfoBean> action)
    {
        List<BookModelDetailsInfoBean> listData = GetModel().GetBookModelDetailsInfoDataById(id);
        if (listData.IsNull())
        {
            GetView().GetBookModelDetailsInfoFail("没有数据", null);
        }
        else
        {
            GetView().GetBookModelDetailsInfoSuccess(listData[0], action);
        }
    }
} 