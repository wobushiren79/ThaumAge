using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BiomeForest : Biome
{
    //森林
    public BiomeForest() : base(BiomeTypeEnum.Forest)
    {

    }

    public BlockTypeEnum GetBlockForMaxHeightDown(Chunk chunk, Vector3Int localPos)
    {
        return BlockTypeEnum.None;
        //if (localPos.y == terrainData.maxHeight)
        //{
        //    Vector3Int wPos = localPos + chunk.chunkData.positionForWorld;
        //    int waterHeight = biomeInfo.GetWaterPlaneHeight();
        //    if (localPos.y == waterHeight)
        //    {
        //        AddStoneMoss(wPos);
        //        AddTreeForFallDown(wPos);
        //        return BlockTypeEnum.Sand;
        //    }
        //    else if (localPos.y < waterHeight)
        //    {
        //        return BlockTypeEnum.Dirt;
        //    }
        //    AddTreeForFallDown(wPos);
        //    AddWeed(wPos);
        //    AddFlowerAndDeadWood(wPos);
        //    AddTree(wPos);
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

    public void InitBiomeBlockForChunk(Chunk chunk)
    {
        //base.InitBiomeBlockForChunk(chunk, biomeMapData);
        ////获取地形数据
        //ChunkTerrainData startTerrainData = GetTerrainData(chunk, biomeMapData, 0, 0);

        //int waterHeight = biomeInfo.GetWaterPlaneHeight();
        //Vector3Int flowerPosition = new Vector3Int(chunk.chunkData.positionForWorld.x, startTerrainData.maxHeight, chunk.chunkData.positionForWorld.z);
        //if (startTerrainData.maxHeight == waterHeight)
        //{

        //}
        //else if (startTerrainData.maxHeight < waterHeight)
        //{
        //    AddFlowerWater(flowerPosition);
        //}
        //else
        //{
        //    AddFlowerWood(flowerPosition);
        //}
    }

    /// <summary>
    /// 增加元素花
    /// </summary>
    /// <param name="wPos"></param>
    protected void AddFlowerWood(Vector3Int wPos)
    {
        //增加花
        BiomeCreatePlantTool.BiomeForPlantData flowersData = new BiomeCreatePlantTool.BiomeForPlantData
        {
            addRate = 0.1f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.FlowerWood }
        };
        BiomeCreatePlantTool.AddFlower(110, wPos, flowersData);
    }
    protected void AddFlowerWater(Vector3Int wPos)
    {
        //增加花
        BiomeCreatePlantTool.BiomeForPlantData flowersData = new BiomeCreatePlantTool.BiomeForPlantData
        {
            addRate = 0.1f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.FlowerWater }
        };
        BiomeCreatePlantTool.AddFlower(111, wPos, flowersData);
    }

    /// <summary>
    /// 添加花花
    /// </summary>
    /// <param name="wPos"></param>
    protected void AddFlowerAndDeadWood(Vector3Int wPos)
    {
        //增加花
        BiomeCreatePlantTool.BiomeForPlantData flowersData = new BiomeCreatePlantTool.BiomeForPlantData
        {
            addRate = 0.03f,
            listPlantType = new List<BlockTypeEnum> 
            { 
                BlockTypeEnum.FlowerSun, BlockTypeEnum.FlowerRose, BlockTypeEnum.FlowerChrysanthemum,
                BlockTypeEnum.MushroomWhite1,BlockTypeEnum.MushroomWhite2,BlockTypeEnum.MushroomWhite3,BlockTypeEnum.MushroomRed,
                BlockTypeEnum.BerryBushRed,BlockTypeEnum.BerryBushBlue
            }
        };
        BiomeCreatePlantTool.AddFlower(101, wPos, flowersData);
        //增加枯木
        BiomeCreatePlantTool.AddDeadwood(102, 0.001f, wPos);
    }

    /// <summary>
    /// 增加树
    /// </summary>
    /// <param name="wPos"></param>
    protected void AddTree(Vector3Int wPos)
    {
        BiomeCreateTreeTool.BiomeForTreeData treeData = new BiomeCreateTreeTool.BiomeForTreeData
        {
            addRate = 0.025f,
            minHeight = 5,
            maxHeight = 8,
            treeTrunk = BlockTypeEnum.TreeOak,
            treeLeaves = BlockTypeEnum.LeavesOak,
            leavesRange = 2,
        };
        BiomeCreateTreeTool.AddTree(201, wPos, treeData);
    }

    /// <summary>
    /// 增加倒下的树
    /// </summary>
    /// <param name="wPos"></param>
    protected void AddTreeForFallDown(Vector3Int wPos)
    {
        BiomeCreateTreeTool.BiomeForTreeData treeData = new BiomeCreateTreeTool.BiomeForTreeData
        {
            addRate = 0.005f,
            minHeight = 1,
            maxHeight = 3,
            treeTrunk = BlockTypeEnum.TreeOak,
            treeLeaves = BlockTypeEnum.LeavesOak,
        };
        BiomeCreateTreeTool.AddTreeForFallDown(211, wPos + new Vector3Int(0, 1, 0), treeData);
    }

    /// <summary>
    /// 添加杂草
    /// </summary>
    /// <param name="wPos"></param>
    protected void AddWeed(Vector3Int wPos)
    {
        BiomeCreatePlantTool.BiomeForPlantData weedData = new BiomeCreatePlantTool.BiomeForPlantData
        {
            addRate = 0.5f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.WeedGrassLong, BlockTypeEnum.WeedGrassNormal, BlockTypeEnum.WeedGrassShort, BlockTypeEnum.WeedGrassStart }
        };
        BiomeCreatePlantTool.AddPlant(333, wPos, weedData);
    }

    /// <summary>
    /// 增加苔石
    /// </summary>
    /// <param name="wPos"></param>
    protected void AddStoneMoss(Vector3Int wPos)
    {
        Vector3Int startPosition = wPos + Vector3Int.up;
        BiomeCreateTool.AddBuilding(0.01f, 801, startPosition, BuildingTypeEnum.StoneMoss);
    }
}