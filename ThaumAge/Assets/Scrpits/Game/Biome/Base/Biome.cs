﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static BiomeCreateTool;

public class Biome
{
    public BiomeTypeEnum biomeType;

    public Biome(BiomeTypeEnum biomeType)
    {
        this.biomeType = biomeType;
    }

    /// <summary>
    /// 获取方块类型
    /// </summary>
    /// <param name="genHeight"></param>
    /// <returns></returns>
    public virtual BlockTypeEnum GetBlockType(Chunk chunk,BiomeInfoBean biomeInfo,int genHeight, Vector3Int localPos, Vector3Int wPos)
    {
        if (wPos.y < genHeight - 1)
        {
            AddCave(wPos);
        }
        return BlockTypeEnum.Stone;
    }

    /// <summary>
    /// 增加洞穴
    /// </summary>
    /// <param name="startPosition"></param>
    public void AddCave(Vector3Int startPosition)
    {
        BiomeForCaveData caveData = new BiomeForCaveData
        {
            addRate = 0.00001f,
            minDepth = 1,
            maxDepth = 4,
        };
        BiomeCreateTool.AddCave(startPosition, caveData);
    }

    /// <summary>
    /// 生成花丛
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="flowerData"></param>
    //public virtual void AddFlowerRange(Vector3Int startPosition, FlowerData flowerData)
    //{
    //    int worldSeed = WorldCreateHandler.Instance.manager.GetWorldSeed();
    //    System.Random random = new System.Random(worldSeed + startPosition.x * startPosition.y * startPosition.z);
    //    int flowerTypeNumber = random.Next(0, flowerData.listFlowerType.Count);
    //    float addRate = SimplexNoiseUtil.Generate(new Vector2(startPosition.x, startPosition.z), worldSeed, flowerData.flowerRange);
    //    if (addRate > (float)flowerData.addRateMin / flowerData.addRateMax)
    //    {
    //        BlockBean blockData = new BlockBean(flowerData.listFlowerType[flowerTypeNumber], Vector3Int.zero, startPosition + Vector3Int.up);
    //        WorldCreateHandler.Instance.manager.listUpdateBlock.Add(blockData);
    //    }
    //}
}