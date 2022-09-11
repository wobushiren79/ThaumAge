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
    public string icon_color;

    public int items_type;
    public int type_id; //关联类型的ID
    public int max_number;  //最大格子数量

    public string model_name;//模型名字
    public string model_remark_name;//备用模型名字

    public string tex_name;//贴图名字
    public float cd_use;//使用间隔

    public string attribute_data;//属性数据
    public string range_damage;//伤害范围 长(距离目标的距离) 宽（攻击宽度）高（攻击高度）
    public string anim_use;//使用动画
    public string hold_data;//拿住道具的数据（位置旋转等）
    public string link_class;//关联的类（针对有对应类 需要单独处理的特殊道具）
    public string fire_items;//道具被火烧之后的数据（道具ID,数量,时间）

    public string sound_use;//道具使用声音

    public int elemental_metal;    //元素
    public int elemental_wood;
    public int elemental_water;
    public int elemental_fire;
    public int elemental_earth;

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
            listName.Add(itemData.GetName());
        }

        return listName;
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

    /// <summary>
    /// 获取烧制的物品
    /// </summary>
    public void GetFireItems(out int[] fireItemsId, out int[] fireItemsNum, out int[] fireTime)
    {
        fireItemsId = null;
        fireItemsNum = null;
        fireTime = null;
        if (fire_items.IsNull())
            return;
        string[] itemsDataStr = fire_items.SplitForArrayStr('|');
        fireItemsId = new int[itemsDataStr.Length];
        fireItemsNum = new int[itemsDataStr.Length];
        fireTime = new int[itemsDataStr.Length];

        for (int i = 0; i < itemsDataStr.Length; i++)
        {
            int[] itemData = itemsDataStr[i].SplitForArrayInt(',');
            fireItemsId[i] = itemData[0];
            fireItemsNum[i] = itemData[1];
            fireTime[i] = itemData[2];
        }
    }

    /// <summary>
    /// 获取备用模型名字
    /// </summary>
    /// <param name="path"></param>
    public List<string> GetModelRemarkName(string path)
    {
        List<string> listName = new List<string>();
        if (model_remark_name.IsNull())
            return listName;
        string[] arrayName = model_remark_name.Split("|");
        for (int i = 0; i < arrayName.Length; i++)
        {
            var itemData = arrayName[i];
            listName.Add($"{path}/{itemData}.prefab");
        }
        return listName;
    }

    protected DamageBean damageData;//伤害数据
    /// <summary>
    /// 获取伤害数据
    /// </summary>
    /// <returns></returns>
    public DamageBean GetDamageData()
    {
        if (damageData == null)
        {
            damageData = new DamageBean(attribute_data);
        }
        return damageData;
    }

    protected AttributeBean attributeData;
    /// <summary>
    /// 获取属性数据
    /// </summary>
    /// <returns></returns>
    public AttributeBean GetAttributeData()
    {
        if (attributeData == null)
        {
            attributeData = new AttributeBean(attribute_data);
        }
        return attributeData;
    }
   
}