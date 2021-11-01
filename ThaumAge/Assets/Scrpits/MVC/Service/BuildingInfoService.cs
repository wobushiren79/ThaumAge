/*
* FileName: BuildingInfo 
* Author: AppleCoffee 
* CreateTime: 2021-06-08-14:40:47 
*/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BuildingInfoService : BaseMVCService
{
    public BuildingInfoService() : base("building_info", "")
    {

    }

    /// <summary>
    /// 查询所有数据
    /// </summary>
    /// <returns></returns>
    public List<BuildingInfoBean> QueryAllData()
    {
        List<BuildingInfoBean> listData = BaseQueryAllData<BuildingInfoBean>();
        return listData;
    }

    /// <summary>
    /// 查询数据
    /// </summary>
    /// <returns></returns>
    public BuildingInfoBean QueryData()
    {
        return null;
    }

    /// <summary>
    /// 通过ID查询数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<BuildingInfoBean> QueryDataById(long id)
    {
        return BaseQueryData<BuildingInfoBean>("link_id", "id", $"{id}");
    }

    /// <summary>
    /// 根据ID查询数据
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public List<BuildingInfoBean> QueryDataByIds(long[] ids)
    {
        string values = TypeConversionUtil.ArrayToStringBySplit(ids, ",");
        return BaseQueryData<BuildingInfoBean>("id", "IN", $"({values})");
    }

    /// <summary>
    /// 通过名字查询
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public List<BuildingInfoBean> QueryDataByName(string name)
    {
        return BaseQueryData<BuildingInfoBean>("name", $"'{name}'");
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool UpdateData(BuildingInfoBean data)
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
        return BaseDeleteData("id", $"{id}");
    }
}