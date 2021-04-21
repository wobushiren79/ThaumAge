using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using UnityEngine;

public class Test : BaseMonoBehaviour
{
    private void Start()
    {
        int seed = int.MaxValue/4;

        WorldRandTools.Randomize(seed);
        Stopwatch stopwatch = TimeUtil.GetMethodTimeStart();
        for (int x = -100; x <= 100; x++)
        {
            for (int z = -100; z <= 100; z++)
            {
                RandomTools randomTools = RandomUtil.GetRandom(seed, x, 1, z);
                for (int i = 0; i < 100; i++)
                {
                    int data = randomTools.NextInt(2);
                }

                //if (data == 0)
                //{
                //    GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                //    obj.transform.position = new Vector3(x, 0, z);
                //}

            }
        }
        TimeUtil.GetMethodTimeEnd("1", stopwatch);
        stopwatch = TimeUtil.GetMethodTimeStart();
        for (int x = -100; x <= 100; x++)
        {
            for (int z = -100; z <= 100; z++)
            {
                for (int i=0;i<100;i++)
                {
                    float data = WorldRandTools.GetValue(new Vector3(x, 0, z));
                }
            }
        }
        TimeUtil.GetMethodTimeEnd("2", stopwatch);
    }


}
