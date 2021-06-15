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

    public override BlockTypeEnum GetBlockType(BiomeInfoBean biomeInfo, int genHeight, Vector3Int localPos, Vector3Int wPos)
    {
        base.GetBlockType(biomeInfo, genHeight, localPos, wPos);
        if (wPos.y == genHeight)
        {
            AddWeed(wPos);
            AddFlower(wPos);
            AddTree(wPos);
            // 地表，使用草
            return BlockTypeEnum.Grass_Wild;
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
    protected void AddFlower(Vector3Int wPos)
    {
        BiomeForFlowerData flowersData = new BiomeForFlowerData
        {
            addRate = 0.02f,
            listFlowerType = new List<BlockTypeEnum> { BlockTypeEnum.FlowerSun, BlockTypeEnum.FlowerRose, BlockTypeEnum.FlowerChrysanthemum }
        };
        BiomeCreateTool.AddFlower(101,wPos, flowersData);
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
        BiomeCreateTool.AddTree(wPos, treeData);
    }

    protected void AddWeed(Vector3Int wPos)
    {
        BiomeForPlantData weedData = new BiomeForPlantData
        {
            addRate = 0.3f,
            listPlantType = new List<BlockTypeEnum>{ BlockTypeEnum.Weed_Long, BlockTypeEnum.Weed_Normal, BlockTypeEnum.Weed_Short }
        };
        BiomeCreateTool.AddPlant(wPos, weedData);
    }



}