using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static BiomeCreatePlantTool;
using static BiomeCreateTool;
using static BiomeCreateTreeTool;

public class BiomeTest : Biome
{
    //测试
    public BiomeTest() : base(BiomeTypeEnum.Test)
    {

    }

    public override BlockTypeEnum GetBlockForMaxHeightDown(Chunk chunk, Vector3Int localPos)
    {
        return BlockTypeEnum.None;
        //if (localPos.y == terrainData.maxHeight)
        //{
        //    //AddWeed(wPos);
        //    //AddBigTree(wPos);
        //    //AddWorldTree(wPos);
        //    //AddMushroomTree(wPos);
        //    //AddStoneMoss(wPos);
        //    //AddFlower(wPos);
        //    //AddDeadwood(wPos);
        //    //草
        //    return BlockTypeEnum.Grass;
        //}
        //else if (localPos.y == 0)
        //{
        //    //基础
        //    return BlockTypeEnum.Foundation;
        //}
        //else if (localPos.y < terrainData.maxHeight && localPos.y >= terrainData.maxHeight - 5)
        //{
        //    //其他土
        //    return BlockTypeEnum.Dirt;
        //}
        //else
        //{
        //    //BiomeCreateTool.AddOre(900, 0.00001f, wPos);
        //    //其他土
        //    return BlockTypeEnum.Stone;
        //}
    }


    protected void AddMushroomTree(Vector3Int wPos)
    {
        Vector3Int startPosition = wPos + Vector3Int.up;
        BiomeCreateTool.AddBuilding(0.0001f, 101, startPosition, BuildingTypeEnum.MushrooBig);
        BiomeCreateTool.AddBuilding(0.0001f, 201, startPosition, BuildingTypeEnum.Mushroom);
        BiomeCreateTool.AddBuilding(0.0001f, 301, startPosition, BuildingTypeEnum.MushrooSmall);
    }

    protected void AddTree(Vector3Int wPos)
    {
        BiomeForTreeData treeData = new BiomeForTreeData
        {
            addRate = 0.01f,
            minHeight = 3,
            maxHeight = 6,
            treeTrunk = BlockTypeEnum.TreeOak,
            treeLeaves = BlockTypeEnum.LeavesOak,
            leavesRange = 2,
        };
        BiomeCreateTreeTool.AddTree(401, wPos, treeData);
    }

    protected void AddBigTree(Vector3Int wPos)
    {
        BiomeForTreeData treeData = new BiomeForTreeData
        {
            addRate = 0.005f,
            minHeight = 6,
            maxHeight = 10,
            treeTrunk = BlockTypeEnum.TreeSilver,
            treeLeaves = BlockTypeEnum.LeavesSilver,
            leavesRange = 4,
        };
        BiomeCreateTreeTool.AddTreeForBig(501, wPos, treeData);
    }

    protected void AddWorldTree(Vector3Int wPos)
    {
        BiomeForTreeData treeData = new BiomeForTreeData
        {
            addRate = 0.0001f,
            minHeight = 30,
            maxHeight = 50,
            treeTrunk = BlockTypeEnum.TreeWorld,
            treeLeaves = BlockTypeEnum.LeavesWorld,
            leavesRange = 4,
            trunkRange = 3,
        };
        BiomeCreateTreeTool.AddTreeForWorld(502,wPos, treeData);
    }

    protected void AddWeed(Vector3Int wPos)
    {
        BiomeForPlantData weedData = new BiomeForPlantData
        {
            addRate = 0.3f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.WeedGrassLong, BlockTypeEnum.WeedGrassNormal, BlockTypeEnum.WeedGrassShort, BlockTypeEnum.WeedGrassStart }
        };
        BiomeCreatePlantTool.AddPlant(601, wPos, weedData);
    }

    public void AddFlower(Vector3Int wPos)
    {
        BiomeForPlantData flowersData = new BiomeForPlantData
        {
            addRate = 0.005f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.MushroomLuminous }
        };
        BiomeCreatePlantTool.AddFlower(701, wPos, flowersData);
    }

    protected void AddStoneMoss(Vector3Int wPos)
    {
        Vector3Int startPosition = wPos + Vector3Int.up;
        BiomeCreateTool.AddBuilding(0.005f, 801, startPosition, BuildingTypeEnum.StoneMoss);
    }

    /// <summary>
    /// 增加枯木
    /// </summary>
    /// <param name="startPosition"></param>
    public void AddDeadwood(Vector3Int startPosition)
    {
        BiomeCreatePlantTool.AddDeadwood(101, 0.001f, startPosition);
    }
}