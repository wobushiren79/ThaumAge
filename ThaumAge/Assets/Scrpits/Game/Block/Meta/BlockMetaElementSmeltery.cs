using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockMetaElementSmeltery : BlockMetaItemsTransition
{
    //当前剩余烧制时间
    public int fireTimeRemain;
    //最大存储烧制时间
    public int fireTimeMax;

    //火能源 来源
    public int itemFireSourceId;
    public int itemFireSourceNum;

    public List<int> listElemental;
    public int elementalMax;
    public BlockMetaElementSmeltery()
    {
        fireTimeMax = 200;
        elementalMax = 10;
        listElemental = new List<int>();
    }

    /// <summary>
    /// 增加剩余制烧事件
    /// </summary>
    public int AddFireTimeRemain(int addFireTimeRemain)
    {
        fireTimeRemain += addFireTimeRemain;
        if (fireTimeRemain > fireTimeMax)
        {
            fireTimeRemain = fireTimeMax;
        }
        if (fireTimeRemain < 0)
        {
            fireTimeRemain = 0;
        }
        return fireTimeRemain;
    }

    /// <summary>
    /// 增加元素
    /// </summary>
    public bool AddElemental(ElementalTypeEnum elementalType)
    {
        if (listElemental.Count < elementalMax)
        {
            listElemental.Add((int)elementalType);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 减去元素
    /// </summary>
    public bool SubElemental(out ElementalTypeEnum subElemental)
    {
        subElemental = ElementalTypeEnum.None;
        if (listElemental.IsNull())
        {
            return false;
        }
        subElemental = (ElementalTypeEnum)listElemental[0];
        listElemental.RemoveAt(0);
        return true;
    }

    /// <summary>
    /// 获取容器的进度
    /// </summary>
    /// <returns></returns>
    public float GetElementalPro()
    {
        return listElemental.Count / (float)elementalMax;
    }
}