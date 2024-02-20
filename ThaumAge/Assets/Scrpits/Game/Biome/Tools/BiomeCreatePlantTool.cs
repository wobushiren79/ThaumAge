using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BiomeCreatePlantTool
{
    public struct BiomeForPlantData
    {
        public float addRate;
        public float flowerRange;
        public int minSize;
        public int maxSize;
        public List<BlockTypeEnum> listPlantType;
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
        if (addRate < plantData.addRate)
        {
            int weedTypeNumber = WorldRandTools.Range(0, plantData.listPlantType.Count);
            WorldCreateHandler.Instance.manager.AddUpdateBlock(startPosition.x, startPosition.y + 1, startPosition.z, plantData.listPlantType[weedTypeNumber]);
        }
    }

    /// <summary>
    /// 增加长植物
    /// </summary>
    public static void AddLongPlant(int blockId, Vector3Int startPosition, int minHeight = 2, int maxHeight = 12)
    {
        //高度
        int plantHeight = WorldRandTools.Range(minHeight, maxHeight, startPosition);
        for (int i = 1; i < plantHeight; i++)
        {
            WorldCreateHandler.Instance.manager.AddUpdateBlock(startPosition.x, startPosition.y + i, startPosition.z, blockId);
        }
    }

    /// <summary>
    /// 增加鲜花
    /// </summary>
    /// <param name="randomData"></param>
    /// <param name="startPosition"></param>
    /// <param name="flowerData"></param>
    public static void AddFlower(uint randomData, Vector3Int startPosition, BiomeForPlantData flowerData)
    {
        float addRate = WorldRandTools.GetValue(startPosition, randomData);
        int flowerTypeNumber = WorldRandTools.Range(0, flowerData.listPlantType.Count);
        if (addRate < flowerData.addRate)
        {
            WorldCreateHandler.Instance.manager.AddUpdateBlock(startPosition.x, startPosition.y + 1, startPosition.z, flowerData.listPlantType[flowerTypeNumber]);
        }
    }



    /// <summary>
    /// 生成枯木
    /// </summary>
    /// <param name="randomData"></param>
    /// <param name="startPosition"></param>
    public static void AddDeadwood(Vector3Int startPosition)
    {
        //高度
        int treeHeight = WorldRandTools.Range(1, 3, startPosition);
        Vector3Int treeDataPosition = startPosition;
        for (int i = 1; i < treeHeight; i++)
        {
            WorldCreateHandler.Instance.manager.AddUpdateBlock(treeDataPosition.x, treeDataPosition.y + i, treeDataPosition.z, BlockTypeEnum.WoodDead, BlockDirectionEnum.UpForward);
        }
    }
}