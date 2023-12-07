using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static BiomeCreatePlantTool;
using static BiomeCreateTool;

public class BiomeMountain : Biome
{
    protected int maxHight;
    protected int lineSnow;
    protected int lineHalfSnow;
    //高山
    public BiomeMountain() : base(BiomeTypeEnum.Mountain)
    {
        //maxHight = biomeInfo.min_height + (int)biomeInfo.amplitude0 / 2;
        //lineSnow = maxHight - 30;
        //lineHalfSnow = maxHight - 24;
    }

    public BlockTypeEnum GetBlockForMaxHeightUp(Chunk chunk, Vector3Int localPos)
    {
        //if (localPos.y == terrainData.maxHeight + 1)
        //{
        //    if (localPos.y >= lineSnow && localPos.y <= lineHalfSnow)
        //    {
        //        return BlockTypeEnum.HalfStoneSnow;
        //    }
        //}
        return BlockTypeEnum.None;
    }

    public BlockTypeEnum GetBlockForMaxHeightDown(Chunk chunk, Vector3Int localPos)
    {
        return BlockTypeEnum.None;
        //if (localPos.y == terrainData.maxHeight)
        //{
        //    Vector3Int wPos = localPos + chunk.chunkData.positionForWorld;
        //    if (localPos.y > lineHalfSnow - 1)
        //    {
        //        // 地表，使用草
        //        AddWeedSnow(wPos);
        //        return BlockTypeEnum.StoneSnow;
        //    }
        //    AddWeed(wPos);
        //    AddFlower(wPos);
        //    AddTree1(wPos);
        //    if (localPos.y < lineSnow)
        //    {
        //        AddTree2(wPos);
        //    }
        //    if (localPos.y >= lineSnow && localPos.y <= lineHalfSnow)
        //    {
        //        return BlockTypeEnum.GrassSnow;
        //    }
        //    // 地表，使用草
        //    return BlockTypeEnum.Grass;
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

    protected void AddFlower(Vector3Int wPos)
    {
        BiomeForPlantData flowersData = new BiomeForPlantData
        {
            addRate = 0.02f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.FlowerMetal }
        };
        BiomeCreatePlantTool.AddFlower(101, wPos, flowersData);
    }

    protected void AddWeed(Vector3Int wPos)
    {
        BiomeForPlantData weedData = new BiomeForPlantData
        {
            addRate = 0.05f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.WeedGrassLong, BlockTypeEnum.WeedGrassNormal, BlockTypeEnum.WeedGrassShort, BlockTypeEnum.WeedGrassStart }
        };
        BiomeCreatePlantTool.AddPlant(201, wPos, weedData);
    }

    protected void AddWeedSnow(Vector3Int wPos)
    {
        BiomeForPlantData weedData = new BiomeForPlantData
        {
            addRate = 0.05f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.WeedSnowLong, BlockTypeEnum.WeedSnowNormal, BlockTypeEnum.WeedSnowShort, BlockTypeEnum.WeedSnowStart }
        };
        BiomeCreatePlantTool.AddPlant(211, wPos, weedData);
    }

    /// <summary>
    /// 增加树
    /// </summary>
    /// <param name="wPos"></param>
    protected void AddTree1(Vector3Int wPos)
    {
        BiomeCreateTreeTool.BiomeForTreeData treeData = new BiomeCreateTreeTool.BiomeForTreeData
        {
            addRate = 0.0005f,
            minHeight = 3,
            maxHeight = 6,
            treeTrunk = BlockTypeEnum.TreeCherry,
            treeLeaves = BlockTypeEnum.LeavesCherry,
            leavesRange = 2,
        };
        BiomeCreateTreeTool.AddTree(301, wPos, treeData);
    }

    /// <summary>
    /// 增加树
    /// </summary>
    /// <param name="wPos"></param>
    protected void AddTree2(Vector3Int wPos)
    {
        BiomeCreateTreeTool.BiomeForTreeData treeData = new BiomeCreateTreeTool.BiomeForTreeData
        {
            addRate = 0.002f,
            minHeight = 3,
            maxHeight = 6,
            treeTrunk = BlockTypeEnum.TreeOak,
            treeLeaves = BlockTypeEnum.LeavesOak,
            leavesRange = 2,
        };
        BiomeCreateTreeTool.AddTree(302, wPos, treeData);
    }
}