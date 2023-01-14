using UnityEditor;
using UnityEngine;

public class BlockMetaWardedJar : BlockMetaBase
{
    public int elementalType = 1;
    public int curElemental = 0;
    public int maxElemental = 100;
    
    public bool  CheckCanAdd(ElementalTypeEnum elementalType)
    {
        //种类不同不能添加
        if (curElemental != 0 && this.elementalType != (int)elementalType)
        {
            return false;
        }
        return true;
    }

    public bool AddElemental(ElementalTypeEnum elementalType,int elementalNum, out int leftElementalNum)
    {
        leftElementalNum = elementalNum;
        //种类不同不能添加
        if (curElemental != 0 && this.elementalType != (int)elementalType)
        {
            return false;
        }
        this.elementalType = (int)elementalType;
        curElemental += elementalNum;
        if (curElemental > maxElemental)
        {
            leftElementalNum = curElemental - maxElemental;
            curElemental = maxElemental;
        }
        else 
        {
            leftElementalNum = 0;
        }
        return true;
    }

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