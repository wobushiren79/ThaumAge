using UnityEditor;
using UnityEngine;

public struct ChunkTerrainData
{
    //当前坐标
    public Vector2 position;

    //距离该方块最近的生态点距离
    public float minBiomeDis;
    //距离该方块第二近的生态点距离
    public float secondMinBiomeDis;
    //距离最近和第二进的距离差
    public float offsetDis;
    //地形最大高度
    public int maxHeight;
    //当前生态
    public int biomeIndex;
}

