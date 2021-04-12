using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static BiomeCreateTool;

public class BiomePrairie : Biome
{
    //草原
    public BiomePrairie() : base(BiomeTypeEnum.Prairie)
    {

    }

    public override BlockTypeEnum GetBlockType(int genHeight, Vector3Int localPos, Vector3Int wPos)
    {
        if (wPos.y == genHeight)
        {
            AddWeed(wPos);
            AddFlower(wPos);
            AddTree(wPos);
            // 地表，使用草
            return BlockTypeEnum.Grass;
        }
        if (wPos.y < genHeight && wPos.y > genHeight - 5)
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

    protected void AddTree(Vector3Int wPos)
    {
        TreeData treeData = new TreeData
        {
            addRateMin = 1,
            addRateMax = 500,
            minHeight = 4,
            maxHeight = 7,
            treeTrunk = BlockTypeEnum.Oak,
            treeLeaves = BlockTypeEnum.LeavesOak,
            leavesRange = 2,
        };
        BiomeCreateTool.AddTree(wPos, treeData);
    }

    protected void AddWeed(Vector3Int wPos)
    {
        PlantData weedData = new PlantData
        {
            addRateMin = 3,
            addRateMax = 10,
            listPlantType = new List<BlockTypeEnum>{ BlockTypeEnum.Weed_Long, BlockTypeEnum.Weed_Normal, BlockTypeEnum.Weed_Short }
        };
        BiomeCreateTool.AddPlant(wPos, weedData);
    }

    protected void AddFlower(Vector3Int wPos)
    {
        FlowerData flowersData = new FlowerData
        {
            addRateMin = 1,
            addRateMax = 50,
            listFlowerType = new List<BlockTypeEnum> { BlockTypeEnum.Sunflower, BlockTypeEnum.Rose, BlockTypeEnum.Chrysanthemum }
        };
        BiomeCreateTool.AddFlower(wPos, flowersData);
    }

}