using UnityEditor;
using UnityEngine;

public struct ChunkTerrainData
{
    //当前坐标
    public Vector2 position;
    //频率（宽度）
    public float perlinFrequency;
    //振幅(高度) 
    public float perlinAmplitude;
    //循环大小 最好大于512
    public float perlinSize;
    //迭代次数（越多地图越复杂）
    public int perlinIterateNumber;
    //最低高度
    public float minHeight;
    //生态下标
    public int biomeIndex;
    //距离该方块最近的生态点距离
    public float minBiomeDis;
    //距离该方块第二近的生态点距离
    public float secondMinBiomeDis;
    //距离最近和第二进的距离差
    public float offsetDis;
    //地形最大高度
    public float maxHeight;

}