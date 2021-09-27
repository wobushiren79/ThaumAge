using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class BaseBean 
{
    public int id;//id
    public int valid;//是否有效

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
}