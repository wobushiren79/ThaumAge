/*
* FileName: BookModelDetailsInfo 
* Author: AppleCoffee 
* CreateTime: 2022-03-29-22:33:00 
*/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BookModelDetailsInfoModel : BaseMVCModel
{
    protected BookModelDetailsInfoService serviceBookModelDetailsInfo;

    public override void InitData()
    {
        serviceBookModelDetailsInfo = new BookModelDetailsInfoService();
    }

    /// <summary>
    /// 获取所有数据
    /// </summary>
    /// <returns></returns>
    public List<BookModelDetailsInfoBean> GetAllBookModelDetailsInfoData()
    {
        List<BookModelDetailsInfoBean> listData = serviceBookModelDetailsInfo.QueryAllData();
        return listData;
    }

    /// <summary>
    /// 获取游戏数据
    /// </summary>
    /// <returns></returns>
    public BookModelDetailsInfoBean GetBookModelDetailsInfoData()
    {
        BookModelDetailsInfoBean data = serviceBookModelDetailsInfo.QueryData();
        if (data == null)
            data = new BookModelDetailsInfoBean();
        return data;
    }

    /// <summary>
    /// 根据ID获取数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<BookModelDetailsInfoBean> GetBookModelDetailsInfoDataById(long id)
    {
        List<BookModelDetailsInfoBean> listData = serviceBookModelDetailsInfo.QueryDataById(id);
        return listData;
    }

    /// <summary>
    /// 保存游戏数据
    /// </summary>
    /// <param name="data"></param>
    public void SetBookModelDetailsInfoData(BookModelDetailsInfoBean data)
    {
        serviceBookModelDetailsInfo.UpdateData(data);
    }

}