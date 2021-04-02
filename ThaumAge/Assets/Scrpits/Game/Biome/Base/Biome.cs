using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Biome
{
    public BiomeTypeEnum biomeType;

    public Biome(BiomeTypeEnum biomeType)
    {
        this.biomeType = biomeType;
    }

    public struct TreeData
    {
        public int addRateMin;
        public int addRateMax;
        public int minHeight;
        public int maxHeight;
        public BlockTypeEnum treeTrunk;//树干
        public BlockTypeEnum treeLeaves;//树叶
    }

    public struct WeedData
    {
        public int addRateMin;
        public int addRateMax;
        public List<BlockTypeEnum> listWeedType;
    }

    public struct CactusData
    {
        public int addRateMin;
        public int addRateMax;
    }

    public struct FlowerData
    {
        public int addRateMin;
        public int addRateMax;
        public float flowerRange;
        public List<BlockTypeEnum> listFlowerType;
    }

    /// <summary>
    /// 获取方块类型
    /// </summary>
    /// <param name="genHeight"></param>
    /// <returns></returns>
    public virtual BlockTypeEnum GetBlockType(int genHeight, Vector3Int localPos, Vector3Int wPos)
    {
        return BlockTypeEnum.Stone;
    }

    /// <summary>
    /// 增加普通的树
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="treeData"></param>
    public virtual void AddTree(Vector3Int startPosition, TreeData treeData)
    {
        int worldSeed = WorldCreateHandler.Instance.manager.GetWorldSeed();
        System.Random random = new System.Random(worldSeed * startPosition.x * startPosition.y * startPosition.z);
        //生成概率
        int addRate = random.Next(0, treeData.addRateMax);
        //高度
        int treeHeight = random.Next(treeData.minHeight, treeData.maxHeight + 1);

        if (addRate < treeData.addRateMin)
        {
            for (int i = 0; i < treeHeight; i++)
            {
                BlockBean blockData = new BlockBean(treeData.treeTrunk, Vector3Int.zero, startPosition + Vector3Int.up * (i + 1));
                WorldCreateHandler.Instance.manager.listUpdateBlock.Add(blockData);
            }
        }
    }

    /// <summary>
    /// 增加杂草
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="weedData"></param>
    public virtual void AddWeed(Vector3Int startPosition, WeedData weedData)
    {
        int worldSeed = WorldCreateHandler.Instance.manager.GetWorldSeed();
        int seed = (int)Math.Sqrt(worldSeed * startPosition.x * startPosition.y * startPosition.z);
        System.Random random = new System.Random(seed);
        int addRate = random.Next(0, weedData.addRateMax);
        int weedTypeNumber = random.Next(0, weedData.listWeedType.Count);
        if (addRate < weedData.addRateMin)
        {
            BlockBean blockData = new BlockBean(weedData.listWeedType[weedTypeNumber], Vector3Int.zero, startPosition + Vector3Int.up);
            WorldCreateHandler.Instance.manager.listUpdateBlock.Add(blockData);
        }
    }


    /// <summary>
    /// 增加鲜花
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="flowerData"></param>
    public virtual void AddFlower(Vector3Int startPosition, FlowerData flowerData)
    {
        int worldSeed = WorldCreateHandler.Instance.manager.GetWorldSeed();
        System.Random random = new System.Random(worldSeed * startPosition.x * startPosition.y * startPosition.z);
        int addRate = random.Next(0, flowerData.addRateMax);
        int flowerTypeNumber = random.Next(0, flowerData.listFlowerType.Count);
        if (addRate < flowerData.addRateMin)
        {
            BlockBean blockData = new BlockBean(flowerData.listFlowerType[flowerTypeNumber], Vector3Int.zero, startPosition + Vector3Int.up);
            WorldCreateHandler.Instance.manager.listUpdateBlock.Add(blockData);
        }
    }

    /// <summary>
    /// 生成花丛
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="flowerData"></param>
    public virtual void AddFlowerRange(Vector3Int startPosition, FlowerData flowerData)
    {
        int worldSeed = WorldCreateHandler.Instance.manager.GetWorldSeed();
        System.Random random = new System.Random(worldSeed * startPosition.x * startPosition.y * startPosition.z);
        int flowerTypeNumber = random.Next(0, flowerData.listFlowerType.Count);
        float addRate = SimplexNoiseUtil.Generate(new Vector2(startPosition.x, startPosition.z), worldSeed, flowerData.flowerRange);
        if (addRate > (float)flowerData.addRateMin / flowerData.addRateMax)
        {
            BlockBean blockData = new BlockBean(flowerData.listFlowerType[flowerTypeNumber], Vector3Int.zero, startPosition + Vector3Int.up);
            WorldCreateHandler.Instance.manager.listUpdateBlock.Add(blockData);
        }
    }
}