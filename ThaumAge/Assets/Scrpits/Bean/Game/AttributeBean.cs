using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AttributeBean 
{
    public Dictionary<AttributeTypeEnum, int> dicAttributeData;

    public AttributeBean(string dataStr)
    {
        dicAttributeData = new Dictionary<AttributeTypeEnum, int>();
        string[] itemDataStr = dataStr.SplitForArrayStr('|');
        for (int i = 0; i < itemDataStr.Length; i++)
        {
            string[] itemDetailsDataStr = itemDataStr[i].SplitForArrayStr(':');
            AttributeTypeEnum damageAdditionEnum = EnumExtension.GetEnum<AttributeTypeEnum>(itemDetailsDataStr[0]);
            dicAttributeData.Add(damageAdditionEnum, int.Parse(itemDetailsDataStr[1]));
        }
    }

    /// <summary>
    /// 获取属性图标
    /// </summary>
    public static string GetAttributeIconKey(AttributeTypeEnum attributeType)
    {
        switch (attributeType) 
        {
            case AttributeTypeEnum.Health:
                return "ui_life_1";
            case AttributeTypeEnum.Stamina:
                return "ui_life_2";
            case AttributeTypeEnum.Magic:
                return "ui_life_3";
            case AttributeTypeEnum.Saturation:
                return "ui_life_4";
            case AttributeTypeEnum.Air:
                return "ui_life_5";

            case AttributeTypeEnum.Damage:
                return "ui_attack_1";
            case AttributeTypeEnum.DamageMagic:
                return "ui_attack_4";
            case AttributeTypeEnum.KnockbackDis:
                return "ui_attack_2";
            case AttributeTypeEnum.KnockbackTime:
                return "ui_attack_3";

            case AttributeTypeEnum.Def:
                return "ui_def_1";
            case AttributeTypeEnum.DefMagic:
                return "ui_def_2";
            case AttributeTypeEnum.DefMetal:
                return "ui_def_3";
            case AttributeTypeEnum.DefWood:
                return "ui_def_4";
            case AttributeTypeEnum.DefWater:
                return "ui_def_5";
            case AttributeTypeEnum.DefFire:
                return "ui_def_6";
            case AttributeTypeEnum.DefEarth:
                return "ui_def_7";
        }
        return "";
    }

    /// <summary>
    /// 获取属性文本
    /// </summary>
    public static string GetAttributeText(AttributeTypeEnum attributeType)
    {
        switch (attributeType)
        {
            case AttributeTypeEnum.Health:
                return TextHandler.Instance.GetTextById(2001);
            case AttributeTypeEnum.Stamina:
                return TextHandler.Instance.GetTextById(2002);
            case AttributeTypeEnum.Magic:
                return TextHandler.Instance.GetTextById(2003);
            case AttributeTypeEnum.Saturation:
                return TextHandler.Instance.GetTextById(2004);
            case AttributeTypeEnum.Air:
                return TextHandler.Instance.GetTextById(2005);

            case AttributeTypeEnum.Damage:
                return TextHandler.Instance.GetTextById(2013);
            case AttributeTypeEnum.DamageMagic:
                return TextHandler.Instance.GetTextById(2016);
            case AttributeTypeEnum.KnockbackDis:
                return TextHandler.Instance.GetTextById(2014);
            case AttributeTypeEnum.KnockbackTime:
                return TextHandler.Instance.GetTextById(2015);

            case AttributeTypeEnum.Def:
                return TextHandler.Instance.GetTextById(2006);
            case AttributeTypeEnum.DefMagic:
                return TextHandler.Instance.GetTextById(2007);
            case AttributeTypeEnum.DefMetal:
                return TextHandler.Instance.GetTextById(2008);
            case AttributeTypeEnum.DefWood:
                return TextHandler.Instance.GetTextById(2009);
            case AttributeTypeEnum.DefWater:
                return TextHandler.Instance.GetTextById(2010);
            case AttributeTypeEnum.DefFire:
                return TextHandler.Instance.GetTextById(2011);
            case AttributeTypeEnum.DefEarth:
                return TextHandler.Instance.GetTextById(2012);
        }
        return "";
    }
}