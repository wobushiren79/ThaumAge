﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class BiomeHandler : BaseHandler<BiomeHandler, BiomeManager>
{
    protected static object lockWorldCreate = new object();
    public FastNoise fastNoise;
    public float offsetBiome;

    public void InitWorldBiomeData()
    {

    }
    public void InitWorldBiomeSeed()
    {
        int seed = WorldCreateHandler.Instance.manager.GetWorldSeed();
        offsetBiome = UnityEngine.Random.value * 1000;
        fastNoise = new FastNoise(seed);
        SimplexNoiseUtil.Seed = seed;
    }

    /// <summary>
    /// 清理缓存数据
    /// </summary>
    public void ClearBiomeMapData()
    {
        manager.dicWorldChunkTerrainDataPool.Clear();
    }

    public void GetBiomeMapData(Chunk chunk, Action<BiomeMapData> callBackForComplete)
    {
        if (manager.dicWorldChunkTerrainDataPool.TryGetValue($"{chunk.chunkData.positionForWorld.x}|{chunk.chunkData.positionForWorld.z}", out BiomeMapData biomeMapData))
        {
            callBackForComplete?.Invoke(biomeMapData);
        }
        else
        {
            biomeMapData = new BiomeMapData();
            biomeMapData.InitData(chunk, (data) =>
             {
                //添加到缓存中
                if (manager.dicWorldChunkTerrainDataPool.Count > 2047)
                     ClearBiomeMapData();
                 manager.dicWorldChunkTerrainDataPool.Add($"{chunk.chunkData.positionForWorld.x}|{chunk.chunkData.positionForWorld.z}", data);

                 callBackForComplete?.Invoke(data);
             });
        }
    }

    /// <summary>
    /// 根据生物生态 创造方块
    /// </summary>
    /// <param name="listBiome"></param>
    /// <param name="wPos"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public BlockTypeEnum CreateBiomeBlockType(Chunk chunk, Vector3Int blockLocalPosition, Biome biome, ChunkTerrainData chunkTerrainData)
    {
        return biome.InitBiomeBlock(chunk, blockLocalPosition, chunkTerrainData);
    }

    /// <summary>
    /// 根据生物生态 创建区块方块
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="biome"></param>
    public void CreateBiomeBlockTypeForChunk(Chunk chunk, BiomeMapData biomeMapData, Biome biome)
    {

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
}