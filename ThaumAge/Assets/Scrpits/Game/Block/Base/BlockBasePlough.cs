using UnityEditor;
using UnityEngine;

public class BlockBasePlough : Block
{
    //耕地的左右旋转
    public Vector2[] uvsAddUpRotate;

    /// <summary>
    /// 获取meta数据
    /// </summary>
    /// <param name="rotate">0横 1竖</param>
    /// <returns></returns>
    public static string ToMetaData(int rotate)
    {
        return $"{rotate}";
    }

    public static void FromMetaData(string data, out int rotate)
    {
        rotate = int.Parse(data);
    }
}