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
}