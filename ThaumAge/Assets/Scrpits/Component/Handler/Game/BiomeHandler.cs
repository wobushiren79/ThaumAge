﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BiomeHandler : BaseHandler<BiomeHandler, BiomeManager>
{
    public Vector3 offset0;
    public Vector3 offset1;
    public Vector3 offset2;
    public float offsetBiome;
    public void InitWorldBiomeSeed()
    {
        offsetBiome = Random.value * 1000;
        offset0 = new Vector3(Random.value * 1000, Random.value * 1000, Random.value * 1000);
        offset1 = new Vector3(Random.value * 1000, Random.value * 1000, Random.value * 1000);
        offset2 = new Vector3(Random.value * 1000, Random.value * 1000, Random.value * 1000);
    }

    /// <summary>
    /// 根据生物生态 创造方块
    /// </summary>
    /// <param name="listBiome"></param>
    /// <param name="wPos"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public BlockTypeEnum CreateBiomeBlockType(Chunk chunk, List<Vector3Int> listBiomeCenterPosition, List<Biome> listBiome, Vector3Int blockLocPosition)
    {
        Vector3Int wPos = blockLocPosition + chunk.worldPosition;
        //y坐标是否在Chunk内
        if (wPos.y >= chunk.height)
        {
            return BlockTypeEnum.None;
        }
        //最近的距离
        float minDis = float.MaxValue;
        //第二靠近的生态点 用于生态边缘处理
        float secondDis = float.MaxValue;
        //最靠近的生态点
        Vector3Int biomeCenterPosition = Vector3Int.zero;
        //便利中心点，寻找最靠近的生态点（维诺图）
        for (int i = 0; i < listBiomeCenterPosition.Count; i++)
        {
            Vector3Int itemCenterPosition = listBiomeCenterPosition[i];
            float tempDis = Vector3Int.Distance(itemCenterPosition, new Vector3Int(wPos.x, 0, wPos.z));
            if (tempDis < minDis)
            {
                //如果小于最小距离
                biomeCenterPosition = itemCenterPosition;
                secondDis = minDis;
                minDis = tempDis;
            }
            else
            {
                //如果大于最小距离 并且小于第二小距离
                if (tempDis < secondDis)
                {
                    secondDis = tempDis;
                }
            }
        }

        //获取该点的生态信息
        //int worldSeed = WorldCreateHandler.Instance.manager.GetWorldSeed();
        //RandomTools biomeRandom = RandomUtil.GetRandom(worldSeed, biomeCenterPosition.x, biomeCenterPosition.z);
        //int biomeIndex = biomeRandom.NextInt(listBiome.Count);
        int biomeIndex = WorldRandTools.Range(listBiome.Count, biomeCenterPosition);

        Biome biome = listBiome[biomeIndex];
        BiomeInfoBean biomeInfo = manager.GetBiomeInfo(biome.biomeType);

        //获取当前位置方块随机生成的高度值
        int genHeight = GetHeightData(wPos, biomeInfo);
        //边缘处理 逐渐减缓到最低高度
        float offsetDis = secondDis - minDis;
        if (genHeight - biomeInfo.minHeight > 2//高度大于3格
            && offsetDis <= 10) //在10范围以内
        {
            int edgeHeight = Mathf.CeilToInt((genHeight - biomeInfo.minHeight) / 10f) * Mathf.CeilToInt(offsetDis) + biomeInfo.minHeight;
            //只有当小于最大高度时才使用生成边缘高度
            if (edgeHeight < genHeight)
            {
                genHeight = edgeHeight;
            }
        }
        //当前方块位置高于随机生成的高度值时，当前方块类型为空
        if (wPos.y > genHeight)
        {
            return BlockTypeEnum.None;
        }
        //获取方块
        return biome.GetBlockType(genHeight, blockLocPosition, wPos);
    }

    /// <summary>
    /// 获取高度信息
    /// </summary>
    /// <param name="wPos"></param>
    /// <param name="biomeInfo"></param>
    /// <returns></returns>
    public int GetHeightData(Vector3Int wPos, BiomeInfoBean biomeInfo)
    {

        ////让随机种子，振幅，频率，应用于我们的噪音采样结果
        float x0 = (wPos.x + offset0.x) * biomeInfo.frequency;
        float y0 = (wPos.y + offset0.y) * biomeInfo.frequency;
        float z0 = (wPos.z + offset0.z) * biomeInfo.frequency;

        //float x1 = (wPos.x + offset1.x) * biomeInfo.frequency * 2;
        //float y1 = (wPos.y + offset1.y) * biomeInfo.frequency * 2;
        //float z1 = (wPos.z + offset1.z) * biomeInfo.frequency * 2;

        //float x2 = (wPos.x + offset2.x) * biomeInfo.frequency / 4;
        //float y2 = (wPos.y + offset2.y) * biomeInfo.frequency / 4;
        //float z2 = (wPos.z + offset2.z) * biomeInfo.frequency / 4;

        //float noise0 = SimplexNoiseUtil.Generate(x0, y0, z0) * biomeInfo.amplitude;
        //float noise1 = SimplexNoiseUtil.Generate(x1, y1, z1) * biomeInfo.amplitude / 2;
        //float noise2 = SimplexNoiseUtil.Generate(x2, y2, z2) * biomeInfo.amplitude / 4;

        ////在采样结果上，叠加上baseHeight，限制随机生成的高度下限
        //return Mathf.FloorToInt(noise0 + noise1 + noise2 + biomeInfo.minHeight);

        float noise0 = Mathf.PerlinNoise(x0, z0) * biomeInfo.amplitude;
        return Mathf.FloorToInt(noise0 + biomeInfo.minHeight);
    }

    /// <summary>
    /// 获取生态中心点
    /// </summary>
    /// <param name="currentChunk"></param>
    /// <param name="range">范围</param>
    /// <param name="rate">倍数</param>
    /// <returns></returns>
    public List<Vector3Int> GetBiomeCenterPosition(Chunk currentChunk, int range, int rate)
    {
        List<Vector3Int> listData = new List<Vector3Int>();
        int worldSeed = WorldCreateHandler.Instance.manager.GetWorldSeed();
        int unitSize = currentChunk.width * rate;
        int startX = Mathf.FloorToInt(currentChunk.worldPosition.x / (float)unitSize) * unitSize;
        int startZ = Mathf.FloorToInt(currentChunk.worldPosition.z / (float)unitSize) * unitSize;
        for (int x = -range; x < range; x++)
        {
            for (int z = -range; z < range; z++)
            {
                Vector3Int currentPosition = new Vector3Int(x * unitSize + startX, 0, z * unitSize + startZ);
                //RandomTools random = RandomUtil.GetRandom(worldSeed, currentPosition.x, currentPosition.z);
                //int addRate = random.NextInt(50);
                int addRate = WorldRandTools.Range(50, currentPosition * worldSeed);
                if (addRate <= 1)
                {
                    listData.Add(currentPosition);
                }
            }
        }
        return listData;
    }

    /// <summary>
    /// 根据世界类型获取生态数据
    /// </summary>
    /// <param name="worldType"></param>
    /// <returns></returns>
    public List<Biome> GetBiomeListByWorldType(WorldTypeEnum worldType)
    {
        List<Biome> listBiome = new List<Biome>();
        switch (worldType)
        {
            case WorldTypeEnum.Main:
                if(CheckUtil.ListIsNull(manager.listBiomeForMain))
                {
                    manager.listBiomeForMain.Add(new BiomePrairie());
                    manager.listBiomeForMain.Add(new BiomeForest());
                    manager.listBiomeForMain.Add(new BiomeDesert());
                    manager.listBiomeForMain.Add(new BiomeMagicForest());
                    manager.listBiomeForMain.Add(new BiomeVolcano());
                    manager.listBiomeForMain.Add(new BiomeMountain());
                    manager.listBiomeForMain.Add(new BiomeOcean());
                }
                listBiome = manager.listBiomeForMain;
                break;
        }
        return listBiome;
    }


}