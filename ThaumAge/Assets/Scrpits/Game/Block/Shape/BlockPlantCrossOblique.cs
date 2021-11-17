using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockPlantCrossOblique : BlockCrossOblique
{
    public BlockPlantCrossOblique() : base()
    {
        //往下偏移的位置
        float offsetY = -1f / 16f;
        for (int i = 0; i < vertsAdd.Length; i++)
        {
            vertsAdd[i] = vertsAdd[i].AddY(offsetY);
        }
    }
}