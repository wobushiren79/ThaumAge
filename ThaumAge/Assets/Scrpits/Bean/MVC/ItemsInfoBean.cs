/*
* FileName: ItemsInfo 
* Author: AppleCoffee 
* CreateTime: 2021-05-31-15:59:03 
*/

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class ItemsInfoBean : BaseBean
{
    public string icon_key;

    public int items_type;
    public int type_id; //关联类型的ID
    public int max_number;  //最大格子数量
    public string model_name;//模型名字
    public string tex_name;//贴图名字
    public int life;//耐久度

    public float cd_use;//使用间隔

    public int atk;//攻击力

    public string range_damage;//伤害范围 长宽高

    public string anim_use;//使用动画

    public string hold_data;//拿 数据
    public ItemsTypeEnum GetItemsType()
    {
        return (ItemsTypeEnum)items_type;
    }

    /// <summary>
    /// 获取名字列表
    /// </summary>
    /// <param name="listCharacterInfo"></param>
    /// <returns></returns>
    public static List<string> GetNameList(List<ItemsInfoBean> listInfo)
    {
        List<string> listName = new List<string>(listInfo.Count);

        for (int i = 0; i < listInfo.Count; i++)
        {
            ItemsInfoBean itemData = listInfo[i];
            listName.Add(itemData.name);
        }

        return listName;
    }


    /// <summary>
    /// 获取伤害范围
    /// </summary>
    /// <param name="length"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public void GetRangeDamage(out float length, out float width, out float height)
    {
        length = 0;
        width = 0;
        height = 0;

        float[] data = range_damage.SplitForArrayFloat(',');
        if (data.Length >= 1)
        {
            length = data[0];
        }
        if (data.Length >= 2)
        {
            width = data[1];
        }
        if (data.Length >= 3)
        {
            height = data[2];
        }
    }

    /// <summary>
    /// 获取握住数据
    /// </summary>
    /// <param name="rotate"></param>
    public bool GetHoldData(out Vector3 rotate)
    {
        if (hold_data.IsNull())
        {
            rotate = Vector3.zero;
            return false;
        }
        float[] rotateData = hold_data.SplitForArrayFloat(',');
        rotate = new Vector3(rotateData[0], rotateData[1], rotateData[2]);
        return true;
    }
}