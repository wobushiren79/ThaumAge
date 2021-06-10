using UnityEditor;
using UnityEngine;
using System;

[Serializable]
public class BuildingBean 
{
    public int blockId;
    public int direction;
    public Vector3IntBean position;

    public Vector3Int GetPosition()
    {
        return position.GetVector3Int();
    }


}