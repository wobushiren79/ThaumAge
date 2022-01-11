using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;

public class Test : BaseMonoBehaviour
{
    public float angle = 0;
    private void OnGUI()
    {
        if (GUILayout.Button("Test"))
        {
            TestM();
        }
    }

    public void TestM()
    {
       Vector2[] arrayData= VectorUtil.GetListCirclePosition(36,new Vector2(10,0),Vector2.zero,0);
        foreach (var itemData in arrayData)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            obj.transform.position = itemData;
        }

    }
}
