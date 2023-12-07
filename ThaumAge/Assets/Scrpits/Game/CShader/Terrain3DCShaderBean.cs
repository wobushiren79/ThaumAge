using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Terrain3DCShaderBean
{
    //区块位置
    public Vector3 chunkPosition;
    //区块宽
    public int chunkSizeW;
    //区块高
    public int chunkSizeH; 
    //洞穴状态 0不生成洞穴 1生成普通洞穴
    public int stateCaves; 
    //基岩状态 0不生成基岩 1生成基岩 Y=0时
    public int stateBedrock;
    
    //种子
    public int seed;
    //种子偏移
    public Vector3 seedOffset = Vector3.zero;

    //noise层级
    public ComputeBuffer noiseLayersArrayBuffer;
    public Terrain3DCShaderNoiseLayers[] noiseLayers;

    //方块数据
    public ComputeBuffer blockArrayBuffer;
    //不是空气方块的数量
    public ComputeBuffer blockCountBuffer;
    

    /// <summary>
    /// 获取方块总数
    /// </summary>
    public int GetBlockTotalNum()
    {
        return chunkSizeW * chunkSizeW * chunkSizeH;
    }

    public void Dispose()
    {
        blockArrayBuffer?.Dispose();
        blockCountBuffer?.Dispose();
    }
}

[System.Serializable]
public struct Terrain3DCShaderNoiseLayers
{
    //生态ID
    public int biomeId;
    //出现频率 数值越大 波峰越多
    public float frequency;
    //振幅 数值越大 越宽 （0-1）
    public float amplitude;
    //间隙性
    public float lacunarity;
    //噪音循环迭代次数 复杂度
    public int octaves;

    //洞穴大小
    public float caveScale;
    //洞穴的阈值（0-1）
    public float caveThreshold;

    //洞穴出现频率 数值越大 波峰越多
    public float caveFrequency;
    //洞穴振幅 数值越大 越宽
    public float caveAmplitude;
    //洞穴循环迭代次数 复杂度
    public int caveOctaves;

    //地面的最低高度
    public int groundMinHeigh;
    //海洋高度
    public int oceanHeight;
}
