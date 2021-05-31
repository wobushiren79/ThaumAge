/*
* FileName: ItemsInfo 
* Author: AppleCoffee 
* CreateTime: 2021-05-31-15:59:03 
*/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ItemsInfoService : BaseMVCService
{
    public ItemsInfoService() : base("items_info", "items_info_details_" + GameDataHandler.Instance.manager.GetGameConfig().language)
    {

    }

    /// <summary>
    /// 查询所有数据
    /// </summary>
    /// <returns></returns>
    public List<ItemsInfoBean> QueryAllData()
    {
        List<ItemsInfoBean> listData = BaseQueryAllData<ItemsInfoBean>();
        return listData; 
    }

    /// <summary>
    /// 查询数据
    /// </summary>
    /// <returns></returns>
    public ItemsInfoBean QueryData()
    {
        return null; 
    }

    /// <summary>
    /// 通过ID查询数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<ItemsInfoBean> QueryDataById(long id)
    {
        return BaseQueryData<ItemsInfoBean>("link_id", "id", id + "");
    }

    /// <summary>
    /// 根据ID查询数据
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public List<ItemsInfoBean> QueryDataByIds(long[] ids)
    {
        string values = TypeConversionUtil.ArrayToStringBySplit(ids, ",");
        return BaseQueryData<ItemsInfoBean>("link_id", tableNameForMain + ".id", "IN", "(" + values + ")");
    }

    /// <summary>
    /// 通过名字查询
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public List<ItemsInfoBean> QueryDataByName(string name)
    {
        return BaseQueryData<ItemsInfoBean>("link_id", tableNameForLeft + ".name", "'" + name + "'");
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool UpdateData(ItemsInfoBean data)
    {
        bool deleteState = BaseDeleteDataById(data.id);
        if (deleteState)
        {
            bool insertSuccess = BaseInsertData(tableNameForMain, data);
            return insertSuccess;
        }
        return false;
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool DeleteData(long id)
    {
        return BaseDeleteDataWithLeft("id", "link_id", id + "");
    }
}