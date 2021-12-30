using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;

public class Test : BaseMonoBehaviour
{
    public int seed = 123;
    public int number = 123;
    public Vector3 startPosition;
    private void OnGUI()
    {
        if (GUILayout.Button("Test"))
        {
            TestM();
        }
    }

    public void TestM()
    {
        transform.DestroyAllChild();
        int count = number;

        Vector3 movePosition = startPosition;
        FastNoise fastNoise = new FastNoise(seed);
        for (int i = 0; i < count; i++)
        {
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            gameObject.transform.position = movePosition;
            gameObject.transform.SetParent(transform);

            float[] dataArray = new float[3];
            dataArray[0] = fastNoise.GetPerlin(movePosition.x, movePosition.y, 0) * 10;
            dataArray[1] = fastNoise.GetPerlin(0, movePosition.y, movePosition.z) * 10;
            dataArray[2] = fastNoise.GetPerlin(movePosition.x, 0, movePosition.z) * 10;
            //dataArray[0] = SimplexNoiseUtil.Generate(movePosition.x, movePosition.y, 0) * 2;
            //dataArray[1] = SimplexNoiseUtil.Generate(0, movePosition.y, movePosition.z) * 2;
            //dataArray[2] = SimplexNoiseUtil.Generate(movePosition.x, 0, movePosition.z) * 2;
            movePosition += new Vector3(dataArray[1], dataArray[2], dataArray[0]);

        }
    }
}
