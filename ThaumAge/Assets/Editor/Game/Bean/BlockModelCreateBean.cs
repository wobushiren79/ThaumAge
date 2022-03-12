using UnityEditor;
using UnityEngine;
using System;

[Serializable]
public class BlockModelCreateBean 
{
    //模型名字
    public string nameBlock;
    //UV开始的地方
    public Vector2Int startUV;
    //UV缩放比例
    public int uvScaleSize;
    //单个方块的贴图
    public Texture2D texureBlock;
}