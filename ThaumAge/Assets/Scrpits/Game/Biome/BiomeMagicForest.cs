﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using static BiomeCreateTool;

public class BiomeMagicForest : Biome
{

    //魔法深林
    public BiomeMagicForest() : base(BiomeTypeEnum.MagicForest)
    {
    }

    public override BlockTypeEnum GetBlockType(Chunk chunk, BiomeInfoBean biomeInfo, int genHeight, Vector3Int localPos, Vector3Int wPos)
    {
        base.GetBlockType(chunk, biomeInfo, genHeight, localPos, wPos);
        if (wPos.y == genHeight)
        {
            AddWeed(wPos);
            AddBigTree(wPos);
            AddWorldTree(wPos);
            AddMushroomTree(wPos);
            AddStoneMoss(wPos);
            AddFlower(wPos);
            // 地表，使用草
            return BlockTypeEnum.Grass_Magic;
        }
        if (wPos.y < genHeight && wPos.y > genHeight - 10)
        {
            //中使用泥土
            return BlockTypeEnum.Dirt;
        }
        else if (wPos.y == 0)
        {
            //基础
            return BlockTypeEnum.Foundation;
        }
        else
        {
            //其他石头
            return BlockTypeEnum.Stone;
        }
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
        BiomeCreateTool.AddTree(401, wPos, treeData);
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
        BiomeCreateTool.AddTreeForBig(501, wPos, treeData);
    }

    protected void AddWorldTree(Vector3Int wPos)
    {
        BiomeForTreeData treeData = new BiomeForTreeData
        {
            addRate = 0.00005f,
            minHeight = 30,
            maxHeight = 50,
            treeTrunk = BlockTypeEnum.TreeWorld,
            treeLeaves = BlockTypeEnum.LeavesWorld,
            leavesRange = 4,
            trunkRange = 3,
        };
        BiomeCreateTool.AddTreeForWorld(wPos, treeData);
    }

    protected void AddWeed(Vector3Int wPos)
    {
        BiomeForPlantData weedData = new BiomeForPlantData
        {
            addRate = 0.3f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.Weed_Long, BlockTypeEnum.Weed_Normal, BlockTypeEnum.Weed_Short }
        };
        BiomeCreateTool.AddPlant(601,wPos, weedData);
    }

    public void AddFlower(Vector3Int wPos)
    {
        BiomeForFlowerData flowersData = new BiomeForFlowerData
        {
            addRate = 0.005f,
            listFlowerType = new List<BlockTypeEnum> { BlockTypeEnum.MushroomLuminous }
        };
        BiomeCreateTool.AddFlower(701, wPos, flowersData);
    }

    protected void AddStoneMoss(Vector3Int wPos)
    {
        Vector3Int startPosition = wPos + Vector3Int.up;
        BiomeCreateTool.AddBuilding(0.005f, 801, startPosition, BuildingTypeEnum.StoneMoss);
    }
}