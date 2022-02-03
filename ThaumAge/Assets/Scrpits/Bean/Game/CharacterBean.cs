using UnityEditor;
using UnityEngine;
using System;

[Serializable]
public class CharacterBean : CreatureBean
{
    //角色姓名
    public string characterName;
    //角色性别
    public int characterSex;

    //衣服ID
    public long clothesId;

    public long hairId;
    public long eyeId;
    public long mouthId;

    public ColorBean colorHair;
    public ColorBean colorSkin;

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
}