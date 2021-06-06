using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public  class EnumUtil
{
    public static string GetEnumName<T>(T data)
    {
         return data.ToString();
    }

    public static T GetEnum<T>(string data)
    {
        return (T)Enum.Parse(typeof(T), data);
    }

    public static E GetEnum<E>(int type)
    {
        return (E)Enum.ToObject(typeof(E), type);
    }

    /// <summary>
    /// 获取枚举最大值
    /// </summary>
    /// <param name="enumType"></param>
    /// <returns></returns>
    public static int GetEnumMaxIndex<E>()
    {  
        int maxIndex = int.MinValue;
        Array EnumArray = Enum.GetValues(typeof(E));
        foreach (int item in EnumArray)
        {
            if (item > maxIndex)
                maxIndex = item;
        }
        return maxIndex;
    }

    /// <summary>
    /// 获取枚举第几项
    /// </summary>
    /// <typeparam name="E"></typeparam>
    /// <param name="position"></param>
    /// <returns></returns>
    public static E GetEnumValueByPosition<E>(int position)
    {
        int i = 0;
        foreach (E item in Enum.GetValues(typeof(E)))
        {
            if (i == position)
            {
                return item;
            }
            i++;
        }
        return default;
    }

    /// <summary>
    /// 获取所有枚举类型
    /// </summary>
    /// <typeparam name="E"></typeparam>
    /// <returns></returns>
    public static List<E> GetEnumValue<E>()
    {
        List<E> listDat = new List<E>();
        int i = 0;
        foreach (E item in Enum.GetValues(typeof(E)))
        {
            listDat.Add(item);
            i++;
        }
        return listDat;
    }

}