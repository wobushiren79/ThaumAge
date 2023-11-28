using System;
using UnityEditor;
using UnityEngine;

public class BiomeCreateTreeTool
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

        public int minLeavesHeight;//树叶高度 用于柳条
        public int maxLeavesHeight;//树叶高度 用于柳条
    }

    /// <summary>
    ///  增加仙人掌
    /// </summary>
    /// <param name="randomData"></param>
    /// <param name="startPosition"></param>
    /// <param name="cactusData"></param>
    public static void AddCactus(uint randomData, Vector3Int startPosition, BiomeForTreeData cactusData)
    {
        //生成概率
        float addRate = WorldRandTools.GetValue(startPosition, randomData);

        if (addRate < cactusData.addRate)
        {
            //高度
            int treeHeight = WorldRandTools.Range(cactusData.minHeight, cactusData.maxHeight);
            for (int i = 0; i < treeHeight; i++)
            {
                Vector3Int treeTrunkPosition = startPosition + Vector3Int.up * (i + 1);
                //生成树干
                if (i < treeHeight)
                {
                    WorldCreateHandler.Instance.manager.AddUpdateBlock(treeTrunkPosition, cactusData.treeTrunk);
                }
            }
        }
    }

    /// <summary>
    /// 增加倒下的树干
    /// </summary>
    public static void AddTreeForFallDown(uint randomData, Vector3Int startPosition, BiomeForTreeData treeData)
    {
        //生成概率
        float addRate = WorldRandTools.GetValue(startPosition, randomData);

        if (addRate < treeData.addRate)
        {
            GetRandomBlockDirection(out BlockDirectionEnum randomBlockDirection, out Vector3Int randomAddPosition);
            //长度
            int treeHeight = WorldRandTools.Range(treeData.minHeight, treeData.maxHeight);
            for (int i = 0; i < treeHeight; i++)
            {
                Vector3Int treeTrunkPosition = startPosition + i * randomAddPosition;
                WorldCreateHandler.Instance.manager.AddUpdateBlock(treeTrunkPosition, treeData.treeTrunk, randomBlockDirection);
            }
        }
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
        addRate = 0;
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
                    WorldCreateHandler.Instance.manager.AddUpdateBlock(treeTrunkPosition, treeData.treeTrunk);
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
                            WorldCreateHandler.Instance.manager.AddUpdateBlock(treeTrunkPosition.x + x, treeTrunkPosition.y, treeTrunkPosition.z + z, treeData.treeLeaves);
                        }
                    }
                }
            }
        }
    }
    public static void AddTreeEditor(Vector3Int startPosition, BiomeForTreeData treeData)
    {
        //高度
        int treeHeight = WorldRandTools.Range(treeData.minHeight, treeData.maxHeight);
        for (int i = 0; i < treeHeight + 2; i++)
        {
            Vector3Int treeTrunkPosition = new(startPosition.x, startPosition.y + (i + 1), startPosition.z);
            //生成树干
            if (i < treeHeight)
            {
                WorldCreateHandler.Instance.manager.AddUpdateBlockForEditor(treeTrunkPosition, treeData.treeTrunk);
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
                        WorldCreateHandler.Instance.manager.AddUpdateBlockForEditor(treeTrunkPosition.x + x, treeTrunkPosition.y, treeTrunkPosition.z + z, treeData.treeLeaves);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 增加高树树 高度最低不能小于6
    /// </summary>
    public static void AddTreeForTallEditor(Vector3Int startPosition, BiomeForTreeData treeData)
    {
        //高度
        int treeHeight = WorldRandTools.Range(treeData.minHeight, treeData.maxHeight);
        for (int i = 0; i < treeHeight + 2; i++)
        {
            Vector3Int treeTrunkPosition = new(startPosition.x, startPosition.y + (i + 1), startPosition.z);
            //生成树干
            if (i < treeHeight)
            {
                WorldCreateHandler.Instance.manager.AddUpdateBlockForEditor(treeTrunkPosition, (int)treeData.treeTrunk);
            }
            //再靠近树叶的地方随机生成一个树枝
            if (i == treeHeight - 6)
            {
                GetRandomBlockDirection(out BlockDirectionEnum randomBlockDirection, out Vector3Int randomAddPosition);
                WorldCreateHandler.Instance.manager.AddUpdateBlockForEditor(treeTrunkPosition + randomAddPosition, treeData.treeTrunk, randomBlockDirection);
            }
            if (i > treeHeight - 4)
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
                        WorldCreateHandler.Instance.manager.AddUpdateBlockForEditor(treeTrunkPosition.x + x, treeTrunkPosition.y, treeTrunkPosition.z + z, treeData.treeLeaves);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 增加高树树 高度最低不能小于6
    /// </summary>
    public static void AddTreeForTall(uint randomData, Vector3Int startPosition, BiomeForTreeData treeData)
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
                    WorldCreateHandler.Instance.manager.AddUpdateBlock(treeTrunkPosition, treeData.treeTrunk);
                }
                //再靠近树叶的地方随机生成一个树枝
                if (i == treeHeight - 6)
                {
                    GetRandomBlockDirection(out BlockDirectionEnum randomBlockDirection, out Vector3Int randomAddPosition);
                    WorldCreateHandler.Instance.manager.AddUpdateBlock(treeTrunkPosition + randomAddPosition, treeData.treeTrunk, randomBlockDirection);
                }
                if (i > treeHeight - 4)
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
                            WorldCreateHandler.Instance.manager.AddUpdateBlock(treeTrunkPosition.x + x, treeTrunkPosition.y, treeTrunkPosition.z + z, treeData.treeLeaves);
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
                        WorldCreateHandler.Instance.manager.AddUpdateBlock(treeTrunkPosition.x + x, treeTrunkPosition.y, treeTrunkPosition.z + z, treeData.treeLeaves);
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
                        WorldCreateHandler.Instance.manager.AddUpdateBlock(treeTrunkPosition.x - 2, treeTrunkPosition.y, treeTrunkPosition.z, treeData.treeTrunk, BlockDirectionEnum.LeftForward);
                        WorldCreateHandler.Instance.manager.AddUpdateBlock(treeTrunkPosition.x + 2, treeTrunkPosition.y, treeTrunkPosition.z, treeData.treeTrunk, BlockDirectionEnum.RightForward);
                        WorldCreateHandler.Instance.manager.AddUpdateBlock(treeTrunkPosition.x - 2, treeTrunkPosition.y, treeTrunkPosition.z + 2, treeData.treeTrunk, BlockDirectionEnum.ForwardForward);
                        WorldCreateHandler.Instance.manager.AddUpdateBlock(treeTrunkPosition.x - 2, treeTrunkPosition.y, treeTrunkPosition.z - 2, treeData.treeTrunk, BlockDirectionEnum.BackForward);
                    }
                    WorldCreateHandler.Instance.manager.AddUpdateBlock(treeTrunkPosition.x + 1, treeTrunkPosition.y, treeTrunkPosition.z + 1, treeData.treeTrunk);
                    WorldCreateHandler.Instance.manager.AddUpdateBlock(treeTrunkPosition.x - 1, treeTrunkPosition.y, treeTrunkPosition.z - 1, treeData.treeTrunk);
                    WorldCreateHandler.Instance.manager.AddUpdateBlock(treeTrunkPosition.x + 1, treeTrunkPosition.y, treeTrunkPosition.z - 1, treeData.treeTrunk);
                    WorldCreateHandler.Instance.manager.AddUpdateBlock(treeTrunkPosition.x - 1, treeTrunkPosition.y, treeTrunkPosition.z + 1, treeData.treeTrunk);
                }

                if (i > treeHeight - 3)
                {
                    int isCreate = WorldRandTools.Range(0, 4);
                    if (isCreate == 1)
                    {
                        WorldCreateHandler.Instance.manager.AddUpdateBlock(treeTrunkPosition.x - 2, treeTrunkPosition.y, treeTrunkPosition.z, treeData.treeTrunk, BlockDirectionEnum.LeftForward);
                    }
                    isCreate = WorldRandTools.Range(0, 4);
                    if (isCreate == 1)
                    {
                        WorldCreateHandler.Instance.manager.AddUpdateBlock(treeTrunkPosition.x + 2, treeTrunkPosition.y, treeTrunkPosition.z, treeData.treeTrunk, BlockDirectionEnum.RightForward);
                    }
                    isCreate = WorldRandTools.Range(0, 4);
                    if (isCreate == 1)
                    {
                        WorldCreateHandler.Instance.manager.AddUpdateBlock(treeTrunkPosition.x, treeTrunkPosition.y, treeTrunkPosition.z + 2, treeData.treeTrunk, BlockDirectionEnum.ForwardForward);
                    }
                    isCreate = WorldRandTools.Range(0, 4);
                    if (isCreate == 1)
                    {
                        WorldCreateHandler.Instance.manager.AddUpdateBlock(treeTrunkPosition.x, treeTrunkPosition.y, treeTrunkPosition.z - 2, treeData.treeTrunk, BlockDirectionEnum.BackForward);
                    }
                }
                WorldCreateHandler.Instance.manager.AddUpdateBlock(treeTrunkPosition.x, treeTrunkPosition.y, treeTrunkPosition.z, treeData.treeTrunk);
                WorldCreateHandler.Instance.manager.AddUpdateBlock(treeTrunkPosition.x - 1, treeTrunkPosition.y, treeTrunkPosition.z, treeData.treeTrunk);
                WorldCreateHandler.Instance.manager.AddUpdateBlock(treeTrunkPosition.x + 1, treeTrunkPosition.y, treeTrunkPosition.z + 1, treeData.treeTrunk);
                WorldCreateHandler.Instance.manager.AddUpdateBlock(treeTrunkPosition.x, treeTrunkPosition.y, treeTrunkPosition.z + 1, treeData.treeTrunk);
                WorldCreateHandler.Instance.manager.AddUpdateBlock(treeTrunkPosition.x, treeTrunkPosition.y, treeTrunkPosition.z - 1, treeData.treeTrunk);
            }
        }
    }
    public static void AddTreeForBigEditor(Vector3Int startPosition, BiomeForTreeData treeData)
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
                    WorldCreateHandler.Instance.manager.AddUpdateBlockForEditor(treeTrunkPosition.x + x, treeTrunkPosition.y, treeTrunkPosition.z + z, treeData.treeLeaves);
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
                    WorldCreateHandler.Instance.manager.AddUpdateBlockForEditor(treeTrunkPosition.x - 2, treeTrunkPosition.y, treeTrunkPosition.z, treeData.treeTrunk, BlockDirectionEnum.LeftForward);
                    WorldCreateHandler.Instance.manager.AddUpdateBlockForEditor(treeTrunkPosition.x + 2, treeTrunkPosition.y, treeTrunkPosition.z, treeData.treeTrunk, BlockDirectionEnum.RightForward);
                    WorldCreateHandler.Instance.manager.AddUpdateBlockForEditor(treeTrunkPosition.x - 2, treeTrunkPosition.y, treeTrunkPosition.z + 2, treeData.treeTrunk, BlockDirectionEnum.ForwardForward);
                    WorldCreateHandler.Instance.manager.AddUpdateBlockForEditor(treeTrunkPosition.x - 2, treeTrunkPosition.y, treeTrunkPosition.z - 2, treeData.treeTrunk, BlockDirectionEnum.BackForward);
                }
                WorldCreateHandler.Instance.manager.AddUpdateBlockForEditor(treeTrunkPosition.x + 1, treeTrunkPosition.y, treeTrunkPosition.z + 1, treeData.treeTrunk);
                WorldCreateHandler.Instance.manager.AddUpdateBlockForEditor(treeTrunkPosition.x - 1, treeTrunkPosition.y, treeTrunkPosition.z - 1, treeData.treeTrunk);
                WorldCreateHandler.Instance.manager.AddUpdateBlockForEditor(treeTrunkPosition.x + 1, treeTrunkPosition.y, treeTrunkPosition.z - 1, treeData.treeTrunk);
                WorldCreateHandler.Instance.manager.AddUpdateBlockForEditor(treeTrunkPosition.x - 1, treeTrunkPosition.y, treeTrunkPosition.z + 1, treeData.treeTrunk);
            }

            if (i > treeHeight - 3)
            {
                int isCreate = WorldRandTools.Range(0, 4);
                if (isCreate == 1)
                {
                    WorldCreateHandler.Instance.manager.AddUpdateBlockForEditor(treeTrunkPosition.x - 2, treeTrunkPosition.y, treeTrunkPosition.z, treeData.treeTrunk, BlockDirectionEnum.LeftForward);
                }
                isCreate = WorldRandTools.Range(0, 4);
                if (isCreate == 1)
                {
                    WorldCreateHandler.Instance.manager.AddUpdateBlockForEditor(treeTrunkPosition.x + 2, treeTrunkPosition.y, treeTrunkPosition.z, treeData.treeTrunk, BlockDirectionEnum.RightForward);
                }
                isCreate = WorldRandTools.Range(0, 4);
                if (isCreate == 1)
                {
                    WorldCreateHandler.Instance.manager.AddUpdateBlockForEditor(treeTrunkPosition.x, treeTrunkPosition.y, treeTrunkPosition.z + 2, treeData.treeTrunk, BlockDirectionEnum.ForwardForward);
                }
                isCreate = WorldRandTools.Range(0, 4);
                if (isCreate == 1)
                {
                    WorldCreateHandler.Instance.manager.AddUpdateBlockForEditor(treeTrunkPosition.x, treeTrunkPosition.y, treeTrunkPosition.z - 2, treeData.treeTrunk, BlockDirectionEnum.BackForward);
                }
            }
            WorldCreateHandler.Instance.manager.AddUpdateBlockForEditor(treeTrunkPosition.x, treeTrunkPosition.y, treeTrunkPosition.z, treeData.treeTrunk);
            WorldCreateHandler.Instance.manager.AddUpdateBlockForEditor(treeTrunkPosition.x - 1, treeTrunkPosition.y, treeTrunkPosition.z, treeData.treeTrunk);
            WorldCreateHandler.Instance.manager.AddUpdateBlockForEditor(treeTrunkPosition.x + 1, treeTrunkPosition.y, treeTrunkPosition.z + 1, treeData.treeTrunk);
            WorldCreateHandler.Instance.manager.AddUpdateBlockForEditor(treeTrunkPosition.x, treeTrunkPosition.y, treeTrunkPosition.z + 1, treeData.treeTrunk);
            WorldCreateHandler.Instance.manager.AddUpdateBlockForEditor(treeTrunkPosition.x, treeTrunkPosition.y, treeTrunkPosition.z - 1, treeData.treeTrunk);
        }
    }

    /// <summary>
    /// 增加世界之树
    /// </summary>
    public static bool AddTreeForWorld(uint randomData, Vector3Int startPosition, BiomeForTreeData treeData)
    {
        ////概率小于万分之一的用RandomTools
        //int seed = WorldCreateHandler.Instance.manager.GetWorldSeed();
        //RandomTools randomTools = RandomUtil.GetRandom(seed, startPosition.x, startPosition.y, startPosition.z);
        ////生成概率
        //float addRate = randomTools.NextFloat();

        //生成概率
        float addRate = WorldRandTools.GetValue(startPosition, randomData);
        if (addRate < treeData.addRate)
        {       
            //高度
            int treeHeight = WorldRandTools.Range(treeData.minHeight, treeData.maxHeight);
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
                            WorldCreateHandler.Instance.manager.AddUpdateBlock(tempTrunkPosition, treeData.treeTrunk);
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

                                    BlockDirectionEnum baseDirection =
                                        (x == -trunkRange ? BlockDirectionEnum.LeftForward
                                        : x == trunkRange ? BlockDirectionEnum.RightForward
                                        : z == -trunkRange ? BlockDirectionEnum.ForwardForward
                                        : z == trunkRange ? BlockDirectionEnum.BackForward
                                        : BlockDirectionEnum.LeftForward);

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
                                        baseDirection = BlockDirectionEnum.UpForward;
                                    }

                                    Vector3Int addPosition = new(addPositionX, addPositionY, addPositionZ);
                                    baseStartPosition += addPosition;
                                    //干
                                    WorldCreateHandler.Instance.manager.AddUpdateBlock(baseStartPosition, treeData.treeTrunk, baseDirection);
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
                                        BlockDirectionEnum branchDirection =
                                            (x == -trunkRange ? BlockDirectionEnum.LeftForward
                                            : x == trunkRange ? BlockDirectionEnum.RightForward
                                            : z == -trunkRange ? BlockDirectionEnum.ForwardForward
                                            : z == trunkRange ? BlockDirectionEnum.BackForward
                                            : BlockDirectionEnum.LeftForward);

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
                                        WorldCreateHandler.Instance.manager.AddUpdateBlock(branchStartPosition, treeData.treeTrunk, branchDirection);
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
                                                        WorldCreateHandler.Instance.manager.AddUpdateBlock(leavesPosition, treeData.treeLeaves);
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
            return true;
        }
        return false;
    }
    public static void AddTreeForWorldEditor(Vector3Int startPosition, BiomeForTreeData treeData)
    {
        //高度
        int treeHeight = WorldRandTools.Range(treeData.minHeight, treeData.maxHeight);
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
                        WorldCreateHandler.Instance.manager.AddUpdateBlockForEditor(tempTrunkPosition, treeData.treeTrunk);
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

                                BlockDirectionEnum baseDirection =
                                    (x == -trunkRange ? BlockDirectionEnum.LeftForward
                                    : x == trunkRange ? BlockDirectionEnum.RightForward
                                    : z == -trunkRange ? BlockDirectionEnum.ForwardForward
                                    : z == trunkRange ? BlockDirectionEnum.BackForward
                                    : BlockDirectionEnum.LeftForward);

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
                                    baseDirection = BlockDirectionEnum.UpForward;
                                }

                                Vector3Int addPosition = new(addPositionX, addPositionY, addPositionZ);
                                baseStartPosition += addPosition;
                                //干
                                WorldCreateHandler.Instance.manager.AddUpdateBlockForEditor(baseStartPosition, treeData.treeTrunk, baseDirection);
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
                                    BlockDirectionEnum branchDirection =
                                        (x == -trunkRange ? BlockDirectionEnum.LeftForward
                                        : x == trunkRange ? BlockDirectionEnum.RightForward
                                        : z == -trunkRange ? BlockDirectionEnum.ForwardForward
                                        : z == trunkRange ? BlockDirectionEnum.BackForward
                                        : BlockDirectionEnum.LeftForward);

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
                                    WorldCreateHandler.Instance.manager.AddUpdateBlockForEditor(branchStartPosition, treeData.treeTrunk, branchDirection);
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
                                                    WorldCreateHandler.Instance.manager.AddUpdateBlockForEditor(leavesPosition, treeData.treeLeaves);
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
                    WorldCreateHandler.Instance.manager.AddUpdateBlock(treeTrunkPosition, treeData.treeTrunk);
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
                            WorldCreateHandler.Instance.manager.AddUpdateBlock(treeTrunkPosition.x + x, treeTrunkPosition.y, treeTrunkPosition.z + z, treeData.treeLeaves);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 增加倾斜的树（例如椰子树）
    /// </summary>
    public static void AddTreeForOblique(uint randomData, Vector3Int startPosition, BiomeForTreeData treeData)
    {
        //生成概率
        float addRate = WorldRandTools.GetValue(startPosition, randomData);

        if (addRate < treeData.addRate)
        {
            GetRandomBlockDirection(out BlockDirectionEnum randomBlockDirection, out Vector3Int randomAddPosition);
            //高度
            int treeHeight = WorldRandTools.Range(treeData.minHeight, treeData.maxHeight);
            //树偏移点（每次一半偏移）
            int treeHalfHeight = Mathf.FloorToInt(treeHeight / 2f);
            //偏移量
            int offsetIndex = 0;
            //最大范围
            int range = treeData.leavesRange;
            int leavesHeightOffset = 0;
            Vector3Int startLeavesPos = Vector3Int.zero;
            for (int i = 0; i < treeHeight + 1; i++)
            {
                if (i >= treeHalfHeight)
                {
                    treeHalfHeight += Mathf.FloorToInt((treeHeight - treeHalfHeight) / 2f);
                    offsetIndex++;
                }
                Vector3Int treeTrunkPosition = new
                    (startPosition.x + offsetIndex * randomAddPosition.x, startPosition.y + (i + 1), startPosition.z + offsetIndex * randomAddPosition.z);

                if (i >= treeHeight - 1)
                {
                    startLeavesPos = startLeavesPos.AddY(1);
                    if (i >= treeHeight)
                    {
                        //叶子在最顶层递减
                        range--;
                    }

                    //生成叶子
                    for (int x = -range; x <= range; x++)
                    {
                        for (int z = -range; z <= range; z++)
                        {
                            //有几率不生成
                            //int randomLeaves = WorldRandTools.Range(0, 5);
                            //if (randomLeaves < 1)
                            //    continue;
                            WorldCreateHandler.Instance.manager.AddUpdateBlock(startLeavesPos.x + x, startLeavesPos.y, startLeavesPos.z + z, treeData.treeLeaves);
                        }
                    }
                    leavesHeightOffset++;
                }
                else
                {
                    startLeavesPos = treeTrunkPosition;
                }

                //生成树干
                if (i < treeHeight)
                {
                    WorldCreateHandler.Instance.manager.AddUpdateBlock(treeTrunkPosition, treeData.treeTrunk);
                }
            }
        }
    }
    public static void AddTreeForObliqueEditor(Vector3Int startPosition, BiomeForTreeData treeData)
    {
        GetRandomBlockDirection(out BlockDirectionEnum randomBlockDirection, out Vector3Int randomAddPosition);
        //高度
        int treeHeight = WorldRandTools.Range(treeData.minHeight, treeData.maxHeight);
        //树偏移点（每次一半偏移）
        int treeHalfHeight = Mathf.FloorToInt(treeHeight / 2f);
        //偏移量
        int offsetIndex = 0;
        //最大范围
        int range = treeData.leavesRange;
        int leavesHeightOffset = 0;
        Vector3Int startLeavesPos = Vector3Int.zero;
        for (int i = 0; i < treeHeight + 1; i++)
        {
            if (i >= treeHalfHeight)
            {
                treeHalfHeight += Mathf.FloorToInt((treeHeight - treeHalfHeight) / 2f);
                offsetIndex++;
            }
            Vector3Int treeTrunkPosition = new
                (startPosition.x + offsetIndex * randomAddPosition.x, startPosition.y + (i + 1), startPosition.z + offsetIndex * randomAddPosition.z);

            if (i >= treeHeight - 1)
            {
                startLeavesPos = startLeavesPos.AddY(1);
                if (i >= treeHeight)
                {
                    //叶子在最顶层递减
                    range--;
                }

                //生成叶子
                for (int x = -range; x <= range; x++)
                {
                    for (int z = -range; z <= range; z++)
                    {
                        //有几率不生成
                        //int randomLeaves = WorldRandTools.Range(0, 5);
                        //if (randomLeaves < 1)
                        //    continue;
                        WorldCreateHandler.Instance.manager.AddUpdateBlockForEditor(startLeavesPos.x + x, startLeavesPos.y, startLeavesPos.z + z, treeData.treeLeaves);
                    }
                }
                leavesHeightOffset++;
            }
            else
            {
                startLeavesPos = treeTrunkPosition;
            }

            //生成树干
            if (i < treeHeight)
            {
                WorldCreateHandler.Instance.manager.AddUpdateBlockForEditor(treeTrunkPosition, treeData.treeTrunk);
            }
        }
    }
    /// <summary>
    /// 增加柳树
    /// </summary>
    public static bool AddTreeForSalix(uint randomData, Vector3Int startPosition, BiomeForTreeData treeData)
    {
        //生成概率
        float addRate = WorldRandTools.GetValue(startPosition, randomData);

        if (addRate < treeData.addRate)
        {
            //高度
            int treeHeight = WorldRandTools.Range(treeData.minHeight, treeData.maxHeight);
            for (int i = treeHeight - 3; i < treeHeight + 2; i++)
            {
                //生成树叶
                Vector3Int treeTrunkPosition = startPosition + Vector3Int.up * (i + 1);
                int range = treeData.leavesRange;
                if (i == treeHeight + 1)
                {
                    range -= 1;
                }
                else if (i == treeHeight - 3)
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

                        //生成柳条
                        if (i == treeHeight - 2)
                        {
                            int randomCreate = WorldRandTools.Range(0, 6);
                            if (randomCreate == 0)
                            {
                                int wickerLenth = WorldRandTools.Range(treeData.minLeavesHeight, treeData.maxLeavesHeight);
                                for (int w = 0; w < wickerLenth; w++)
                                {
                                    WorldCreateHandler.Instance.manager.AddUpdateBlock(treeTrunkPosition.x + x, treeTrunkPosition.y - w, treeTrunkPosition.z + z, BlockTypeEnum.Wicker);
                                }
                                continue;
                            }
                        }
                        WorldCreateHandler.Instance.manager.AddUpdateBlock(treeTrunkPosition.x + x, treeTrunkPosition.y, treeTrunkPosition.z + z, treeData.treeLeaves);
                    }
                }
            }


            //生成树干
            for (int i = 0; i < treeHeight; i++)
            {
                Vector3Int treeTrunkPosition = startPosition + Vector3Int.up * (i + 1);

                //枝干
                int randomCreate = WorldRandTools.Range(0, 4);
                if (randomCreate == 1)
                {
                    int createDirection = WorldRandTools.Range(0, 4);
                    int createDirectionOffset = WorldRandTools.Range(0, 2);
                    switch (createDirection)
                    {
                        case 0:
                            WorldCreateHandler.Instance.manager.AddUpdateBlock(treeTrunkPosition.x - 1, treeTrunkPosition.y, treeTrunkPosition.z + createDirectionOffset, treeData.treeTrunk, BlockDirectionEnum.LeftForward);
                            break;
                        case 1:
                            WorldCreateHandler.Instance.manager.AddUpdateBlock(treeTrunkPosition.x + 2, treeTrunkPosition.y, treeTrunkPosition.z + createDirectionOffset, treeData.treeTrunk, BlockDirectionEnum.RightForward);
                            break;
                        case 2:
                            WorldCreateHandler.Instance.manager.AddUpdateBlock(treeTrunkPosition.x + createDirectionOffset, treeTrunkPosition.y, treeTrunkPosition.z - 1, treeData.treeTrunk, BlockDirectionEnum.ForwardForward);
                            break;
                        case 3:
                            WorldCreateHandler.Instance.manager.AddUpdateBlock(treeTrunkPosition.x + createDirectionOffset, treeTrunkPosition.y, treeTrunkPosition.z + 2, treeData.treeTrunk, BlockDirectionEnum.BackForward);
                            break;
                    }
                }



                WorldCreateHandler.Instance.manager.AddUpdateBlock(treeTrunkPosition.x, treeTrunkPosition.y, treeTrunkPosition.z, treeData.treeTrunk);
                WorldCreateHandler.Instance.manager.AddUpdateBlock(treeTrunkPosition.x + 1, treeTrunkPosition.y, treeTrunkPosition.z, treeData.treeTrunk);
                WorldCreateHandler.Instance.manager.AddUpdateBlock(treeTrunkPosition.x, treeTrunkPosition.y, treeTrunkPosition.z + 1, treeData.treeTrunk);
                WorldCreateHandler.Instance.manager.AddUpdateBlock(treeTrunkPosition.x + 1, treeTrunkPosition.y, treeTrunkPosition.z + 1, treeData.treeTrunk);
            }

            return true;
        }
        return false;
    }

    /// <summary>
    /// 获取随机四个方向
    /// </summary>
    /// <param name="randomBlockDirection"></param>
    /// <param name="randomAddPosition"></param>
    protected static void GetRandomBlockDirection(out BlockDirectionEnum randomBlockDirection, out Vector3Int randomAddPosition)
    {
        int randomAdd = WorldRandTools.Range(1, 5);
        switch (randomAdd)
        {
            case 1:
                randomAddPosition = new Vector3Int(-1, 0, 0);
                randomBlockDirection = BlockDirectionEnum.LeftForward;
                break;
            case 2:
                randomAddPosition = new Vector3Int(1, 0, 0);
                randomBlockDirection = BlockDirectionEnum.RightForward;
                break;
            case 3:
                randomAddPosition = new Vector3Int(0, 0, -1);
                randomBlockDirection = BlockDirectionEnum.ForwardForward;
                break;
            case 4:
                randomAddPosition = new Vector3Int(0, 0, 1);
                randomBlockDirection = BlockDirectionEnum.BackForward;
                break;
            default:
                randomAddPosition = Vector3Int.zero;
                randomBlockDirection = BlockDirectionEnum.UpForward;
                break;
        }
    }
}