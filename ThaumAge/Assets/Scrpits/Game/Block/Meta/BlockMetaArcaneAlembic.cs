using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class BlockMetaArcaneAlembic : BlockMetaBase
{
    public List<NumberBean> listElemental;
    public int maxElemental;

    public BlockMetaArcaneAlembic()
    {
        maxElemental = 25;
    }

    public bool AddElemental(ElementalTypeEnum elementalType, int elementalNum)
    {
        if (listElemental.IsNull())
        {
            if (listElemental == null)
                listElemental = new List<NumberBean>();
            NumberBean numberData = new NumberBean((int)elementalType, elementalNum);
            listElemental.Add(numberData);
            return true;
        }
        else
        {
            NumberBean numberData = listElemental[0];
            if(numberData.id == (int)elementalType && (numberData.number + elementalNum) <= maxElemental)
            {
                numberData.number += elementalNum;
                return true;
            }
        }
        return false;
    }
}