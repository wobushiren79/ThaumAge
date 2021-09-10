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

    protected string tableNameEyeForMain;

    protected string tableNameMouthForMain;

    public CharacterInfoService() : base("", "")
    {
        tableNameHairForMain = "character_info_hair";

        tableNameEyeForMain = "character_info_eye";

        tableNameMouthForMain = "character_info_mouth";
    }

    /// <summary>
    /// 查询所有数据
    /// </summary>
    /// <returns></returns>
    public List<CharacterInfoBean> QueryAllHairData()
    {
        return QueryAllData<CharacterInfoBean>(tableNameHairForMain);
    }
    public List<CharacterInfoBean> QueryAllEyeData()
    {
        return QueryAllData<CharacterInfoBean>(tableNameEyeForMain);
    }
    public List<CharacterInfoBean> QueryAllMouthData()
    {
        return QueryAllData<CharacterInfoBean>(tableNameMouthForMain);
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool UpdateHairData(CharacterInfoBean data)
    {
        return UpdateData(tableNameHairForMain, data);
    }
    public bool UpdateEyeData(CharacterInfoBean data)
    {
        return UpdateData(tableNameEyeForMain, data);
    }
    public bool UpdateMouthData(CharacterInfoBean data)
    {
        return UpdateData(tableNameMouthForMain, data);
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool DeleteHairData(long id)
    {
        return DeleteData(tableNameHairForMain, id);
    }
    public bool DeleteEyeData(long id)
    {
        return DeleteData(tableNameEyeForMain, id);
    }
    public bool DeleteMouthData(long id)
    {
        return DeleteData(tableNameMouthForMain, id);
    }

    protected List<T> QueryAllData<T>(string tableNameForMain)
    {
        this.tableNameForMain = tableNameForMain;
        List<T> listData = BaseQueryAllData<T>();
        return listData;
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    protected bool UpdateData<T>(string tableNameForMain, T data) where T : BaseBean
    {
        this.tableNameForMain = tableNameForMain;
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
    protected bool DeleteData(string tableNameForMain, long id)
    {
        this.tableNameForMain = tableNameForMain;
        return BaseDeleteDataById(id);
    }
}