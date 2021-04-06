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
        public int leavesRange;//树叶范围
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
        public int minHeight;
        public int maxHeight;
        public BlockTypeEnum cactusType;
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
        RandomTools random = RandomUtil.GetRandom(worldSeed, startPosition.x, startPosition.y, startPosition.z);
        //生成概率
        int addRate = random.NextInt(treeData.addRateMax);
        //高度
        int treeHeight = random.NextInt(treeData.maxHeight - treeData.minHeight) + treeData.minHeight;

        if (addRate < treeData.addRateMin)
        {
            for (int i = 0; i < treeHeight + 2; i++)
            {
                Vector3Int treeTrunkPosition = startPosition + Vector3Int.up * (i + 1);
                //生成树干
                if (i < treeHeight)
                {
                    BlockBean blockData = new BlockBean(treeData.treeTrunk, treeTrunkPosition);
                    WorldCreateHandler.Instance.manager.listUpdateBlock.Add(blockData);
                }
                if (i > 2)
                {
                    //最大范围
                    int range = treeData.leavesRange;
                    if (i >= treeHeight)
                    {
                        //叶子在最顶层递减
                        range = range - (i - treeHeight);
                        if (range < 0)
                            range = 0;
                    }

                    //生成叶子
                    for (int x = -range; x <= range; x++)
                    {
                        for (int z = -range; z <= range; z++)
                        {
                            if (x == startPosition.x && z == startPosition.z)
                                continue;
                            BlockBean blockData = new BlockBean(treeData.treeLeaves, treeTrunkPosition+ new Vector3Int(x,0,z));
                            WorldCreateHandler.Instance.manager.listUpdateBlock.Add(blockData);
                        }
                    }
                }
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
        RandomTools random = RandomUtil.GetRandom(worldSeed, startPosition.x, startPosition.y, startPosition.z);
        //生成概率
        int addRate = random.NextInt(weedData.addRateMax);
        int weedTypeNumber = random.NextInt(weedData.listWeedType.Count);
        if (addRate < weedData.addRateMin)
        {
            BlockBean blockData = new BlockBean(weedData.listWeedType[weedTypeNumber], startPosition + Vector3Int.up);
            WorldCreateHandler.Instance.manager.listUpdateBlock.Add(blockData);
        }
    }


    /// <summary>
    /// 增加仙人掌
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="cactusData"></param>
    public virtual void AddCactus(Vector3Int startPosition, CactusData cactusData)
    {
        int worldSeed = WorldCreateHandler.Instance.manager.GetWorldSeed();
        RandomTools random = RandomUtil.GetRandom(worldSeed, startPosition.x, startPosition.y, startPosition.z);
        //生成概率
        int addRate = random.NextInt(cactusData.addRateMax);
        //高度
        int treeHeight = random.NextInt(cactusData.maxHeight - cactusData.minHeight) + cactusData.minHeight;

        if (addRate < cactusData.addRateMin)
        {
            for (int i = 0; i < treeHeight; i++)
            {
                Vector3Int treeTrunkPosition = startPosition + Vector3Int.up * (i + 1);
                //生成树干
                if (i < treeHeight)
                {
                    BlockBean blockData = new BlockBean(cactusData.cactusType, treeTrunkPosition);
                    WorldCreateHandler.Instance.manager.listUpdateBlock.Add(blockData);
                }
            }
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
        RandomTools random = RandomUtil.GetRandom(worldSeed, startPosition.x, startPosition.y, startPosition.z);
        int addRate = random.NextInt(flowerData.addRateMax);
        int flowerTypeNumber = random.NextInt(flowerData.listFlowerType.Count);
        if (addRate < flowerData.addRateMin)
        {
            BlockBean blockData = new BlockBean(flowerData.listFlowerType[flowerTypeNumber], startPosition + Vector3Int.up);
            WorldCreateHandler.Instance.manager.listUpdateBlock.Add(blockData);
        }
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