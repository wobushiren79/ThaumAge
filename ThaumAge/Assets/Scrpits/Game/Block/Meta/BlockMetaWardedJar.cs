using UnityEditor;
using UnityEngine;

public class BlockMetaWardedJar : BlockMetaBase
{
    public int elementalType = 1;
    public int curElemental = 0;
    public int maxElemental = 100;
    
    /// <summary>
    /// 获取元素类型
    /// </summary>
    /// <returns></returns>
    public ElementalTypeEnum GetElementalType()
    {
        return (ElementalTypeEnum)elementalType;
    }

    /// <summary>
    /// 获取元素进度
    /// </summary>
    /// <returns></returns>
    public float GetElementalPro()
    {
        if (maxElemental == 0)
            return 0;
        return curElemental / (float)maxElemental;
    }
}