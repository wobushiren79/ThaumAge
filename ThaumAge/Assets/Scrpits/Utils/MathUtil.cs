
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
        return GetSingleIndexForThree(data.x, data.y, data.z, size1, size2);
    }

    public static int GetSingleIndexForThree(int a, int b, int c, int size1, int size2)
    {
        return a * size1 * size2 + b * size1 + c;
    }

    /// <summary>
    /// 获取个位
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static int GetUnit(int data)
    {
        return data % 10;
    }

    /// <summary>
    /// 获取数字的十位
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static int GetUnitTen(int data)
    {
        return (data % 100) / 10;
    }

    /// <summary>
    /// 获取数字的百位
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static int GetUnitHundred(int data)
    {
        return (data % 1000) / 100;
    }

    /// <summary>
    /// 获取贝塞尔曲线上一点
    /// </summary>
    /// <param name="t">0到1的值，0获取曲线的起点，1获得曲线的终点</param>
    /// <param name="start">曲线的起始位置</param>
    /// <param name="center">决定曲线形状的控制点</param>
    /// <param name="end">曲线的终点</param>
    /// <returns></returns>
    public static Vector3 GetBezierPoint(float t, Vector3 start, Vector3 center, Vector3 end)
    {
        return (1 - t) * (1 - t) * start + 2 * t * (1 - t) * center + t * t * end;
    }

    /// <summary>
    /// 获取贝塞尔曲线
    /// </summary>
    /// <returns></returns>
    public static Vector3[] GetBezierPoints(int number, Vector3 start, Vector3 center, Vector3 end)
    {
        Vector3[] pointArray = new Vector3[number];
        for (int i = 0; i < number; i++)
        {
            float pro = (i + 0.5f) / number;
            GetBezierPoint(pro, start, center, end);
        }
        return pointArray;
    }

    /// <summary>
    /// 获取贝塞尔曲线
    /// </summary>
    /// <returns></returns>
    public static Vector3[] GetBezierPoints(int number, Vector3 start, Vector3 end, float hight)
    {
        Vector3 centerPoint = ((end - start) / 2) + new Vector3(0, hight, 0);
        return GetBezierPoints(number, start, end, centerPoint);
    }
}