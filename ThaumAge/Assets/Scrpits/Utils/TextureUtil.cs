using UnityEditor;
using UnityEngine;

public class TextureUtil
{
    /// <summary>
    /// 获取像素
    /// </summary>
    /// <param name="tex"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public static Color GetPixel(Texture2D tex, Vector2Int position)
    {
        return tex.GetPixel(position.x, position.y);
    }
}