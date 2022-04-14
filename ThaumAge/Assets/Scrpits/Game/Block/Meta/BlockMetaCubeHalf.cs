using UnityEditor;
using UnityEngine;
using System;

[Serializable]
public class BlockMetaCubeHalf : BlockMetaBase
{
    public int halfPosition;


    public DirectionEnum GetHalfPosition()
    {
        return (DirectionEnum)halfPosition;
    }

    public void SetHalfPosition(DirectionEnum halfPosition)
    {
        this.halfPosition = (int)halfPosition;
    }
}