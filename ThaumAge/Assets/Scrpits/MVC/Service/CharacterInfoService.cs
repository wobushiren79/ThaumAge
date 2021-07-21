/*
* FileName: CharacterInfo 
* Author: AppleCoffee 
* CreateTime: 2021-07-21-17:20:11 
*/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CharacterInfoService : BaseMVCService
{
    protected string tableNameHairForMain;
    protected string tableNameHairForLeft;

    public CharacterInfoService() : base("", "")
    {
        tableNameHairForMain = "character_info_hair";
        tableNameHairForLeft = "character_info_hair_detals_" + GameDataHandler.Instance.manager.GetGameConfig().language;
    }

    /// <summary>
    /// 查询所有头发数据
    /// </summary>
    /// <returns></returns>
    public List<CharacterInfoHairBean> QueryAllHairData()
    {
        return QueryAllData<CharacterInfoHairBean>(tableNameHairForMain, tableNameHairForLeft);
    }

    /// <summary>
    /// 更新头发数据
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool UpdateHairData(CharacterInfoHairBean data)
    {
        return UpdateData(tableNameHairForMain, tableNameHairForLeft, data);
    }

    /// <summary>
    /// 删除头发数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool DeleteHairData(long id)
    {
        return DeleteData(tableNameHairForMain, tableNameHairForLeft, id);
    }


    public List<T> QueryAllData<T>(string tableNameForMain, string tableNameForLeft)
    {
        this.tableNameForMain = tableNameForMain;
        this.tableNameForLeft = tableNameForLeft;
        List<T> listData = BaseQueryAllData<T>("link_id");
        return listData;
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool UpdateData<T>(string tableNameForMain, string tableNameForLeft, T data) where T : BaseBean
    {
        this.tableNameForMain = tableNameForMain;
        this.tableNameForLeft = tableNameForLeft;
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
    public bool DeleteData(string tableNameForMain, string tableNameForLeft, long id)
    {
        this.tableNameForMain = tableNameForMain;
        this.tableNameForLeft = tableNameForLeft;
        return BaseDeleteDataWithLeft("id", "link_id", id + "");
    }
}