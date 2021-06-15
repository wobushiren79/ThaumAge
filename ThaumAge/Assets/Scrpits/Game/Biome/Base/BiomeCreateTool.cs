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
    }

    /// <summary>
    /// 增加普通的树
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="treeData"></param>
    public static void AddTree(Vector3Int startPosition, BiomeForTreeData treeData)
    {
        //生成概率
        float addRate = WorldRandTools.GetValue(startPosition);
        //高度
        int treeHeight = WorldRandTools.Range(treeData.minHeight, treeData.maxHeight);

        if (addRate < treeData.addRate)
        {
            for (int i = 0; i < treeHeight + 2; i++)
            {
                Vector3Int treeTrunkPosition = startPosition + Vector3Int.up * (i + 1);
                //生成树干
                if (i < treeHeight)
                {
                    BlockBean blockData = new BlockBean(treeTrunkPosition, treeData.treeTrunk);
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
                            BlockBean blockData = new BlockBean(treeTrunkPosition + new Vector3Int(x, 0, z), treeData.treeLeaves);
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
    public static void AddTreeForBig(Vector3Int startPosition, BiomeForTreeData treeData)
    {
        //生成概率
        float addRate = WorldRandTools.GetValue(startPosition);
        //高度
        int treeHeight = WorldRandTools.Range(treeData.minHeight, treeData.maxHeight);

        if (addRate < treeData.addRate)
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
                            int leavesRate = WorldRandTools.Range(0, 4);
                            if (leavesRate == 0)
                                continue;
                        }
                        BlockBean blockData = new BlockBean(treeTrunkPosition + new Vector3Int(x, 0, z), treeData.treeLeaves);
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
                        BlockBean leftData_1 = new BlockBean(treeTrunkPosition + Vector3Int.left * 2, treeData.treeTrunk, DirectionEnum.Left);
                        WorldCreateHandler.Instance.manager.AddUpdateBlock(leftData_1);

                        BlockBean rightData_1 = new BlockBean(treeTrunkPosition + Vector3Int.right * 2, treeData.treeTrunk, DirectionEnum.Right);
                        WorldCreateHandler.Instance.manager.AddUpdateBlock(rightData_1);

                        BlockBean forwardData_1 = new BlockBean(treeTrunkPosition + Vector3Int.forward * 2, treeData.treeTrunk, DirectionEnum.Forward);
                        WorldCreateHandler.Instance.manager.AddUpdateBlock(forwardData_1);

                        BlockBean backData_1 = new BlockBean(treeTrunkPosition + Vector3Int.back * 2, treeData.treeTrunk, DirectionEnum.Back);
                        WorldCreateHandler.Instance.manager.AddUpdateBlock(backData_1);
                    }


                    BlockBean leftData_2 = new BlockBean(treeTrunkPosition + new Vector3Int(1, 0, 1), treeData.treeTrunk);
                    WorldCreateHandler.Instance.manager.AddUpdateBlock(leftData_2);

                    BlockBean rightData_2 = new BlockBean(treeTrunkPosition + new Vector3Int(-1, 0, -1), treeData.treeTrunk);
                    WorldCreateHandler.Instance.manager.AddUpdateBlock(rightData_2);

                    BlockBean forwardData_2 = new BlockBean(treeTrunkPosition + new Vector3Int(1, 0, -1), treeData.treeTrunk);
                    WorldCreateHandler.Instance.manager.AddUpdateBlock(forwardData_2);

                    BlockBean backData_2 = new BlockBean(treeTrunkPosition + new Vector3Int(-1, 0, 1), treeData.treeTrunk);
                    WorldCreateHandler.Instance.manager.AddUpdateBlock(backData_2);
                }

                if (i > treeHeight - 3)
                {
                    int isCreate = WorldRandTools.Range(0, 4);
                    if (isCreate == 1)
                    {
                        BlockBean leftData_1 = new BlockBean(treeTrunkPosition + Vector3Int.left * 2, treeData.treeTrunk, DirectionEnum.Left);
                        WorldCreateHandler.Instance.manager.AddUpdateBlock(leftData_1);
                    }
                    isCreate = WorldRandTools.Range(0, 4);
                    if (isCreate == 1)
                    {
                        BlockBean rightData_1 = new BlockBean(treeTrunkPosition + Vector3Int.right * 2, treeData.treeTrunk, DirectionEnum.Right);
                        WorldCreateHandler.Instance.manager.AddUpdateBlock(rightData_1);
                    }
                    isCreate = WorldRandTools.Range(0, 4);
                    if (isCreate == 1)
                    {
                        BlockBean forwardData_1 = new BlockBean(treeTrunkPosition + Vector3Int.forward * 2, treeData.treeTrunk, DirectionEnum.Forward);
                        WorldCreateHandler.Instance.manager.AddUpdateBlock(forwardData_1);
                    }
                    isCreate = WorldRandTools.Range(0, 4);
                    if (isCreate == 1)
                    {
                        BlockBean backData_1 = new BlockBean(treeTrunkPosition + Vector3Int.back * 2, treeData.treeTrunk, DirectionEnum.Back);
                        WorldCreateHandler.Instance.manager.AddUpdateBlock(backData_1);
                    }
                }

                BlockBean blockData = new BlockBean(treeTrunkPosition, treeData.treeTrunk);
                WorldCreateHandler.Instance.manager.AddUpdateBlock(blockData);

                BlockBean leftData = new BlockBean(treeTrunkPosition + Vector3Int.left, treeData.treeTrunk);
                WorldCreateHandler.Instance.manager.AddUpdateBlock(leftData);

                BlockBean rightData = new BlockBean(treeTrunkPosition + Vector3Int.right, treeData.treeTrunk);
                WorldCreateHandler.Instance.manager.AddUpdateBlock(rightData);

                BlockBean forwardData = new BlockBean(treeTrunkPosition + Vector3Int.forward, treeData.treeTrunk);
                WorldCreateHandler.Instance.manager.AddUpdateBlock(forwardData);

                BlockBean backData = new BlockBean(treeTrunkPosition + Vector3Int.back, treeData.treeTrunk);
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
        Dictionary<Vector3Int, BlockBean> dicData = new Dictionary<Vector3Int, BlockBean>();

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

                            Vector3Int tempTrunkPosition = treeTrunkPosition + new Vector3Int(x, 0, z);
                            //生成树干
                            if (dicData.TryGetValue(tempTrunkPosition, out BlockBean valueTrunk))
                            {
                                dicData.Remove(tempTrunkPosition);
                            }
                            BlockBean blockData = new BlockBean(tempTrunkPosition, treeData.treeTrunk);
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

                                    Vector3Int addPosition = new Vector3Int(addPositionX, addPositionY, addPositionZ);
                                    baseStartPosition += addPosition;
                                    //干
                                    if (dicData.TryGetValue(baseStartPosition, out BlockBean valueTrunk))
                                    {
                                        dicData.Remove(baseStartPosition);
                                    }
                                    BlockBean blockData = new BlockBean(baseStartPosition, treeData.treeTrunk, baseDirection);
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

                                        Vector3Int addPosition = new Vector3Int(addPositionX, addPositionY, addPositionZ);
                                        branchStartPosition += addPosition;
                                        //干
                                        if (dicData.TryGetValue(branchStartPosition, out BlockBean valueTrunk))
                                        {
                                            dicData.Remove(branchStartPosition);
                                        }
                                        BlockBean blockData = new BlockBean(branchStartPosition, treeData.treeTrunk, branchDirection);
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
                                                            BlockBean blockLeavesData = new BlockBean(leavesPosition, treeData.treeLeaves);
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
    /// 增加植物
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="weedData"></param>
    public static void AddPlant(Vector3Int startPosition, BiomeForPlantData plantData)
    {
        //生成概率
        float addRate = WorldRandTools.GetValue(startPosition);
        int weedTypeNumber = WorldRandTools.Range(0, plantData.listPlantType.Count);
        if (addRate < plantData.addRate)
        {
            BlockBean blockData = new BlockBean(startPosition + Vector3Int.up, plantData.listPlantType[weedTypeNumber]);
            WorldCreateHandler.Instance.manager.AddUpdateBlock(blockData);
        }
    }

    /// <summary>
    /// 增加仙人掌
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="cactusData"></param>
    public static void AddCactus(Vector3Int startPosition, BiomeForCactusData cactusData)
    {
        //生成概率
        float addRate = WorldRandTools.GetValue(startPosition);
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
                    BlockBean blockData = new BlockBean(treeTrunkPosition, cactusData.cactusType);
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
            BlockBean blockData = new BlockBean(startPosition + Vector3Int.up, flowerData.listFlowerType[flowerTypeNumber]);
            WorldCreateHandler.Instance.manager.AddUpdateBlock(blockData);
        }
    }


    /// <summary>
    /// 增加山洞
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="caveData"></param>
    public static void AddCave(Vector3Int startPosition, BiomeForCaveData caveData)
    {
        float addRate = WorldRandTools.GetValue(startPosition);
        if (addRate < caveData.addRate)
        {

            int depth = WorldRandTools.Range(caveData.minDepth, caveData.maxDepth);

            Vector3Int pathPosition = startPosition;

            for (int d = 0; d < depth; d++)
            {

                int offset = (WorldRandTools.Range(3, 5, pathPosition));
                int caveSize = (WorldRandTools.Range(3, 5, pathPosition));
                int randomX = (WorldRandTools.Range(0, 2, pathPosition) == 0 ? 1 : -1);
                int randomY = (WorldRandTools.Range(0, 2, pathPosition));
                int randomZ = (WorldRandTools.Range(0, 2, pathPosition) == 0 ? 1 : -1);
                int addPositionX = randomX * offset;
                int addPositionZ = randomZ * offset;
                int addPositionY = (randomY == 0 ? -offset : 0);

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
                            BlockBean blockData = new BlockBean(tempPosition, BlockTypeEnum.None);
                            WorldCreateHandler.Instance.manager.AddUpdateBlock(blockData);
                        }
                    }
                }

                pathPosition += new Vector3Int(addPositionX, addPositionY, addPositionZ);
            }


        }
        //---------------------------------------------------------------------------------------------------
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
        //                     WorldCreateHandler.Instance.manager.AddUpdateBlock(blockData);
        //                }
        //            }
        //        }
        //    }
        //}

    }


    /// <summary>
    /// 增加建筑
    /// </summary>
    /// <param name="addRate"></param>
    /// <param name="randomData"></param>
    /// <param name="startPosition"></param>
    /// <param name="buildingType"></param>
    public static void AddBuilding(float addRate, uint randomData, Vector3Int startPosition, BuildingTypeEnum buildingType)
    {
        float randomRate = WorldRandTools.GetValue(startPosition, randomData);
        if (randomRate < addRate)
        {
            BuildingInfoBean buildingInfo = BiomeHandler.Instance.manager.GetBuildingInfo(buildingType);

            List<BuildingBean> listBuildingData = buildingInfo.listBuildingData;

            for (int i = 0; i < listBuildingData.Count; i++)
            {
                BuildingBean buildingData = listBuildingData[i];
                Vector3Int targetPosition = startPosition + buildingData.GetPosition();
                float createRate = WorldRandTools.GetValue(targetPosition);           
                if (buildingData.randomRate == 0 || createRate < buildingData.randomRate)
                {
                    BlockBean blockData = new BlockBean(targetPosition, (BlockTypeEnum)buildingData.blockId, (DirectionEnum)buildingData.direction);
                    WorldCreateHandler.Instance.manager.AddUpdateBlock(blockData);
                }
            }
        }
    }

}