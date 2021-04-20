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
        int seed = int.MaxValue/2;



        //for (int x = -100; x <= 100; x++)
        //{
        //    for (int z = -100; z <= 100; z++)
        //    {
        //        Stopwatch stopwatch = TimeUtil.GetMethodTimeStart();
        //        RandomTools randomTools = RandomUtil.GetRandom(seed, x, 1, z);
        //        TimeUtil.GetMethodTimeEnd("1", stopwatch);

        //        stopwatch = TimeUtil.GetMethodTimeStart();
        //        int data = randomTools.NextInt(2);
        //        TimeUtil.GetMethodTimeEnd("2", stopwatch);

        //        //if (data == 0)
        //        //{
        //        //    GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //        //    obj.transform.position = new Vector3(x, 0, z);
        //        //}

        //    }
        //}

        for (int x = -10; x <= 10; x++)
        {
            for (int z = -10; z <= 10; z++)
            {
                Stopwatch stopwatch = TimeUtil.GetMethodTimeStart();
                string dataRandomStr = (seed.ToString() + x.ToString() + z.ToString());
                TimeUtil.GetMethodTimeEnd("1", stopwatch);

                stopwatch = TimeUtil.GetMethodTimeStart();
                int dataRandom = dataRandomStr.GetHashCode();
                TimeUtil.GetMethodTimeEnd("2", stopwatch);

                stopwatch = TimeUtil.GetMethodTimeStart();
                System.Random random = new System.Random(dataRandom);
                TimeUtil.GetMethodTimeEnd("3", stopwatch);

                stopwatch = TimeUtil.GetMethodTimeStart();
                int data = random.Next(0, 2);
                TimeUtil.GetMethodTimeEnd("4", stopwatch);

                if (data == 0)
                {
                    GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    obj.transform.position = new Vector3(x, 0, z);
                }

            }
        }

    }


}
