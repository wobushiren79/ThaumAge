using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockShapePlantCross : BlockShapeCross
{
    public BlockShapePlantCross() : base()
    {
        //往下偏移的位置
        float offsetY = -1f / 16f;
        for (int i = 0; i < vertsAdd.Length; i++)
        {
            vertsAdd[i] = vertsAdd[i].AddY(offsetY);
        }
    }
}