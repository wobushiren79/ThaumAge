/*
* FileName: BookModelInfo 
* Author: AppleCoffee 
* CreateTime: 2022-03-29-22:32:49 
*/

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class BookModelInfoService : BaseDataRead<BookModelInfoBean>
{
    protected readonly string saveFileName;

    public BookModelInfoService()
    {
        saveFileName = "BookModelInfo";
    }

    /// <summary>
    /// 查询所有数据
    /// </summary>
    /// <returns></returns>
    public List<BookModelInfoBean> QueryAllData()
    {
        return BaseLoadDataForList(saveFileName); 
    }
        
    /// <summary>
    /// 通过ID查询数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<BookModelInfoBean> QueryDataById(long id)
    {
        return null;
    }

        /// <summary>
    /// 查询游戏配置数据
    /// </summary>
    /// <returns></returns>
    public BookModelInfoBean QueryData()
    {
        return null;
    }
        

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="data"></param>
    public void UpdateData(BookModelInfoBean data)
    {

    }
}