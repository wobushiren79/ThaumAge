using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class BiomeCreateTool
{




    public struct BiomeForWaterPoolData
    {
        public float addRate;
        public int minDepth;
        public int maxDepth;
        public int minSize;
        public int maxSize;
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
                        WorldCreateHandler.Instance.manager.AddUpdateBlock(currentPosition.x, currentPosition.y, currentPosition.z, BlockTypeEnum.None);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 增加矿石
    /// </summary>
    /// <param name="randomData"></param>
    /// <param name="addRate"></param>
    /// <param name="blockType"></param>
    /// <param name="startPosition"></param>
    public static void AddOre(uint randomData, float addRate, Vector3Int startPosition)
    {
        //生成概率
        float addRateRandom = WorldRandTools.GetValue(startPosition, randomData);

        if (addRateRandom < addRate)
        {
            //高度
            int range = 2;
            //不能直接用 startPosition 因为addRateRandom 的概率已经决定了他的值
            int randomOre = WorldRandTools.Range(0, arrayBlockOre.Length, new Vector3(startPosition.x, 0, 0));
            for (int x = -range; x < range; x++)
            {
                for (int y = -range; y < range; y++)
                {
                    for (int z = -range; z < range; z++)
                    {
                        Vector3Int blockPosition = new Vector3Int(startPosition.x + x, startPosition.y + y, startPosition.z + z);
                        float disTemp = Vector3Int.Distance(blockPosition, startPosition);
                        if (blockPosition.y <= 3 || disTemp >= range - 0.5f)
                        {
                            continue;
                        }
                        WorldCreateHandler.Instance.manager.AddUpdateBlock(blockPosition, arrayBlockOre[randomOre]);
                    }
                }
            }
        }
    }

    public static BlockTypeEnum[] arrayBlockOre = new BlockTypeEnum[]
    {   
        //煤矿
        BlockTypeEnum.OreCoal , BlockTypeEnum.OreCoal , BlockTypeEnum.OreCoal,
        BlockTypeEnum.OreCopper,//铜矿
        BlockTypeEnum.OreIron,//铁矿
        BlockTypeEnum.OreSilver,//银矿
        BlockTypeEnum.OreGold,//金矿
        BlockTypeEnum.OreTin,//锡矿
        BlockTypeEnum. OreAluminum,//铝矿
    };

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

                float createRate = 1;
                if (buildingData.randomRate < 1)
                {
                     createRate = WorldRandTools.GetValue(targetPosition);
                }

                if (buildingData.randomRate == 0 || createRate <= buildingData.randomRate)
                {
                    VectorUtil.GetRotatedPosition(startPosition, targetPosition, new Vector3(0, randomAngle, 0));
                    WorldCreateHandler.Instance.manager.AddUpdateBlock(targetPosition, buildingData.blockId, (BlockDirectionEnum)buildingData.direction);
                }
            }
            return true;
        }
        return false;
    }

}