using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WorldCreateManager : BaseManager
{
    //存储着世界中所有的Chunk
    public static List<TerrainForChunk> chunks = new List<TerrainForChunk>();

    //每个Chunk的长宽Size
    public static int width = 50;
    //每个Chunk的高度
    public static int height = 30;


    //随机种子
    public int seed;

    //最小生成高度
    public float baseHeight = 10;

    //噪音频率（噪音采样时会用到）
    public float frequency = 0.025f;
    //噪音振幅（噪音采样时会用到）
    public float amplitude = 1;

}