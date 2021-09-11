/*
* FileName: CharacterInfo 
* Author: AppleCoffee 
* CreateTime: 2021-07-21-17:20:11 
*/

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class CharacterInfoBean : BaseBean
{
    public long link_id;
    public string model_name;
    public string name_cn;
    public string name_en;
    
    public string name
    {
        get
        {
            GameConfigBean gameConfig = GameDataHandler.Instance.manager.GetGameConfig();
            switch (gameConfig.GetLanguage())
            {
                case LanguageEnum.cn:
                    return name_cn;
                case LanguageEnum.en:
                    return name_en;
            }
            return name_en;
        }
    }

    /// <summary>
    /// 获取名字列表
    /// </summary>
    /// <param name="listCharacterInfo"></param>
    /// <returns></returns>
    public static List<string> GetNameList(List<CharacterInfoBean> listCharacterInfo)
    {
        List<string> listName = new List<string>(listCharacterInfo.Count);

        for (int i = 0; i < listCharacterInfo.Count; i++)
        {
            CharacterInfoBean itemData = listCharacterInfo[i];
            listName.Add(itemData.name);
        }

        return listName;
    }
}