using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class BlockModelCreateBean 
{
    //模型名字
    public string nameBlock;
    //像素开始的地方
    public Vector2Int startPixel;
    //UV缩放比例
    public int uvScaleSize;
    //方块的贴图
    public List<Texture2D> listTexureBlock;
    //贴图大小
    public int texureSize;


    /// <summary>
    /// 获取开始UV
    /// </summary>
    /// <param name="texureSize">贴图大小</param>
    /// <returns></returns>
    public Vector2 GetStartUV(int texureSize)
    {
        Vector2 startUV = new Vector2(startPixel.x / (float)texureSize, startPixel.y / (float)texureSize);
        return startUV;
    }
}