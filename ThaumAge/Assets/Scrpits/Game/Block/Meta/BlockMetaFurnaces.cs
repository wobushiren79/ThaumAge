using UnityEditor;
using UnityEngine;
using System;

[Serializable]
public class BlockMetaFurnaces : BlockMetaItemsTransition
{
    //当前剩余烧制时间
    public int fireTimeRemain;
    //最大存储烧制时间
    public int fireTimeMax;

    //火能源 来源
    public int itemFireSourceId;
    public int itemFireSourceNum;

    public BlockMetaFurnaces()
    {
        fireTimeMax = 100;
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
}