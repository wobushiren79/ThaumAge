
using System;
using UnityEngine;

public class MathUtil
{

    /// <summary>
    /// 获取唯一下标
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static int GetSingleIndexForTwo(int a, int b, int size)
    {
        return a * size + b;
    }


    public static int GetSingleIndexForThree(Vector3Int data, int size1, int size2)
    {
        return GetSingleIndexForThree(data.x, data.y, data.z,  size1,  size2);
    }

    public static int GetSingleIndexForThree(int a, int b, int c, int size1, int size2)
    {
        return a * size1 * size2 + b * size1 + c;
    }
}