using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public class BlockMetaBaseLink : BlockMetaBase
{
    public int level;
    public Vector3IntBean linkBasePosition;

    public Vector3Int GetBasePosition()
    {
        return linkBasePosition.GetVector3Int();
    }
}