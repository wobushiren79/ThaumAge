using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class BlockMetaArcaneAlembic : BlockMetaBase
{
    public NumberBean elementalData;
    public int maxElemental;

    public BlockMetaArcaneAlembic()
    {
        maxElemental = 25;
    }

    public bool AddElemental(ElementalTypeEnum elementalType, int elementalNum)
    {
        if (elementalData == null)
            elementalData = new NumberBean((int)elementalType, 0);
        if (elementalData.number == 0)
        {
            elementalData.id = (int)elementalType;
            elementalData.number = elementalNum;
            return true;
        }
        if (elementalData.id == (int)elementalType && (elementalData.number + elementalNum) <= maxElemental)
        {
            elementalData.number += elementalNum;
            return true;
        }
        if(elementalData.number > maxElemental)
        {
            elementalData.number = maxElemental;
        }
        else if (elementalData.number < 0)
        {
            elementalData.number = 0;
        }
        return false;
    }
}