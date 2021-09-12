/*
* FileName: CharacterInfo 
* Author: AppleCoffee 
* CreateTime: 2021-07-21-17:20:11 
*/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CharacterInfoModel : BaseMVCModel
{
    protected CharacterInfoService serviceCharacterInfo;

    public override void InitData()
    {
        serviceCharacterInfo = new CharacterInfoService();
    }

    /// <summary>
    /// 获取所有数据
    /// </summary>
    /// <returns></returns>
    public List<CharacterInfoBean> GetAllCharacterInfoHairData()
    {
        List<CharacterInfoBean> listData = serviceCharacterInfo.QueryAllHairData();
        return listData;
    }
    public List<CharacterInfoBean> GetAllCharacterInfoEyeData()
    {
        List<CharacterInfoBean> listData = serviceCharacterInfo.QueryAllEyeData();
        return listData;
    }
    public List<CharacterInfoBean> GetAllCharacterInfoMouthData()
    {
        List<CharacterInfoBean> listData = serviceCharacterInfo.QueryAllMouthData();
        return listData;
    }
    public List<CharacterInfoBean> GetAllCharacterInfoSkinData()
    {
        List<CharacterInfoBean> listData = serviceCharacterInfo.QueryAllSkinData();
        return listData;
    }

    /// <summary>
    /// 保存游戏数据
    /// </summary>
    /// <param name="data"></param>
    public void SetCharacterInfoHairData(CharacterInfoBean data)
    {
        serviceCharacterInfo.UpdateHairData(data);
    }
    public void SetCharacterInfoEyeData(CharacterInfoBean data)
    {
        serviceCharacterInfo.UpdateEyeData(data);
    }
    public void SetCharacterInfoMouthData(CharacterInfoBean data)
    {
        serviceCharacterInfo.UpdateMouthData(data);
    }
}