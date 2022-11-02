using UnityEditor;
using UnityEngine;

public struct ChunkTerrainData
{
    //当前坐标
    public Vector2 position;
    //地形最大高度
    public int maxHeight;
    //当前生态
    public int biomeIndex;
    //当前生态中心位置
    public Vector2 biomePosition;
}

