using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockMetaCrucible : BlockMetaBase
{
    public int waterLevel;
    public List<NumberBean> listElemental;


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