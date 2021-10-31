/*
* FileName: BiomeInfo 
* Author: AppleCoffee 
* CreateTime: 2021-03-18-17:53:13 
*/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BiomeInfoService : BaseMVCService
{
    public BiomeInfoService() : base("biome_info", "biome_info_details_" + GameDataHandler.Instance.manager.GetGameConfig().language)
    {

    }

    /// <summary>
    /// 查询所有数据
    /// </summary>
    /// <returns></returns>
    public List<BiomeInfoBean> QueryAllData()
    {
        List<BiomeInfoBean> listData = BaseQueryAllData<BiomeInfoBean>();
        return listData;
    }

    /// <summary>
    /// 查询数据
    /// </summary>
    /// <returns></returns>
    public BiomeInfoBean QueryData()
    {
        return null;
    }

    /// <summary>
    /// 通过ID查询数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<BiomeInfoBean> QueryDataById(long id)
    {
        return BaseQueryData<BiomeInfoBean>("id", $"{id}");
    }

    /// <summary>
    /// 根据ID查询数据
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public List<BiomeInfoBean> QueryDataByIds(long[] ids)
    {
        string values = TypeConversionUtil.ArrayToStringBySplit(ids, ",");
        return BaseQueryData<BiomeInfoBean>("id", "IN", $"({values})");
    }

    /// <summary>
    /// 通过名字查询
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public List<BiomeInfoBean> QueryDataByName(LanguageEnum language, string name)
    {
        return BaseQueryData<BiomeInfoBean>($"name_{language.GetEnumName()}", $"'{name}'");
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool UpdateData(BiomeInfoBean data)
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