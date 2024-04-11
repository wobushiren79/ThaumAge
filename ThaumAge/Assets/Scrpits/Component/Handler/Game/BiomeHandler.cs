using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class BiomeHandler : BaseHandler<BiomeHandler, BiomeManager>
{
    protected static object lockWorldCreate = new object();
    public float offsetBiome;

    public void InitWorldBiomeData()
    {

    }
    public void InitWorldBiomeSeed()
    {
        int seed = WorldCreateHandler.Instance.manager.GetWorldSeed();
        offsetBiome = UnityEngine.Random.value * 1000;
        SimplexNoiseUtil.Seed = seed;
    }

    /// <summary>
    /// 获取生态中心点 （无用）
    /// </summary>
    /// <param name="currentChunk"></param>
    /// <param name="range">范围</param>
    /// <param name="rate">倍数</param>
    /// <returns></returns>
    public Vector3Int[] GetBiomeCenterPosition(Chunk currentChunk, int range, int rate)
    {
        List<Vector3Int> listData = new List<Vector3Int>();
        int worldSeed = WorldCreateHandler.Instance.manager.GetWorldSeed();
        int unitSize = currentChunk.chunkData.chunkWidth * rate;
        int startX = Mathf.FloorToInt(currentChunk.chunkData.positionForWorld.x / (float)unitSize) * unitSize;
        int startZ = Mathf.FloorToInt(currentChunk.chunkData.positionForWorld.z / (float)unitSize) * unitSize;
        for (int x = -range; x < range; x++)
        {
            for (int z = -range; z < range; z++)
            {
                Vector3Int currentPosition = new Vector3Int(x * unitSize + startX, 0, z * unitSize + startZ);
                int addRate = WorldRandTools.Range(50, currentPosition * worldSeed);
                if (addRate <= 1)
                {
                    listData.Add(currentPosition);
                }
            }
        }
        return listData.ToArray();
    }

    /// <summary>
    /// 刷新测试生态数据
    /// </summary>
    public void RefreshTestBiomeData()
    {
        var biomeTest = manager.GetBiome(BiomeTypeEnum.Test);
        //如果是测试生态 直接获取GameLauncher里的数据
        if (GameHandler.Instance.launcher is GameLauncher gameLauncher)
        {
            biomeTest.terrain3DCShaderNoise = gameLauncher.testTerrain3DCShaderNoise;
        }
    }
}