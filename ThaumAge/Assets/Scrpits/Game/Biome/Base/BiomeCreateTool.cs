using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BiomeCreateTool
{
    public struct TreeData
    {
        public int addRateMin;
        public int addRateMax;
        public int minHeight;
        public int maxHeight;
        public BlockTypeEnum treeTrunk;//树干
        public BlockTypeEnum treeLeaves;//树叶
        public int trunkRange;//躯干范围
        public int leavesRange;//树叶范围
    }

    public struct PlantData
    {
        public int addRateMin;
        public int addRateMax;
        public List<BlockTypeEnum> listPlantType;
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
    /// 增加普通的树
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="treeData"></param>
    public static  void AddTree(Vector3Int startPosition, TreeData treeData)
    {
        int worldSeed = WorldCreateHandler.Instance.manager.GetWorldSeed();
        RandomTools random = RandomUtil.GetRandom(worldSeed + 1, startPosition.x, startPosition.y, startPosition.z);
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
                        range -= (i - treeHeight);
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
                            if (Math.Abs(x) == range || Math.Abs(z) == range)
                            {
                                //如果是边界 则有几率不生成
                                int randomLeaves = random.NextInt(3);
                                if (randomLeaves == 0)
                                    continue;
                            }
                            BlockBean blockData = new BlockBean(treeData.treeLeaves, treeTrunkPosition + new Vector3Int(x, 0, z));
                            WorldCreateHandler.Instance.manager.listUpdateBlock.Add(blockData);
                        }
                    }
                }
            }
        }
    }


    /// <summary>
    /// 生成大树
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="treeData"></param>
    public static void AddBigTree(Vector3Int startPosition, TreeData treeData)
    {
        int worldSeed = WorldCreateHandler.Instance.manager.GetWorldSeed();
        RandomTools random = RandomUtil.GetRandom(worldSeed + 2, startPosition.x, startPosition.y, startPosition.z);
        //生成概率
        int addRate = random.NextInt(treeData.addRateMax);
        //高度
        int treeHeight = random.NextInt(treeData.maxHeight - treeData.minHeight) + treeData.minHeight;

        if (addRate < treeData.addRateMin)
        {

            for (int i = 5; i < treeHeight + 3; i++)
            {
                //生成树叶
                Vector3Int treeTrunkPosition = startPosition + Vector3Int.up * (i + 1);
                int range = treeData.leavesRange;
                if (i == treeHeight + 2)
                {
                    range -= 1;
                }
                else if (i == 5)
                {
                    range -= 1;
                }

                for (int x = -range; x <= range; x++)
                {
                    for (int z = -range; z <= range; z++)
                    {
                        //生成概率
                        if (x == -range || x == range || z == -range || z == range)
                        {
                            int leavesRate = random.NextInt(4);
                            if (leavesRate == 0)
                                continue;
                        }
                        BlockBean blockData = new BlockBean(treeData.treeLeaves, treeTrunkPosition + new Vector3Int(x, 0, z));
                        WorldCreateHandler.Instance.manager.listUpdateBlock.Add(blockData);
                    }
                }
            }


            //生成树干
            for (int i = 0; i < treeHeight; i++)
            {
                Vector3Int treeTrunkPosition = startPosition + Vector3Int.up * (i + 1);

                if (i == 0 || i == 1)
                {
                    if (i == 0)
                    {
                        BlockBean leftData_1 = new BlockBean(treeData.treeTrunk, treeTrunkPosition + Vector3Int.left * 2, DirectionEnum.Left);
                        WorldCreateHandler.Instance.manager.listUpdateBlock.Add(leftData_1);

                        BlockBean rightData_1 = new BlockBean(treeData.treeTrunk, treeTrunkPosition + Vector3Int.right * 2, DirectionEnum.Right);
                        WorldCreateHandler.Instance.manager.listUpdateBlock.Add(rightData_1);

                        BlockBean forwardData_1 = new BlockBean(treeData.treeTrunk, treeTrunkPosition + Vector3Int.forward * 2, DirectionEnum.Forward);
                        WorldCreateHandler.Instance.manager.listUpdateBlock.Add(forwardData_1);

                        BlockBean backData_1 = new BlockBean(treeData.treeTrunk, treeTrunkPosition + Vector3Int.back * 2, DirectionEnum.Back);
                        WorldCreateHandler.Instance.manager.listUpdateBlock.Add(backData_1);
                    }


                    BlockBean leftData_2 = new BlockBean(treeData.treeTrunk, treeTrunkPosition + new Vector3Int(1, 0, 1));
                    WorldCreateHandler.Instance.manager.listUpdateBlock.Add(leftData_2);

                    BlockBean rightData_2 = new BlockBean(treeData.treeTrunk, treeTrunkPosition + new Vector3Int(-1, 0, -1));
                    WorldCreateHandler.Instance.manager.listUpdateBlock.Add(rightData_2);

                    BlockBean forwardData_2 = new BlockBean(treeData.treeTrunk, treeTrunkPosition + new Vector3Int(1, 0, -1));
                    WorldCreateHandler.Instance.manager.listUpdateBlock.Add(forwardData_2);

                    BlockBean backData_2 = new BlockBean(treeData.treeTrunk, treeTrunkPosition + new Vector3Int(-1, 0, 1));
                    WorldCreateHandler.Instance.manager.listUpdateBlock.Add(backData_2);
                }

                if (i > treeHeight - 3)
                {
                    int isCreate = random.NextInt(4);
                    if (isCreate == 1)
                    {
                        BlockBean leftData_1 = new BlockBean(treeData.treeTrunk, treeTrunkPosition + Vector3Int.left * 2, DirectionEnum.Left);
                        WorldCreateHandler.Instance.manager.listUpdateBlock.Add(leftData_1);
                    }
                    isCreate = random.NextInt(4);
                    if (isCreate == 1)
                    {
                        BlockBean rightData_1 = new BlockBean(treeData.treeTrunk, treeTrunkPosition + Vector3Int.right * 2, DirectionEnum.Right);
                        WorldCreateHandler.Instance.manager.listUpdateBlock.Add(rightData_1);
                    }
                    isCreate = random.NextInt(4);
                    if (isCreate == 1)
                    {
                        BlockBean forwardData_1 = new BlockBean(treeData.treeTrunk, treeTrunkPosition + Vector3Int.forward * 2, DirectionEnum.Forward);
                        WorldCreateHandler.Instance.manager.listUpdateBlock.Add(forwardData_1);
                    }
                    isCreate = random.NextInt(4);
                    if (isCreate == 1)
                    {
                        BlockBean backData_1 = new BlockBean(treeData.treeTrunk, treeTrunkPosition + Vector3Int.back * 2, DirectionEnum.Back);
                        WorldCreateHandler.Instance.manager.listUpdateBlock.Add(backData_1);
                    }
                }

                BlockBean blockData = new BlockBean(treeData.treeTrunk, treeTrunkPosition);
                WorldCreateHandler.Instance.manager.listUpdateBlock.Add(blockData);

                BlockBean leftData = new BlockBean(treeData.treeTrunk, treeTrunkPosition + Vector3Int.left);
                WorldCreateHandler.Instance.manager.listUpdateBlock.Add(leftData);

                BlockBean rightData = new BlockBean(treeData.treeTrunk, treeTrunkPosition + Vector3Int.right);
                WorldCreateHandler.Instance.manager.listUpdateBlock.Add(rightData);

                BlockBean forwardData = new BlockBean(treeData.treeTrunk, treeTrunkPosition + Vector3Int.forward);
                WorldCreateHandler.Instance.manager.listUpdateBlock.Add(forwardData);

                BlockBean backData = new BlockBean(treeData.treeTrunk, treeTrunkPosition + Vector3Int.back);
                WorldCreateHandler.Instance.manager.listUpdateBlock.Add(backData);
            }

        }
    }

    /// <summary>
    /// 增加植物
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="weedData"></param>
    public static void AddPlant(Vector3Int startPosition, PlantData plantData)
    {
        int worldSeed = WorldCreateHandler.Instance.manager.GetWorldSeed();
        RandomTools random = RandomUtil.GetRandom(worldSeed + 3, startPosition.x, startPosition.y, startPosition.z);
        //生成概率
        int addRate = random.NextInt(plantData.addRateMax);
        int weedTypeNumber = random.NextInt(plantData.listPlantType.Count);
        if (addRate < plantData.addRateMin)
        {
            BlockBean blockData = new BlockBean(plantData.listPlantType[weedTypeNumber], startPosition + Vector3Int.up);
            WorldCreateHandler.Instance.manager.listUpdateBlock.Add(blockData);
        }
    }


    /// <summary>
    /// 增加仙人掌
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="cactusData"></param>
    public static void AddCactus(Vector3Int startPosition, CactusData cactusData)
    {
        int worldSeed = WorldCreateHandler.Instance.manager.GetWorldSeed();
        RandomTools random = RandomUtil.GetRandom(worldSeed + 4, startPosition.x, startPosition.y, startPosition.z);
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
    public static void AddFlower(Vector3Int startPosition, FlowerData flowerData)
    {
        int worldSeed = WorldCreateHandler.Instance.manager.GetWorldSeed();
        RandomTools random = RandomUtil.GetRandom(worldSeed + 5, startPosition.x, startPosition.y, startPosition.z);
        int addRate = random.NextInt(flowerData.addRateMax);
        int flowerTypeNumber = random.NextInt(flowerData.listFlowerType.Count);
        if (addRate < flowerData.addRateMin)
        {
            BlockBean blockData = new BlockBean(flowerData.listFlowerType[flowerTypeNumber], startPosition + Vector3Int.up);
            WorldCreateHandler.Instance.manager.listUpdateBlock.Add(blockData);
        }
    }
}