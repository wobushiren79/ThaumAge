using UnityEditor;
using UnityEngine;
using System;

[Serializable]
public class CharacterBean : CreatureBean
{
    //角色状态
    public CharacterStatusBean characterStatus;
    //角色装备
    public CharacterEquipBean characterEquip;

    //角色姓名
    public string characterName;
    //角色性别
    public int characterSex;

    public long hairId;
    public long eyeId;
    public long mouthId;

    public ColorBean colorHair;
    public ColorBean colorSkin;

    /// <summary>
    /// 获取属性值
    /// </summary>
    /// <param name="attributeType"></param>
    /// <returns></returns>
    public int GetAttributeValue(AttributeTypeEnum attributeType)
    {
        int totalData = 0;
        totalData += characterEquip.GetAttributeTotal(attributeType);
        switch (attributeType)
        {
            case AttributeTypeEnum.Health:
                totalData += characterStatus.health;
                break;
            case AttributeTypeEnum.Stamina:
                totalData += characterStatus.stamina;
                break;
            case AttributeTypeEnum.Magic:
                totalData += characterStatus.magic;
                break;
            case AttributeTypeEnum.Saturation:
                totalData += characterStatus.saturation;
                break;
            case AttributeTypeEnum.Air:
                totalData += characterStatus.air;
                break;
            case AttributeTypeEnum.Def:
                totalData += characterStatus.def;
                break;
            case AttributeTypeEnum.DefMagic:
                totalData += characterStatus.defMagic;
                break;
            case AttributeTypeEnum.DefMetal:
                totalData += characterStatus.defMetal;
                break;
            case AttributeTypeEnum.DefWood:
                totalData += characterStatus.defWooden;
                break;
            case AttributeTypeEnum.DefWater:
                totalData += characterStatus.defWater;
                break;
            case AttributeTypeEnum.DefFire:
                totalData += characterStatus.defFire;
                break;
            case AttributeTypeEnum.DefEarth:
                totalData += characterStatus.defEarth;
                break;
            case AttributeTypeEnum.Damage:
                totalData += characterStatus.damage;
                break;
            case AttributeTypeEnum.DamageMagic:
                totalData += characterStatus.damageMagic;
                break;
            case AttributeTypeEnum.KnockbackDis:
            case AttributeTypeEnum.KnockbackTime:
                break;
        }
        return totalData;
    }

    /// <summary>
    /// 设置头发颜色
    /// </summary>
    /// <param name="color"></param>
    public void SetColorHair(Color color)
    {
        colorHair = new ColorBean(color);
    }

    /// <summary>
    /// 获取头发颜色
    /// </summary>
    /// <returns></returns>
    public Color GetColorHair()
    {
        if (colorHair == null)
            colorHair = new ColorBean(1, 1, 1, 1);
        return colorHair.GetColor();
    }

    /// <summary>
    /// 设置头发原色
    /// </summary>
    /// <param name="color"></param>
    public void SetColorSkin(Color color)
    {
        colorSkin = new ColorBean(color);
    }

    /// <summary>
    /// 获取皮肤颜色
    /// </summary>
    /// <returns></returns>
    public Color GetColorSkin()
    {
        if (colorSkin == null)
            colorSkin = new ColorBean(1,1,1,1);
        return colorSkin.GetColor();
    }

    /// <summary>
    /// 设置性别
    /// </summary>
    /// <param name="sexType"></param>
    public void SetSex(SexTypeEnum sexType)
    {
        this.characterSex = (int)sexType;
    }

    /// <summary>
    /// 获取性别
    /// </summary>
    /// <returns></returns>
    public SexTypeEnum GetSex()
    {
        return (SexTypeEnum)characterSex;
    }

    /// <summary>
    /// 获取角色状态
    /// </summary>
    public CharacterStatusBean GetCharacterStatus()
    {
        if (characterStatus == null)
            characterStatus = new CharacterStatusBean();
        return characterStatus;
    }

    /// <summary>
    /// 获取角色装备信息
    /// </summary>
    /// <returns></returns>
    public CharacterEquipBean GetCharacterEquip()
    {
        if (characterEquip == null)
        {
            characterEquip = new CharacterEquipBean();
        }
        return characterEquip;
    }
}