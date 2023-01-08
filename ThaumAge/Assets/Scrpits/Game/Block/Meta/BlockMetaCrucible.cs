using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockMetaCrucible : BlockMetaBase
{
    public int waterLevel;
    public List<NumberBean> listElemental;

    /// <summary>
    /// 增加水
    /// </summary>
    /// <param name="addLevel"></param>
    public void AddWater(int addLevel)
    {
        waterLevel+= addLevel;
        if (waterLevel < 0)
            waterLevel = 0;
        if (waterLevel > 5)
            waterLevel = 5;
    }

    /// <summary>
    /// 减少元素
    /// </summary>
    /// <param name="dicSubElemental"></param>
    public void SubElemental(Dictionary<ElementalTypeEnum, int> dicSubElemental,int number = 1)
    {
        foreach (var itemData in dicSubElemental)
        {
            for (int i = 0; i < listElemental.Count; i++)
            {
                var itemHasElemental = listElemental[i];
                if (itemHasElemental.id == (long)itemData.Key)
                {
                    itemHasElemental.number -= (itemData.Value * number);
                    if (itemHasElemental.number < 0)
                        itemHasElemental.number = 0;
                    if (itemHasElemental.number == 0)
                    {
                        listElemental.Remove(itemHasElemental);
                    }
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 增加元素
    /// </summary>
    /// <param name="addListElemental"></param>
    public void AddElemental(List<NumberBean> addListElemental)
    {
        if (listElemental.IsNull())
        {
            listElemental = new List<NumberBean>();
            listElemental.AddRange(addListElemental);
        }
        else
        {
            for (int i = 0; i < addListElemental.Count; i++)
            {
                var itemDataNew = addListElemental[i];
                bool hasSameElemental = false;
                for (int f = 0; f < listElemental.Count; f++)
                {
                    var itemDataOld = listElemental[f];
                    if (itemDataOld.id == itemDataNew.id)
                    {
                        itemDataOld.number += itemDataNew.number;
                        hasSameElemental = true;
                        break;
                    }
                }
                if (!hasSameElemental)
                {
                    listElemental.Add(itemDataNew);
                }
            }
        }
    }
}