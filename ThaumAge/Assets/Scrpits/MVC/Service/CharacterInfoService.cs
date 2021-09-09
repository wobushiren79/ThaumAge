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

    protected string tableNameEyeForMain;
    protected string tableNameEyeForLeft;

    protected string tableNameMouthForMain;
    protected string tableNameMouthForLeft;

    public CharacterInfoService() : base("", "")
    {
        GameConfigBean gameConfig = GameDataHandler.Instance.manager.GetGameConfig();

        tableNameHairForMain = "character_info_hair";
        tableNameHairForLeft = "character_info_hair_details_" + gameConfig.language;

        tableNameEyeForMain = "character_info_eye";
        tableNameEyeForLeft = "character_info_eye_details_" + gameConfig.language;

        tableNameMouthForMain = "character_info_mouth";
        tableNameMouthForLeft = "character_info_mouth_details_" + gameConfig.language;
    }

    /// <summary>
    /// 查询所有数据
    /// </summary>
    /// <returns></returns>
    public List<CharacterInfoBean> QueryAllHairData()
    {
        return QueryAllData<CharacterInfoBean>(tableNameHairForMain, tableNameHairForLeft);
    }
    public List<CharacterInfoBean> QueryAllEyeData()
    {
        return QueryAllData<CharacterInfoBean>(tableNameEyeForMain, tableNameEyeForLeft);
    }
    public List<CharacterInfoBean> QueryAllMouthData()
    {
        return QueryAllData<CharacterInfoBean>(tableNameMouthForMain, tableNameMouthForLeft);
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool UpdateHairData(CharacterInfoBean data)
    {
        return UpdateData(tableNameHairForMain, tableNameHairForLeft, data);
    }
    public bool UpdateEyeData(CharacterInfoBean data)
    {
        return UpdateData(tableNameEyeForMain, tableNameEyeForLeft, data);
    }
    public bool UpdateMouthData(CharacterInfoBean data)
    {
        return UpdateData(tableNameMouthForMain, tableNameMouthForLeft, data);
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool DeleteHairData(long id)
    {
        return DeleteData(tableNameHairForMain, tableNameHairForLeft, id);
    }
    public bool DeleteEyeData(long id)
    {
        return DeleteData(tableNameEyeForMain, tableNameEyeForLeft, id);
    }
    public bool DeleteMouthData(long id)
    {
        return DeleteData(tableNameMouthForMain, tableNameMouthForLeft, id);
    }

    protected List<T> QueryAllData<T>(string tableNameForMain, string tableNameForLeft)
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
    protected bool UpdateData<T>(string tableNameForMain, string tableNameForLeft, T data) where T : BaseBean
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
    protected bool DeleteData(string tableNameForMain, string tableNameForLeft, long id)
    {
        this.tableNameForMain = tableNameForMain;
        this.tableNameForLeft = tableNameForLeft;
        return BaseDeleteDataWithLeft("id", "link_id", id + "");
    }
}