using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class BiomeCreateTool
{
    public struct BiomeForTreeData
    {
        public float addRate;
        public int minHeight;
        public int maxHeight;
        public BlockTypeEnum treeTrunk;//树干
        public BlockTypeEnum treeLeaves;//树叶
        public int trunkRange;//躯干范围
        public int leavesRange;//树叶范围
    }

    public struct BiomeForPlantData
    {
        public float addRate;
        public List<BlockTypeEnum> listPlantType;
    }

    public struct BiomeForCactusData
    {
        public float addRate;
        public int minHeight;
        public int maxHeight;
        public BlockTypeEnum cactusType;
    }

    public struct BiomeForFlowerData
    {
        public float addRate;
        public float flowerRange;
        public List<BlockTypeEnum> listFlowerType;
    }

    public struct BiomeForCaveData
    {
        public float addRate;
        public int minDepth;
        public int maxDepth;
        public int minSize;
        public int maxSize;
    }

    public struct BiomeForWaterPoolData
    {
        public float addRate;
        public int minDepth;
        public int maxDepth;
        public int minSize;
        public int maxSize;
    }


    /// <summary>
    /// 增加普通的树
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="treeData"></param>
    public static void AddTree(uint randomData, Vector3Int startPosition, BiomeForTreeData treeData)
    {
        //生成概率
        float addRate = WorldRandTools.GetValue(startPosition, randomData);

        if (addRate < treeData.addRate)
        {
            //高度
            int treeHeight = WorldRandTools.Range(treeData.minHeight, treeData.maxHeight);
            for (int i = 0; i < treeHeight + 2; i++)
            {
                Vector3Int treeTrunkPosition = new(startPosition.x, startPosition.y + (i + 1), startPosition.z);
                //生成树干
                if (i < treeHeight)
                {
                    BlockTempBean blockData = new(treeData.treeTrunk, treeTrunkPosition.x, treeTrunkPosition.y, treeTrunkPosition.z);
                    WorldCreateHandler.Instance.manager.AddUpdateBlock(blockData);
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
                                int randomLeaves = WorldRandTools.Range(0, 3);
                                if (randomLeaves == 0)
                                    continue;
                            }
                            BlockTempBean blockData = new(treeData.treeLeaves, treeTrunkPosition.x + x, treeTrunkPosition.y, treeTrunkPosition.z + z);
                            WorldCreateHandler.Instance.manager.AddUpdateBlock(blockData);
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
    public static void AddTreeForBig(uint randomData, Vector3Int startPosition, BiomeForTreeData treeData)
    {
        //生成概率
        float addRate = WorldRandTools.GetValue(startPosition, randomData);

        if (addRate < treeData.addRate)
        {
            //高度
            int treeHeight = WorldRandTools.Range(treeData.minHeight, treeData.maxHeight);
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
                            int leavesRate = WorldRandTools.Range(0, 4);
                            if (leavesRate == 0)
                                continue;
                        }
                        BlockTempBean blockData = new(treeData.treeLeaves, treeTrunkPosition.x + x, treeTrunkPosition.y, treeTrunkPosition.z + z);
                        WorldCreateHandler.Instance.manager.AddUpdateBlock(blockData);
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
                        BlockTempBean leftData_1 = new(treeData.treeTrunk, DirectionEnum.Left, treeTrunkPosition.x - 2, treeTrunkPosition.y, treeTrunkPosition.z);
                        WorldCreateHandler.Instance.manager.AddUpdateBlock(leftData_1);

                        BlockTempBean rightData_1 = new(treeData.treeTrunk, DirectionEnum.Right, treeTrunkPosition.x + 2, treeTrunkPosition.y, treeTrunkPosition.z);
                        WorldCreateHandler.Instance.manager.AddUpdateBlock(rightData_1);

                        BlockTempBean forwardData_1 = new(treeData.treeTrunk, DirectionEnum.Forward, treeTrunkPosition.x - 2, treeTrunkPosition.y, treeTrunkPosition.z + 2);
                        WorldCreateHandler.Instance.manager.AddUpdateBlock(forwardData_1);

                        BlockTempBean backData_1 = new(treeData.treeTrunk, DirectionEnum.Back, treeTrunkPosition.x - 2, treeTrunkPosition.y, treeTrunkPosition.z - 2);
                        WorldCreateHandler.Instance.manager.AddUpdateBlock(backData_1);
                    }


                    BlockTempBean leftData_2 = new(treeData.treeTrunk, treeTrunkPosition.x + 1, treeTrunkPosition.y, treeTrunkPosition.z + 1);
                    WorldCreateHandler.Instance.manager.AddUpdateBlock(leftData_2);

                    BlockTempBean rightData_2 = new(treeData.treeTrunk, treeTrunkPosition.x - 1, treeTrunkPosition.y, treeTrunkPosition.z - 1);
                    WorldCreateHandler.Instance.manager.AddUpdateBlock(rightData_2);

                    BlockTempBean forwardData_2 = new(treeData.treeTrunk, treeTrunkPosition.x + 1, treeTrunkPosition.y, treeTrunkPosition.z - 1);
                    WorldCreateHandler.Instance.manager.AddUpdateBlock(forwardData_2);

                    BlockTempBean backData_2 = new(treeData.treeTrunk, treeTrunkPosition.x - 1, treeTrunkPosition.y, treeTrunkPosition.z + 1);
                    WorldCreateHandler.Instance.manager.AddUpdateBlock(backData_2);
                }

                if (i > treeHeight - 3)
                {
                    int isCreate = WorldRandTools.Range(0, 4);
                    if (isCreate == 1)
                    {
                        BlockTempBean leftData_1 = new(treeData.treeTrunk, DirectionEnum.Left, treeTrunkPosition.x - 2, treeTrunkPosition.y, treeTrunkPosition.z);
                        WorldCreateHandler.Instance.manager.AddUpdateBlock(leftData_1);
                    }
                    isCreate = WorldRandTools.Range(0, 4);
                    if (isCreate == 1)
                    {
                        BlockTempBean rightData_1 = new(treeData.treeTrunk, DirectionEnum.Right, treeTrunkPosition.x + 2, treeTrunkPosition.y, treeTrunkPosition.z);
                        WorldCreateHandler.Instance.manager.AddUpdateBlock(rightData_1);
                    }
                    isCreate = WorldRandTools.Range(0, 4);
                    if (isCreate == 1)
                    {
                        BlockTempBean forwardData_1 = new(treeData.treeTrunk, DirectionEnum.Forward, treeTrunkPosition.x, treeTrunkPosition.y, treeTrunkPosition.z + 2);
                        WorldCreateHandler.Instance.manager.AddUpdateBlock(forwardData_1);
                    }
                    isCreate = WorldRandTools.Range(0, 4);
                    if (isCreate == 1)
                    {
                        BlockTempBean backData_1 = new(treeData.treeTrunk, DirectionEnum.Back, treeTrunkPosition.x, treeTrunkPosition.y, treeTrunkPosition.z - 2);
                        WorldCreateHandler.Instance.manager.AddUpdateBlock(backData_1);
                    }
                }

                BlockTempBean blockData = new(treeData.treeTrunk, treeTrunkPosition.x, treeTrunkPosition.y, treeTrunkPosition.z);
                WorldCreateHandler.Instance.manager.AddUpdateBlock(blockData);

                BlockTempBean leftData = new(treeData.treeTrunk, treeTrunkPosition.x - 1, treeTrunkPosition.y, treeTrunkPosition.z);
                WorldCreateHandler.Instance.manager.AddUpdateBlock(leftData);

                BlockTempBean rightData = new(treeData.treeTrunk, treeTrunkPosition.x + 1, treeTrunkPosition.y, treeTrunkPosition.z + 1);
                WorldCreateHandler.Instance.manager.AddUpdateBlock(rightData);

                BlockTempBean forwardData = new(treeData.treeTrunk, treeTrunkPosition.x, treeTrunkPosition.y, treeTrunkPosition.z + 1);
                WorldCreateHandler.Instance.manager.AddUpdateBlock(forwardData);

                BlockTempBean backData = new(treeData.treeTrunk, treeTrunkPosition.x, treeTrunkPosition.y, treeTrunkPosition.z - 1);
                WorldCreateHandler.Instance.manager.AddUpdateBlock(backData);
            }
        }
    }

    /// <summary>
    /// 增加世界之树
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="treeData"></param>
    public static void AddTreeForWorld(Vector3Int startPosition, BiomeForTreeData treeData)
    {
        Dictionary<Vector3Int, BlockTempBean> dicData = new();

        //概率小于万分之一的用RandomTools
        int seed = WorldCreateHandler.Instance.manager.GetWorldSeed();
        RandomTools randomTools = RandomUtil.GetRandom(seed, startPosition.x, startPosition.y, startPosition.z);
        //生成概率
        float addRate = randomTools.NextFloat();
        //高度
        int treeHeight = WorldRandTools.Range(treeData.minHeight, treeData.maxHeight);

        if (addRate < treeData.addRate)
        {
            for (int y = 0; y < treeHeight; y++)
            {
                Vector3Int treeTrunkPosition = startPosition + Vector3Int.up * (y + 1);
                int trunkRange = treeData.trunkRange;
                for (int x = -trunkRange; x <= trunkRange; x++)
                {
                    for (int z = -trunkRange; z <= trunkRange; z++)
                    {
                        if ((x == -trunkRange || x == trunkRange) && (z == -trunkRange || z == trunkRange))
                        {
                            continue;
                        }
                        else if ((x == -trunkRange + 1 || x == trunkRange - 1) && (z == -trunkRange || z == trunkRange))
                        {
                            continue;
                        }
                        else if ((z == -trunkRange + 1 || z == trunkRange - 1) && (x == -trunkRange || x == trunkRange))
                        {
                            continue;
                        }
                        else
                        {

                            Vector3Int tempTrunkPosition = new(treeTrunkPosition.x + x, treeTrunkPosition.y, treeTrunkPosition.z + z);
                            //生成树干
                            if (dicData.ContainsKey(tempTrunkPosition))
                            {
                                dicData.Remove(tempTrunkPosition);
                            }
                            BlockTempBean blockData = new(treeData.treeTrunk, tempTrunkPosition.x, tempTrunkPosition.y, tempTrunkPosition.z);
                            dicData.Add(tempTrunkPosition, blockData);
                        }

                        if ((x == -trunkRange || x == trunkRange || z == -trunkRange || z == trunkRange)
                            || ((x == -trunkRange + 1 || x == trunkRange - 1) && (z == -trunkRange || z == trunkRange))
                            || ((z == -trunkRange + 1 || z == trunkRange - 1) && (x == -trunkRange || x == trunkRange))
                            || ((x == -trunkRange + 2 || x == trunkRange - 2) && (z == -trunkRange || z == trunkRange))
                            || ((z == -trunkRange + 2 || z == trunkRange - 2) && (x == -trunkRange || x == trunkRange)))
                        {
                            //树盘
                            if (y == 0 || y == 1)
                            {
                                Vector3Int baseStartPosition = treeTrunkPosition + new Vector3Int(x, -1, z);
                                int baseWith = WorldRandTools.Range(3, 6);
                                for (int b = 0; b < baseWith; b++)
                                {

                                    DirectionEnum baseDirection =
                                        (x == -trunkRange ? DirectionEnum.Left
                                        : x == trunkRange ? DirectionEnum.Right
                                        : z == -trunkRange ? DirectionEnum.Forward
                                        : z == trunkRange ? DirectionEnum.Back
                                        : DirectionEnum.Left);

                                    int branchDirectionX = WorldRandTools.Range(0, 2);
                                    int branchDirectionY = WorldRandTools.Range(0, 2);
                                    int branchDirectionZ = WorldRandTools.Range(0, 2);

                                    int addPositionX;
                                    if (x == -trunkRange || x == trunkRange)
                                    {
                                        addPositionX = x > 0 ? 1 : -1;
                                    }
                                    else if (x == -trunkRange + 1 || x == trunkRange - 1 || x == -trunkRange + 2 || x == trunkRange - 2)
                                    {
                                        int tempRandom = WorldRandTools.Range(0, 2);
                                        if (tempRandom == 0)
                                        {
                                            addPositionX = x > 0 ? 1 : -1;
                                        }
                                        else
                                        {
                                            addPositionX = (branchDirectionX == 0 ? -1 : 1);
                                        }
                                    }
                                    else
                                    {
                                        addPositionX = (branchDirectionX == 0 ? -1 : 1);
                                    }

                                    int addPositionZ;
                                    if (z == -trunkRange || z == trunkRange)
                                    {
                                        addPositionZ = z > 0 ? 1 : -1;
                                    }
                                    else if (z == -trunkRange + 1 || z == trunkRange - 1 || z == -trunkRange + 2 || z == trunkRange - 2)
                                    {
                                        int tempRandom = WorldRandTools.Range(0, 2);
                                        if (tempRandom == 0)
                                        {
                                            addPositionZ = z > 0 ? 1 : -1;
                                        }
                                        else
                                        {
                                            addPositionZ = (branchDirectionZ == 0 ? -1 : 1);
                                        }
                                    }
                                    else
                                    {
                                        addPositionZ = (branchDirectionZ == 0 ? -1 : 1);
                                    }

                                    int addPositionY = (branchDirectionY == 0 ? -1 : 0);
                                    if (addPositionY == -1)
                                    {
                                        addPositionX = 0;
                                        addPositionZ = 0;
                                        baseDirection = DirectionEnum.UP;
                                    }

                                    Vector3Int addPosition = new(addPositionX, addPositionY, addPositionZ);
                                    baseStartPosition += addPosition;
                                    //干
                                    if (dicData.ContainsKey(baseStartPosition))
                                    {
                                        dicData.Remove(baseStartPosition);
                                    }
                                    BlockTempBean blockData = new(treeData.treeTrunk, baseDirection, baseStartPosition.x, baseStartPosition.y, baseStartPosition.z);
                                    dicData.Add(baseStartPosition, blockData);
                                }
                            }

                            //枝
                            if (y > treeHeight / 2)
                            {

                                int addBranchRate = WorldRandTools.Range(0, 3);
                                if (addBranchRate == 0)
                                {
                                    int branchWith = WorldRandTools.Range(0, treeHeight / 2) + treeHeight / 4;
                                    Vector3Int branchStartPosition = treeTrunkPosition + new Vector3Int(x, 0, z);

                                    for (int b = 0; b < branchWith; b++)
                                    {
                                        DirectionEnum branchDirection =
                                            (x == -trunkRange ? DirectionEnum.Left
                                            : x == trunkRange ? DirectionEnum.Right
                                            : z == -trunkRange ? DirectionEnum.Forward
                                            : z == trunkRange ? DirectionEnum.Back
                                            : DirectionEnum.Left);

                                        int branchDirectionX = WorldRandTools.Range(0, 2);
                                        int branchDirectionY = WorldRandTools.Range(0, 4);
                                        int branchDirectionZ = WorldRandTools.Range(0, 2);

                                        int addPositionX;
                                        if (x == -trunkRange || x == trunkRange)
                                        {
                                            addPositionX = x > 0 ? 1 : -1;
                                        }
                                        else if (x == -trunkRange + 1 || x == trunkRange - 1 || x == -trunkRange + 2 || x == trunkRange - 2)
                                        {
                                            int tempRandom = WorldRandTools.Range(0, 2);
                                            if (tempRandom == 0)
                                            {
                                                addPositionX = x > 0 ? 1 : -1;
                                            }
                                            else
                                            {
                                                addPositionX = (branchDirectionX == 0 ? -1 : 1);
                                            }
                                        }
                                        else
                                        {
                                            addPositionX = (branchDirectionX == 0 ? -1 : 1);
                                        }

                                        int addPositionZ;
                                        if (z == -trunkRange || z == trunkRange)
                                        {
                                            addPositionZ = z > 0 ? 1 : -1;
                                        }
                                        else if (z == -trunkRange + 1 || z == trunkRange - 1 || z == -trunkRange + 2 || z == trunkRange - 2)
                                        {
                                            int tempRandom = WorldRandTools.Range(0, 2);
                                            if (tempRandom == 0)
                                            {
                                                addPositionZ = z > 0 ? 1 : -1;
                                            }
                                            else
                                            {
                                                addPositionZ = (branchDirectionZ == 0 ? -1 : 1);
                                            }
                                        }
                                        else
                                        {
                                            addPositionZ = (branchDirectionZ == 0 ? -1 : 1);
                                        }

                                        int addPositionY = (branchDirectionY == 0 ? 1 : 0);

                                        Vector3Int addPosition = new(addPositionX, addPositionY, addPositionZ);
                                        branchStartPosition += addPosition;
                                        //干
                                        if (dicData.ContainsKey(branchStartPosition))
                                        {
                                            dicData.Remove(branchStartPosition);
                                        }
                                        BlockTempBean blockData = new(treeData.treeTrunk, branchDirection, branchStartPosition.x, branchStartPosition.y, branchStartPosition.z);
                                        dicData.Add(branchStartPosition, blockData);
                                        //叶
                                        if (b % 4 == 0)
                                        {
                                            int leavesRange = 2;
                                            for (int leavesX = -leavesRange; leavesX <= leavesRange; leavesX++)
                                            {
                                                for (int leavesY = -leavesRange; leavesY <= leavesRange; leavesY++)
                                                {
                                                    for (int leavesZ = -leavesRange; leavesZ <= leavesRange; leavesZ++)
                                                    {
                                                        if (((leavesX == leavesRange || leavesX == -leavesRange) && (leavesY == leavesRange || leavesY == -leavesRange))
                                                            || ((leavesY == leavesRange || leavesY == -leavesRange) && (leavesZ == leavesRange || leavesZ == -leavesRange))
                                                            || ((leavesX == leavesRange || leavesX == -leavesRange) && (leavesZ == leavesRange || leavesZ == -leavesRange)))
                                                        {
                                                            continue;
                                                        }
                                                        Vector3Int leavesPosition = branchStartPosition + new Vector3Int(leavesX, leavesY, leavesZ);
                                                        if (!dicData.ContainsKey(leavesPosition))
                                                        {
                                                            BlockTempBean blockLeavesData = new(treeData.treeLeaves, leavesPosition.x, leavesPosition.y, leavesPosition.z);
                                                            dicData.Add(leavesPosition, blockLeavesData);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        foreach (var item in dicData.Values)
        {
            WorldCreateHandler.Instance.manager.AddUpdateBlock(item);
        }

    }

    /// <summary>
    /// 增加环形树
    /// </summary>
    public static void AddTreeForRing(uint randomData, Vector3Int startPosition, BiomeForTreeData treeData)
    {
        //生成概率
        float addRate = WorldRandTools.GetValue(startPosition, randomData);

        if (addRate < treeData.addRate)
        {
            //高度
            int treeHeight = WorldRandTools.Range(treeData.minHeight, treeData.maxHeight);
            for (int i = 0; i < treeHeight + 2; i++)
            {
                Vector3Int treeTrunkPosition = startPosition + Vector3Int.up * (i + 1);
                //生成树干
                if (i < treeHeight)
                {
                    BlockTempBean blockData = new(treeData.treeTrunk, treeTrunkPosition.x, treeTrunkPosition.y, treeTrunkPosition.z);
                    WorldCreateHandler.Instance.manager.AddUpdateBlock(blockData);
                }
                if (i > 4 && i % 2 == 0)
                {
                    //最大范围
                    int range = treeData.leavesRange;

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
                                int randomLeaves = WorldRandTools.Range(0, 3);
                                if (randomLeaves == 0)
                                    continue;
                            }
                            BlockTempBean blockData = new(treeData.treeLeaves, treeTrunkPosition.x + x, treeTrunkPosition.y, treeTrunkPosition.z + z);
                            WorldCreateHandler.Instance.manager.AddUpdateBlock(blockData);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 增加植物
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="weedData"></param>
    public static void AddPlant(uint randomData, Vector3Int startPosition, BiomeForPlantData plantData)
    {
        //生成概率
        float addRate = WorldRandTools.GetValue(startPosition, randomData);
        int weedTypeNumber = WorldRandTools.Range(0, plantData.listPlantType.Count);
        if (addRate < plantData.addRate)
        {
            BlockTempBean blockData = new(plantData.listPlantType[weedTypeNumber], startPosition.x, startPosition.y + 1, startPosition.z);
            WorldCreateHandler.Instance.manager.AddUpdateBlock(blockData);
        }
    }

    /// <summary>
    ///  增加仙人掌
    /// </summary>
    /// <param name="randomData"></param>
    /// <param name="startPosition"></param>
    /// <param name="cactusData"></param>
    public static void AddCactus(uint randomData, Vector3Int startPosition, BiomeForCactusData cactusData)
    {
        //生成概率
        float addRate = WorldRandTools.GetValue(startPosition, randomData);
        //高度
        int treeHeight = WorldRandTools.Range(cactusData.minHeight, cactusData.maxHeight);

        if (addRate < cactusData.addRate)
        {
            for (int i = 0; i < treeHeight; i++)
            {
                Vector3Int treeTrunkPosition = startPosition + Vector3Int.up * (i + 1);
                //生成树干
                if (i < treeHeight)
                {
                    BlockTempBean blockData = new(cactusData.cactusType, treeTrunkPosition.x, treeTrunkPosition.y, treeTrunkPosition.z);
                    WorldCreateHandler.Instance.manager.AddUpdateBlock(blockData);
                }
            }
        }
    }


    /// <summary>
    /// 增加鲜花
    /// </summary>
    /// <param name="randomData"></param>
    /// <param name="startPosition"></param>
    /// <param name="flowerData"></param>
    public static void AddFlower(uint randomData, Vector3Int startPosition, BiomeForFlowerData flowerData)
    {
        float addRate = WorldRandTools.GetValue(startPosition, randomData);
        int flowerTypeNumber = WorldRandTools.Range(0, flowerData.listFlowerType.Count);
        if (addRate < flowerData.addRate)
        {
            BlockTempBean blockData = new(flowerData.listFlowerType[flowerTypeNumber], startPosition.x, startPosition.y + 1, startPosition.z);
            WorldCreateHandler.Instance.manager.AddUpdateBlock(blockData);
        }
    }

    /// <summary>
    /// 增加水池
    /// </summary>
    /// <param name="randomData"></param>
    /// <param name="startPosition"></param>
    /// <param name="riverData"></param>
    public static void AddWaterPool(uint randomData, Vector3Int startPosition, BiomeForWaterPoolData riverData)
    {
        //生成概率
        float addRate = WorldRandTools.GetValue(startPosition, randomData);

        if (addRate < riverData.addRate)
        {
            //高度
            int depth = WorldRandTools.Range(riverData.minDepth, riverData.maxDepth, startPosition, 1);
            int size = WorldRandTools.Range(riverData.minSize, riverData.maxSize, startPosition, 101);

            Vector3Int tempPosition = startPosition;
            for (int z = -size; z <= size; z++)
            {
                for (int y = -depth; y <= 0; y++)
                {
                    for (int x = -size; x <= size; x++)
                    {
                        Vector3Int currentPosition = tempPosition + new Vector3Int(x, y, z);
                        float dis = Vector3.Distance(currentPosition, startPosition);
                        if (tempPosition.y <= 0 || dis >= size)
                        {
                            continue;
                        }
                        BlockTempBean blockData = new(BlockTypeEnum.None, currentPosition.x, currentPosition.y, currentPosition.z);
                        WorldCreateHandler.Instance.manager.AddUpdateBlock(blockData);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 生成枯木
    /// </summary>
    /// <param name="randomData"></param>
    /// <param name="startPosition"></param>
    public static void AddDeadwood(uint randomData,float addRate, Vector3Int startPosition)
    {
        //生成概率
        float addRateRandom = WorldRandTools.GetValue(startPosition, randomData);
        if (addRateRandom < addRate)
        {
            //高度
            int treeHeight = WorldRandTools.Range(1, 4);

            Vector3Int treeDataPosition = startPosition;
            for (int i = 0; i < treeHeight; i++)
            {
                BlockTempBean blockData = new(BlockTypeEnum.WoodDead, DirectionEnum.UP, treeDataPosition.x, treeDataPosition.y + i + 1, treeDataPosition.z);
                WorldCreateHandler.Instance.manager.AddUpdateBlock(blockData);
            }
        }
    }

    /// <summary>
    /// 增加建筑
    /// </summary>
    /// <param name="addRate"></param>
    /// <param name="randomData"></param>
    /// <param name="startPosition"></param>
    /// <param name="buildingType"></param>
    public static bool AddBuilding(float addRate, uint randomData, Vector3Int startPosition, BuildingTypeEnum buildingType)
    {
        float randomRate;
        if (addRate < 0.00001f)
        {
            //概率小于万分之一的用RandomTools
            int seed = WorldCreateHandler.Instance.manager.GetWorldSeed();
            RandomTools randomTools = RandomUtil.GetRandom(seed, startPosition.x, startPosition.y, startPosition.z);
            //生成概率
            randomRate = randomTools.NextFloat();
        }
        else
        {
            randomRate = WorldRandTools.GetValue(startPosition, randomData);
        }
        if (randomRate < addRate)
        {
            BuildingInfoBean buildingInfo = BiomeHandler.Instance.manager.GetBuildingInfo(buildingType);

            List<BuildingBean> listBuildingData = buildingInfo.listBuildingData;

            int randomAngle = WorldRandTools.Range(0, 4, startPosition) * 90;

            for (int i = 0; i < listBuildingData.Count; i++)
            {
                BuildingBean buildingData = listBuildingData[i];
                Vector3Int targetPosition = startPosition + buildingData.GetPosition();
                float createRate = WorldRandTools.GetValue(targetPosition);

                if (buildingData.randomRate == 0 || createRate < buildingData.randomRate)
                {
                    VectorUtil.GetRotatedPosition(startPosition, targetPosition, new Vector3(0, randomAngle, 0));
                    BlockTempBean blockData = new((BlockTypeEnum)buildingData.blockId, (DirectionEnum)buildingData.direction, targetPosition.x, targetPosition.y, targetPosition.z);
                    WorldCreateHandler.Instance.manager.AddUpdateBlock(blockData);
                }
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// 增加山洞
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="caveData"></param>
    public static void AddCave(Chunk chunk, BiomeMapData[,] mapData, BiomeForCaveData caveData)
    {
        float frequency = 2;
        float amplitude = 1;

        FastNoise fastNoise = BiomeHandler.Instance.fastNoise;
        int caveNumber = WorldRandTools.Range(0, 10, chunk.chunkData.positionForWorld);
        for (int i = 8; i < caveNumber; i++)
        {
            int positionX = WorldRandTools.Range(1, chunk.chunkData.chunkWidth);
            int positionZ = WorldRandTools.Range(1, chunk.chunkData.chunkWidth);
            BiomeMapData biomeMapData = mapData[positionX, positionZ];
            int positionY = WorldRandTools.Range(1, biomeMapData.maxHeight);
            Vector3Int startPosition = new Vector3Int(positionX, positionY, positionZ) + chunk.chunkData.positionForWorld;

            int caveDepth = WorldRandTools.Range(caveData.minDepth, caveData.maxDepth);
            int lastType = 0;
            for (int f = 0; f < caveDepth; f++)
            {

                int caveRange = WorldRandTools.Range(caveData.minSize, caveData.maxSize);
                if (startPosition.y < 5)
                {
                    continue;
                }

                float offsetX = fastNoise.GetPerlin(0, startPosition.y * frequency, startPosition.z * frequency) * amplitude;
                float offsetY = fastNoise.GetPerlin(startPosition.x * frequency, 0, startPosition.z * frequency) * amplitude;
                float offsetZ = fastNoise.GetPerlin(startPosition.x * frequency, startPosition.y * frequency, 0) * amplitude;

                Vector3 offset = new Vector3(offsetX, offsetY, offsetZ).normalized;
                Vector3Int offsetInt = new Vector3Int(GetCaveDirection(offset.x), GetCaveDirection(offset.y), GetCaveDirection(offset.z));
                startPosition += offsetInt;

                if (startPosition.y <= 3 || startPosition.y > biomeMapData.maxHeight)
                    break;

                float absOffsetX = Mathf.Abs(offsetX);
                float absOffsetY = Mathf.Abs(offsetY);
                float absOffsetZ = Mathf.Abs(offsetZ);
                if (absOffsetX > absOffsetY && absOffsetX > absOffsetZ)
                {
                    AddCaveRange(startPosition, caveRange, 1, lastType);
                    lastType = 1;
                }
                else if (absOffsetY > absOffsetX && absOffsetY > absOffsetZ)
                {
                    AddCaveRange(startPosition, caveRange, 2, lastType);
                    lastType = 2;
                }
                else if (absOffsetZ > absOffsetX && absOffsetZ > absOffsetY)
                {
                    AddCaveRange(startPosition, caveRange, 3, lastType);
                    lastType = 3;
                }
            }
        }
    }

    protected static int GetCaveDirection(float data)
    {
        if (data > 0.5f)
        {
            return 1;
        }
        else if (data < -0.5f)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    protected static void AddCaveRange(Vector3Int startPosition, int caveRange, int type, int lastType)
    {
        if (type == lastType)
        {
            for (int a = -caveRange; a <= caveRange; a++)
            {
                for (int b = -caveRange; b <= caveRange; b++)
                {

                    Vector3Int blockPosition;
                    switch (type)
                    {
                        //x
                        case 1:
                            blockPosition = new Vector3Int(startPosition.x, startPosition.y + a, startPosition.z + b);
                            break;
                        //y
                        case 2:
                            blockPosition = new Vector3Int(startPosition.x + a, startPosition.y, startPosition.z + b);
                            break;
                        //z
                        case 3:
                            blockPosition = new Vector3Int(startPosition.x + a, startPosition.y + b, startPosition.z);
                            break;

                        default:
                            blockPosition = new Vector3Int(startPosition.x, startPosition.y, startPosition.z);
                            break;
                    }
                    float disTemp = Vector3Int.Distance(blockPosition, startPosition);
                    if (blockPosition.y <= 3 || disTemp >= caveRange - 0.5f)
                    {
                        continue;
                    }
                    BlockTempBean blockTemp = new BlockTempBean(BlockTypeEnum.None, blockPosition.x, blockPosition.y, blockPosition.z);
                    WorldCreateHandler.Instance.manager.AddUpdateBlock(blockTemp);
                }
            }
        }
        else
        {
            //如果是90度转角
            for (int a = -caveRange; a <= caveRange; a++)
            {
                for (int b = -caveRange; b <= caveRange; b++)
                {
                    for (int c = -caveRange; c <= caveRange; c++)
                    {
                        Vector3Int blockPosition = new Vector3Int(startPosition.x + a, startPosition.y + b, startPosition.z + c);
                        BlockTempBean blockTemp = new BlockTempBean(BlockTypeEnum.None, blockPosition.x, blockPosition.y, blockPosition.z);
                        float disTemp = Vector3Int.Distance(blockPosition, startPosition);
                        if (blockTemp.worldY <= 3 || disTemp >= caveRange - 0.5f)
                        {
                            continue;
                        }
                        WorldCreateHandler.Instance.manager.AddUpdateBlock(blockTemp);
                    }
                }
            }
        }

    }
}