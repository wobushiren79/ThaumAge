/*
* FileName: BookModelDetailsInfo 
* Author: AppleCoffee 
* CreateTime: 2022-03-29-22:33:00 
*/

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class BookModelDetailsInfoService : BaseDataRead<BookModelDetailsInfoBean>
{
    protected readonly string saveFileName;

    public BookModelDetailsInfoService()
    {
        saveFileName = "BookModelDetailsInfo";
    }

    /// <summary>
    /// 查询所有数据
    /// </summary>
    /// <returns></returns>
    public List<BookModelDetailsInfoBean> QueryAllData()
    {
        return BaseLoadDataForList(saveFileName); 
    }
        
    /// <summary>
    /// 通过ID查询数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<BookModelDetailsInfoBean> QueryDataById(long id)
    {
        return null;
    }

        /// <summary>
    /// 查询游戏配置数据
    /// </summary>
    /// <returns></returns>
    public BookModelDetailsInfoBean QueryData()
    {
        return null;
    }
        

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="data"></param>
    public void UpdateData(BookModelDetailsInfoBean data)
    {

    }
}