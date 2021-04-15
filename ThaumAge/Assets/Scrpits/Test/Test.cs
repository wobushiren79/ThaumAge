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
        Dictionary<int, string> list = new Dictionary<int, string>();
        for (int i = 0; i < 9999999; i++)
        {
            list.Add(i, "i");
        }
        //3.0以上版本
        Stopwatch stopwatch = TimeUtil.GetMethodTimeStart();
        foreach (var item in list)
        {
            string value = item.Value;
        }
        TimeUtil.GetMethodTimeEnd("1", stopwatch);
        stopwatch = TimeUtil.GetMethodTimeStart();
        foreach (KeyValuePair<int, string> item in list)
        {
            string value = item.Value;
        }
        TimeUtil.GetMethodTimeEnd("2", stopwatch);
        stopwatch = TimeUtil.GetMethodTimeStart();
        //通过键的集合取
        foreach (var key in list.Keys)
        {
            string value = list[key];
        }
        TimeUtil.GetMethodTimeEnd("3", stopwatch);
        stopwatch = TimeUtil.GetMethodTimeStart();
        //直接取值
        foreach (var val in list.Values)
        {
            string value = val;
        }
        TimeUtil.GetMethodTimeEnd("4", stopwatch);
        stopwatch = TimeUtil.GetMethodTimeStart();
        //非要采用for的方法也可
        List<int> test = new List<int>(list.Keys);
        for (int i = 0; i < test.Count; i++)
        {
            string value = list[test[i]];
        }
        TimeUtil.GetMethodTimeEnd("5", stopwatch);
    }


}
