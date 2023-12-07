using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static BiomeCreatePlantTool;
using static BiomeCreateTool;

public class BiomeOcean : Biome
{
    //海洋
    public BiomeOcean() : base(BiomeTypeEnum.Ocean)
    {

    }

    public BlockTypeEnum GetBlockForMaxHeightDown(Chunk chunk, Vector3Int localPos)
    {
        return BlockTypeEnum.None;
        //if (localPos.y == terrainData.maxHeight)
        //{
        //    Vector3Int wPos = localPos + chunk.chunkData.positionForWorld;
        //    int waterHight = biomeInfo.water_height;
        //    if (localPos.y >= waterHight - 4 && localPos.y <= waterHight + 4)
        //    {
        //        if (localPos.y >= waterHight)
        //        {
        //            AddTree(wPos);
        //        }
        //        return BlockTypeEnum.Sand;
        //    }
        //    else if (localPos.y > waterHight)
        //    {
        //        AddWeed(wPos);
        //        return BlockTypeEnum.Grass;
        //    }
        //    else if (localPos.y < waterHight - 8)
        //    {
        //        AddWaterPlant(wPos);
        //    }
        //    return BlockTypeEnum.Dirt;
        //}
        //if (localPos.y < terrainData.maxHeight && localPos.y > terrainData.maxHeight - 5)
        //{
        //    //中使用泥土
        //    return BlockTypeEnum.Dirt;
        //}
        //else if (localPos.y == 0)
        //{
        //    //基础
        //    return BlockTypeEnum.Foundation;
        //}
        //else
        //{
        //    //其他石头
        //    return BlockTypeEnum.Stone;
        //}
    }

    /// <summary>
    /// 增加水里植物
    /// </summary>
    public void AddWaterPlant(Vector3Int wPos)
    {
        //增加珊瑚
        BiomeForPlantData coralData = new BiomeForPlantData
        {
            addRate = 0.03f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.CoralRed, BlockTypeEnum.CoralBlue, BlockTypeEnum.CoralYellow }
        };
        BiomeCreatePlantTool.AddFlower(101, wPos, coralData);
        //增加水草

        BiomeForPlantData seaweedData = new BiomeForPlantData
        {
            addRate = 0.03f,
            minSize = 3,
            maxSize = 5,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.Seaweed }
        };
        BiomeCreatePlantTool.AddLongPlant(102, wPos, seaweedData);
    }

    /// <summary>
    /// 增加杂草
    /// </summary>
    /// <param name="wPos"></param>
    protected void AddWeed(Vector3Int wPos)
    {
        BiomeForPlantData weedData = new BiomeForPlantData
        {
            addRate = 0.1f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.WeedGrassLong, BlockTypeEnum.WeedGrassNormal, BlockTypeEnum.WeedGrassShort, BlockTypeEnum.WeedGrassStart }
        };
        BiomeCreatePlantTool.AddPlant(222, wPos, weedData);
    }

    /// <summary>
    /// 增加树
    /// </summary>
    /// <param name="wPos"></param>
    protected void AddTree(Vector3Int wPos)
    {
        BiomeCreateTreeTool.BiomeForTreeData treeData = new BiomeCreateTreeTool.BiomeForTreeData
        {
            addRate = 0.001f,
            minHeight = 5,
            maxHeight = 8,
            treeTrunk = BlockTypeEnum.TreePalm,
            treeLeaves = BlockTypeEnum.LeavesPalm,
            leavesRange = 2,
        };
        BiomeCreateTreeTool.AddTreeForOblique(333, wPos, treeData);
    }
}