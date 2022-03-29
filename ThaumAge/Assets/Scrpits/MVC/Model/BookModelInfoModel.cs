/*
* FileName: BookModelInfo 
* Author: AppleCoffee 
* CreateTime: 2022-03-29-22:32:49 
*/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BookModelInfoModel : BaseMVCModel
{
    protected BookModelInfoService serviceBookModelInfo;

    public override void InitData()
    {
        serviceBookModelInfo = new BookModelInfoService();
    }

    /// <summary>
    /// 获取所有数据
    /// </summary>
    /// <returns></returns>
    public List<BookModelInfoBean> GetAllBookModelInfoData()
    {
        List<BookModelInfoBean> listData = serviceBookModelInfo.QueryAllData();
        return listData;
    }

    /// <summary>
    /// 获取游戏数据
    /// </summary>
    /// <returns></returns>
    public BookModelInfoBean GetBookModelInfoData()
    {
        BookModelInfoBean data = serviceBookModelInfo.QueryData();
        if (data == null)
            data = new BookModelInfoBean();
        return data;
    }

    /// <summary>
    /// 根据ID获取数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<BookModelInfoBean> GetBookModelInfoDataById(long id)
    {
        List<BookModelInfoBean> listData = serviceBookModelInfo.QueryDataById(id);
        return listData;
    }

    /// <summary>
    /// 保存游戏数据
    /// </summary>
    /// <param name="data"></param>
    public void SetBookModelInfoData(BookModelInfoBean data)
    {
        serviceBookModelInfo.UpdateData(data);
    }

}