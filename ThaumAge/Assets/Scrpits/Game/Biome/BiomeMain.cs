using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static BiomeCreatePlantTool;
using static BiomeCreateTool;
using static BiomeCreateTreeTool;

public class BiomeMain : Biome
{

    public BiomeMain() : base(BiomeTypeEnum.Main)
    {

    }


    public override BlockTypeEnum GetBlockForMaxHeightDown(Chunk chunk, Vector3Int localPos, ChunkTerrainData terrainData)
    {
        if (localPos.y == terrainData.maxHeight)
        {
            Vector3Int wPos = localPos + chunk.chunkData.positionForWorld;
            if (wPos.x <= 3 && wPos.x >= -3 && wPos.z <= 4 && wPos.z >= -10)
            {

            }
            else
            {
                AddWeed(wPos);
                AddFlower(wPos);
                AddTree(wPos);
            }

            // 地表，使用草
            return BlockTypeEnum.Grass;
        }
        else
        {
            //其他石头
            return BlockTypeEnum.Stone;
        }
    }

    protected void AddWeed(Vector3Int wPos)
    {
        BiomeForPlantData weedData = new BiomeForPlantData
        {
            addRate = 0.1f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.WeedGrassLong, BlockTypeEnum.WeedGrassNormal, BlockTypeEnum.WeedGrassShort }
        };
        BiomeCreatePlantTool.AddPlant(333, wPos, weedData);
    }

    protected void AddFlower(Vector3Int wPos)
    {
        BiomeForPlantData flowersData = new BiomeForPlantData
        {
            addRate = 0.02f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.FlowerSun, BlockTypeEnum.FlowerRose, BlockTypeEnum.FlowerChrysanthemum, BlockTypeEnum.FlowerWood }
        };
        BiomeCreatePlantTool.AddFlower(101, wPos, flowersData);
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
        BiomeCreateTreeTool.AddTree(111, wPos, treeData);
    }

}