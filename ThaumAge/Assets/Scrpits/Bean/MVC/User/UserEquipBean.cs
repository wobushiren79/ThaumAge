using UnityEditor;
using UnityEngine;
using System;

[Serializable]
public class UserEquipBean
{
    //帽子
    public ItemsBean hats;
    //手套
    public ItemsBean gloves;
    //衣服
    public ItemsBean clothes;
    //鞋子
    public ItemsBean shoes;
    //裤子
    public ItemsBean trousers;
    //头饰
    public ItemsBean headwear;
    //左手戒指
    public ItemsBean leftRing;
    //右手戒指
    public ItemsBean rightRing;
    //斗篷
    public ItemsBean cape;


    /// <summary>
    /// 通过道具类型获取身上的装备
    /// </summary>
    /// <param name="itemsType"></param>
    /// <param name="pos">1左 2右</param>
    /// <returns></returns>
    public ItemsBean GetEquipByType(EquipTypeEnum equipType)
    {
        ItemsBean targetItem = null;
        switch (equipType)
        {
            case EquipTypeEnum.Hats:
                targetItem = hats;
                break;
            case EquipTypeEnum.Gloves:
                targetItem = gloves;
                break;
            case EquipTypeEnum.Clothes:
                targetItem = clothes;
                break;
            case EquipTypeEnum.Shoes:
                targetItem = shoes;
                break;
            case EquipTypeEnum.Trousers:
                targetItem = trousers;
                break;
            case EquipTypeEnum.Headwear:
                targetItem = headwear;
                break;
            case EquipTypeEnum.LeftRing:
                targetItem = leftRing;
                break;
            case EquipTypeEnum.RightRing:
                targetItem = rightRing;
                break;
            case EquipTypeEnum.Cape:
                targetItem = cape;
                break;
        }
        if (targetItem == null)
            targetItem = new ItemsBean();
        return targetItem;
    }

    public static ItemsTypeEnum EquipTypeEnumToItemsType(EquipTypeEnum equipType)
    {
        switch (equipType)
        {
            case EquipTypeEnum.Hats:
                return ItemsTypeEnum.Hats;
            case EquipTypeEnum.Gloves:
                return ItemsTypeEnum.Gloves;
            case EquipTypeEnum.Clothes:
                return ItemsTypeEnum.Clothes;
            case EquipTypeEnum.Shoes:
                return ItemsTypeEnum.Shoes;
            case EquipTypeEnum.Trousers:
                return ItemsTypeEnum.Trousers;
            case EquipTypeEnum.Headwear:
                return ItemsTypeEnum.Headwear;
            case EquipTypeEnum.LeftRing:
            case EquipTypeEnum.RightRing:
                return ItemsTypeEnum.Ring;
            case EquipTypeEnum.Cape:
                return ItemsTypeEnum.Cape;
        }
        return ItemsTypeEnum.None;
    }

    public static string GetEquipName(EquipTypeEnum equipType)
    {
        long textId = 0;
        switch (equipType)
        {
            case EquipTypeEnum.Hats:
                textId = 401;
                break;
            case EquipTypeEnum.Clothes:
                textId = 402;
                break;
            case EquipTypeEnum.Gloves:
                textId = 403;
                break;
            case EquipTypeEnum.Shoes:
                textId = 404;
                break;
            case EquipTypeEnum.Trousers:
                textId = 409;
                break;
            case EquipTypeEnum.Headwear:
                textId = 405;
                break;
            case EquipTypeEnum.LeftRing:
                textId = 406;
                break;
            case EquipTypeEnum.RightRing:
                textId = 407;
                break;
            case EquipTypeEnum.Cape:
                textId = 408;
                break;
        }
        return TextHandler.Instance.GetTextById(textId);
    }
}