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
    public List<CharacterInfoHairBean> GetAllCharacterInfoHairData()
    {
        List<CharacterInfoHairBean> listData = serviceCharacterInfo.QueryAllHairData();
        return listData;
    }

    /// <summary>
    /// 保存游戏数据
    /// </summary>
    /// <param name="data"></param>
    public void SetCharacterInfoHairData(CharacterInfoHairBean data)
    {
        serviceCharacterInfo.UpdateHairData(data);
    }

}