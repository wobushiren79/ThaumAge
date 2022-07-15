using UnityEditor;
using UnityEngine;

public struct ChunkTerrainData
{
    //当前坐标
    public Vector2 position;
    //频率（宽度）
    public float perlinFrequency0;
    public float perlinFrequency1;
    public float perlinFrequency2;
    //振幅(高度) 
    public float perlinAmplitude0;
    public float perlinAmplitude1;
    public float perlinAmplitude2;
    //循环大小 最好大于512
    public float perlinSize0;
    public float perlinSize1;
    public float perlinSize2;
    //迭代次数（越多地图越复杂）
    public int perlinIterateNumber0;
    public int perlinIterateNumber1;
    public int perlinIterateNumber2;
    //最低高度
    public float minHeight;

    //距离该方块最近的生态点距离
    public float minBiomeDis;
    //距离该方块第二近的生态点距离
    public float secondMinBiomeDis;
    //距离最近和第二进的距离差
    public float offsetDis;
    //地形最大高度
    public int maxHeight;

}