using Pathfinding;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Test : BaseMonoBehaviour
{
    public int randomSeed;
    public void Start()
    {

    }

    public void Test1()
    {
        System.Random random = new System.Random(randomSeed);
        int randomNumber1 = random.Next(0, 1000);
        int randomNumber2 = random.Next(0, 100);
        int randomNumber3 = random.Next(0, 10);
        int randomNumber4 = random.Next(0, 10000);
        int randomNumber5 = random.Next(0, 100000);

        LogUtil.Log(randomNumber1 + " " + randomNumber2 + " " + randomNumber3 + " " + randomNumber4 + " " + randomNumber5);
    }

    public void OnGUI()
    {
        if (GUI.Button(new Rect(new Vector2(0, 0), new Vector2(50, 50)), "测试"))
        {
            Test1();
        }
    }
}
