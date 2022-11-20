using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class ElementalInfoBean
{
 
}

public partial class ElementalInfoCfg
{
    /// <summary>
    /// 获取元素信息
    /// </summary>
    /// <param name="elementalType"></param>
    /// <returns></returns>
    public static ElementalInfoBean GetItemData(ElementalTypeEnum elementalType)
    {
        return GetItemData((long)elementalType);
    }

    /// <summary>
    /// 通过元素等级 获取元素
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public static List<ElementalInfoBean> GetItemDataByLevel(int level)
    {
        List<ElementalInfoBean> listData = new List<ElementalInfoBean>();
        var dicData = GetAllData();
        foreach (var itemData in dicData)
        {
            var elementItemData = itemData.Value;
            if (elementItemData.level == level)
            {
                listData.Add(elementItemData);
            }
        }
        return listData;
    }
}