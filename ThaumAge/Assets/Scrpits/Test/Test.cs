using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.AI;

public class Test : BaseMonoBehaviour
{
    public int widthChunk = 16;
    public Vector3Int vector3;
    private void Start()
    {

    }

    private void OnGUI()
    {
        if (GUILayout.Button("test"))
        {
          LogUtil.Log(GetChunkPositionForWorldPosition(vector3)+"");
        }
    }
    public Vector3Int GetChunkPositionForWorldPosition(Vector3Int pos)
    {
        int posX;
        int posZ;
        posX = Mathf.FloorToInt((float)pos.x / widthChunk) * widthChunk;
        posZ = Mathf.FloorToInt((float)pos.z / widthChunk) * widthChunk;
        return new Vector3Int(posX, 0, posZ);
    }
}
