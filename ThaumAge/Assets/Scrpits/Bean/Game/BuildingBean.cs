using UnityEditor;
using UnityEngine;
using System;

[Serializable]
public class BuildingBean 
{
    public int blockId;
    public int direction;
    public Vector3Int position;

    public Vector3Int GetPosition()
    {
        return position;
    }


}