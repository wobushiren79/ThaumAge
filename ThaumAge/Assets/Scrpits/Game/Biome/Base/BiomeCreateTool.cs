using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BiomeCreateTool
{
    public struct BiomeForTreeData
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

    public struct BiomeForPlantData
    {
        public int addRateMin;
        public int addRateMax;
        public List<BlockTypeEnum> listPlantType;
    }

    public struct BiomeForCactusData
    {
        public int addRateMin;
        public int addRateMax;
        public int minHeight;
        public int maxHeight;
        public BlockTypeEnum cactusType;
    }

    public struct BiomeForFlowerData
    {
        public int addRateMin;
        public int addRateMax;
        public float flowerRange;
        public List<BlockTypeEnum> listFlowerType;
    }

    public struct BiomeForCaveData
    {
        public int addRateMin;
        public int addRateMax;
        public int minDepth;
        public int maxDepth;
        public int offset;
        public int size;

    }

    /// <summary>
    /// 增加普通的树
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="treeData"></param>
    public static void AddTree(Vector3Int startPosition, BiomeForTreeData treeData)
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
    public static void AddBigTree(Vector3Int startPosition, BiomeForTreeData treeData)
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
    /// 增加世界之树
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="treeData"></param>
    public static void AddWorldTree(Vector3Int startPosition, BiomeForTreeData treeData)
    {
        Dictionary<Vector3Int, BlockBean> dicData = new Dictionary<Vector3Int, BlockBean>();
        int worldSeed = WorldCreateHandler.Instance.manager.GetWorldSeed();
        RandomTools random = RandomUtil.GetRandom(worldSeed + 3, startPosition.x, startPosition.y, startPosition.z);
        //生成概率
        int addRate = random.NextInt(treeData.addRateMax);
        //高度
        int treeHeight = random.NextInt(treeData.maxHeight - treeData.minHeight) + treeData.minHeight;

        if (addRate < treeData.addRateMin)
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

                            Vector3Int tempTrunkPosition = treeTrunkPosition + new Vector3Int(x, 0, z);
                            //生成树干
                            if (dicData.TryGetValue(tempTrunkPosition, out BlockBean valueTrunk))
                            {
                                dicData.Remove(tempTrunkPosition);
                            }
                            BlockBean blockData = new BlockBean(treeData.treeTrunk, tempTrunkPosition);
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
                                int baseWith = random.NextInt(3) + 3;
                                for (int b = 0; b < baseWith; b++)
                                {

                                    DirectionEnum baseDirection =
                                        (x == -trunkRange ? DirectionEnum.Left
                                        : x == trunkRange ? DirectionEnum.Right
                                        : z == -trunkRange ? DirectionEnum.Forward
                                        : z == trunkRange ? DirectionEnum.Back
                                        : DirectionEnum.Left);

                                    int branchDirectionX = random.NextInt(2);
                                    int branchDirectionY = random.NextInt(2);
                                    int branchDirectionZ = random.NextInt(2);

                                    int addPositionX;
                                    if (x == -trunkRange || x == trunkRange)
                                    {
                                        addPositionX = x > 0 ? 1 : -1;
                                    }
                                    else if (x == -trunkRange + 1 || x == trunkRange - 1 || x == -trunkRange + 2 || x == trunkRange - 2)
                                    {
                                        int tempRandom = random.NextInt(2);
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
                                        int tempRandom = random.NextInt(2);
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

                                    Vector3Int addPosition = new Vector3Int(addPositionX, addPositionY, addPositionZ);
                                    baseStartPosition += addPosition;
                                    //干
                                    if (dicData.TryGetValue(baseStartPosition, out BlockBean valueTrunk))
                                    {
                                        dicData.Remove(baseStartPosition);
                                    }
                                    BlockBean blockData = new BlockBean(treeData.treeTrunk, baseStartPosition, baseDirection);
                                    dicData.Add(baseStartPosition, blockData);
                                }
                            }

                            //枝
                            if (y > treeHeight / 2)
                            {

                                int addBranchRate = random.NextInt(3);
                                if (addBranchRate == 0)
                                {
                                    int branchWith = random.NextInt(treeHeight / 2) + treeHeight / 4;
                                    Vector3Int branchStartPosition = treeTrunkPosition + new Vector3Int(x, 0, z);

                                    for (int b = 0; b < branchWith; b++)
                                    {
                                        DirectionEnum branchDirection =
                                            (x == -trunkRange ? DirectionEnum.Left
                                            : x == trunkRange ? DirectionEnum.Right
                                            : z == -trunkRange ? DirectionEnum.Forward
                                            : z == trunkRange ? DirectionEnum.Back
                                            : DirectionEnum.Left);

                                        int branchDirectionX = random.NextInt(2);
                                        int branchDirectionY = random.NextInt(4);
                                        int branchDirectionZ = random.NextInt(2);

                                        int addPositionX;
                                        if (x == -trunkRange || x == trunkRange)
                                        {
                                            addPositionX = x > 0 ? 1 : -1;
                                        }
                                        else if (x == -trunkRange + 1 || x == trunkRange - 1 || x == -trunkRange + 2 || x == trunkRange - 2)
                                        {
                                            int tempRandom = random.NextInt(2);
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
                                            int tempRandom = random.NextInt(2);
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

                                        Vector3Int addPosition = new Vector3Int(addPositionX, addPositionY, addPositionZ);
                                        branchStartPosition += addPosition;
                                        //干
                                        if (dicData.TryGetValue(branchStartPosition, out BlockBean valueTrunk))
                                        {
                                            dicData.Remove(branchStartPosition);
                                        }
                                        BlockBean blockData = new BlockBean(treeData.treeTrunk, branchStartPosition, branchDirection);
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
                                                        if (!dicData.TryGetValue(leavesPosition, out BlockBean valueLeaves))
                                                        {
                                                            BlockBean blockLeavesData = new BlockBean(treeData.treeLeaves, leavesPosition);
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
            WorldCreateHandler.Instance.manager.listUpdateBlock.Add(item);
        }

    }

    /// <summary>
    /// 增加植物
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="weedData"></param>
    public static void AddPlant(Vector3Int startPosition, BiomeForPlantData plantData)
    {
        int worldSeed = WorldCreateHandler.Instance.manager.GetWorldSeed();
        RandomTools random = RandomUtil.GetRandom(worldSeed + 101, startPosition.x, startPosition.y, startPosition.z);
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
    public static void AddCactus(Vector3Int startPosition, BiomeForCactusData cactusData)
    {
        int worldSeed = WorldCreateHandler.Instance.manager.GetWorldSeed();
        RandomTools random = RandomUtil.GetRandom(worldSeed + 201, startPosition.x, startPosition.y, startPosition.z);
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
    public static void AddFlower(Vector3Int startPosition, BiomeForFlowerData flowerData)
    {
        int worldSeed = WorldCreateHandler.Instance.manager.GetWorldSeed();
        RandomTools random = RandomUtil.GetRandom(worldSeed + 301, startPosition.x, startPosition.y, startPosition.z);
        int addRate = random.NextInt(flowerData.addRateMax);
        int flowerTypeNumber = random.NextInt(flowerData.listFlowerType.Count);
        if (addRate < flowerData.addRateMin)
        {
            BlockBean blockData = new BlockBean(flowerData.listFlowerType[flowerTypeNumber], startPosition + Vector3Int.up);
            WorldCreateHandler.Instance.manager.listUpdateBlock.Add(blockData);
        }
    }


    /// <summary>
    /// 增加山洞
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="caveData"></param>
    public static void AddCave(Vector3Int startPosition, BiomeForCaveData caveData)
    {
        int worldSeed = WorldCreateHandler.Instance.manager.GetWorldSeed();
        RandomTools random = RandomUtil.GetRandom(worldSeed + 901, startPosition.x, startPosition.y, startPosition.z);
        int addRate = random.NextInt(caveData.addRateMax);
        if (addRate < caveData.addRateMin)
        {
            int caveSize = caveData.size;
            int offset = caveData.offset;
            int depth = random.NextInt(caveData.minDepth, caveData.maxDepth);
            Queue<Vector3Int> path = new Queue<Vector3Int>();
            Vector3Int pathPosition = startPosition;

            int directionX = (random.NextInt(2) == 0 ? 1 : -1);
            int directionZ = (random.NextInt(2) == 0 ? 1 : -1);

            for (int d = 0; d < depth; d++)
            {
                int randomY = random.NextInt(4);
                int addPositionX = directionX * offset;
                int addPositionZ = directionZ * offset;
                int addPositionY = randomY == 0 ? 0 : - offset;

                pathPosition += new Vector3Int(addPositionX, -addPositionY, addPositionZ);

                path.Enqueue(pathPosition);
            }

            //遍历每个路径点
            while (path.Count > 0)
            {
                pathPosition = path.Dequeue();
                for (int z = -caveSize; z <= caveSize; z++)
                {
                    for (int y = -caveSize; y <= caveSize; y++)
                    {
                        for (int x = -caveSize; x <= caveSize; x++)
                        {

                            Vector3Int tempPosition = pathPosition + new Vector3Int(x, y, z);
                            float dis = Vector3.Distance(tempPosition, pathPosition);
                            if (tempPosition.y <= 0 || dis >= caveSize)
                            {
                                continue;
                            }
                            BlockBean blockData = new BlockBean(BlockTypeEnum.None, tempPosition);
                            WorldCreateHandler.Instance.manager.listUpdateBlock.Add(blockData);
                        }
                    }
                }
            }

        }

        //if (addRate < caveData.addRateMin)
        //{
        //    int caveSize = caveData.size;
        //    Vector3Int cavePosition= startPosition;

        //    byte[,,] caveMap = new byte[16, 48, 16];
        //    Queue<Vector3Int> path = new Queue<Vector3Int>();
        //    int depth = random.NextInt(caveData.minDepth, caveData.maxDepth);
        //    for (int i = 0; i < depth; i++)
        //    {
        //        path.Enqueue(new Vector3Int(
        //            random.NextInt(2, 13),
        //            44 - (i * 4),
        //            random.NextInt(2, 13)
        //        ));
        //    }
        //    Vector3Int currentPos = Vector3Int.zero;
        //    Vector3Int nextPos = path.Dequeue();
        //    float d = 0;
        //    while (path.Count > 0)
        //    {
        //        currentPos = nextPos;
        //        nextPos = path.Dequeue();
        //        float size = Mathf.Lerp(caveSize, 0.75f, d / depth);

        //        for (int i = 0; i < 16; ++i)
        //        {
        //            float lerpPos = i / 15f;
        //            Vector3 lerped = Vector3.Lerp(currentPos, nextPos, lerpPos);
        //            Vector3Int p = new Vector3Int((int)lerped.x, (int)lerped.y, (int)lerped.z);
        //            for (int z = -caveSize; z <= caveSize; ++z)
        //            {
        //                for (int y = -caveSize; y <= caveSize; ++y)
        //                {
        //                    for (int x = -caveSize; x <= caveSize; ++x)
        //                    {
        //                        Vector3Int b = new Vector3Int(p.x + x, p.y + y, p.z + z);
        //                        if (Vector3Int.Distance(p, b) > size) continue;
        //                        if (b.x < 0 || b.x > 15) continue;
        //                        if (b.y < 0 || b.y > 47) continue;
        //                        if (b.z < 0 || b.z > 15) continue;

        //                        caveMap[b.x, b.y, b.z] = (byte)1;
        //                    }
        //                }
        //            }
        //        }
        //        d++;
        //    }
        //    for (int z = 0; z < 16; ++z)
        //    {
        //        for (int y = 0; y < 48; ++y)
        //        {
        //            for (int x = 0; x < 16; ++x)
        //            {
        //                if (caveMap[x, y, z] == 1)
        //                {            
        //                    Vector3Int tempPosition = cavePosition + new Vector3Int(x, y - 48, z);
        //                    if (tempPosition.y <= 0)
        //                    {
        //                        continue;
        //                    }
        //                    BlockBean blockData = new BlockBean(BlockTypeEnum.None, tempPosition);
        //                    WorldCreateHandler.Instance.manager.listUpdateBlock.Add(blockData);
        //                }
        //            }
        //        }
        //    }
        //}

    }
}