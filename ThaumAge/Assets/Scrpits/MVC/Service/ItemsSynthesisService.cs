/*
* FileName: ItemsSynthesis 
* Author: AppleCoffee 
* CreateTime: 2021-12-28-17:06:25 
*/

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class ItemsSynthesisService : BaseDataRead<ItemsSynthesisBean>
{
    protected readonly string saveFileName;

    public ItemsSynthesisService()
    {
        saveFileName = "ItemsSynthesis";
    }

    /// <summary>
    /// 查询所有数据
    /// </summary>
    /// <returns></returns>
    public List<ItemsSynthesisBean> QueryAllData()
    {
        return BaseLoadDataForList(saveFileName); 
    }
        
    /// <summary>
    /// 通过ID查询数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<ItemsSynthesisBean> QueryDataById(long id)
    {
        return null;
    }

        /// <summary>
    /// 查询游戏配置数据
    /// </summary>
    /// <returns></returns>
    public ItemsSynthesisBean QueryData()
    {
        return null;
    }
        

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="data"></param>
    public void UpdateData(ItemsSynthesisBean data)
    {

    }
}