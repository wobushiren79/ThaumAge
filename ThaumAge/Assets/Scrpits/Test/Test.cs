using Pathfinding;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Test : BaseMonoBehaviour
{
    public void Start()
    {
        Vector3Int pos = new Vector3Int(-, 0, -8);
        int posX = (int)Mathf.Floor(pos.x / 16f);

        int posZ = (int)Mathf.Floor(pos.z / 16f);
        LogUtil.Log(posX + " " + posZ);
    }
    
}
