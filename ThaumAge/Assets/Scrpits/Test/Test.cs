using Pathfinding;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Test : BaseMonoBehaviour
{
    public int x;
    public int y;
    public void Start()
    {

    }

    public void Test1()
    {
        Vector3Int pos = new Vector3Int(x, 0, y);
        int posX;
        if (pos.x < 0)
        {
             posX = Mathf.FloorToInt((pos.x - 7) / 16) * 16;
        }
        else
        {
             posX = Mathf.FloorToInt((pos.x + 8) / 16) * 16;
        }

        int posZ = Mathf.FloorToInt((pos.z - 7) / 16) * 16;
        LogUtil.Log("pos:" + posX);
    }

    public void OnGUI()
    {
        if (GUI.Button(new Rect(new Vector2(0,0),new Vector2(50,50)), "测试"))
        {
            Test1();
        }
    }
}
