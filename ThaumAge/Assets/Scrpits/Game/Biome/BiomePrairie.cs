﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static BiomeCreatePlantTool;
using static BiomeCreateTool;
using static BiomeCreateTreeTool;

public class BiomePrairie : Biome
{
    //草原
    public BiomePrairie() : base(BiomeTypeEnum.Prairie)
    {

    }

    public override BlockTypeEnum GetBlockType(Chunk chunk, Vector3Int localPos, ChunkTerrainData terrainData)
    {
        base.GetBlockType(chunk, localPos, terrainData);
        if (localPos.y == terrainData.maxHeight)
        {
            Vector3Int wPos = localPos + chunk.chunkData.positionForWorld;
            AddWeed(wPos);
            AddFlower(wPos);
            AddTree(wPos);
            // 地表，使用草
            return BlockTypeEnum.GrassWild;
        }
        if (localPos.y < terrainData.maxHeight && localPos.y > terrainData.maxHeight - 5)
        {
            //中使用泥土
            return BlockTypeEnum.Dirt;
        }
        else if (localPos.y == 0)
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
    protected void AddFlower(Vector3Int wPos)
    {
        BiomeForPlantData flowersData = new BiomeForPlantData
        {
            addRate = 0.02f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.FlowerSun, BlockTypeEnum.FlowerRose, BlockTypeEnum.FlowerChrysanthemum,BlockTypeEnum.FlowerEarth }
        };
        BiomeCreatePlantTool.AddFlower(101, wPos, flowersData);
    }

    protected void AddTree(Vector3Int wPos)
    {
        BiomeForTreeData treeData = new BiomeForTreeData
        {
            addRate = 0.01f,
            minHeight = 4,
            maxHeight = 7,
            treeTrunk = BlockTypeEnum.TreeOak,
            treeLeaves = BlockTypeEnum.LeavesOak,
            leavesRange = 2,
        };
        BiomeCreateTreeTool.AddTree(111, wPos, treeData);
    }

    protected void AddWeed(Vector3Int wPos)
    {
        BiomeForPlantData weedData = new BiomeForPlantData
        {
            addRate = 0.3f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.WeedLong, BlockTypeEnum.WeedNormal, BlockTypeEnum.WeedShort }
        };
        BiomeCreatePlantTool.AddPlant(222,wPos, weedData);
    }



}