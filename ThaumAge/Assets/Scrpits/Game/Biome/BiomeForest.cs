using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BiomeForest : Biome
{
    //森林
    public BiomeForest() : base(BiomeTypeEnum.Forest)
    {

    }

    public override BlockTypeEnum GetBlockType(int genHeight, Vector3Int localPos, Vector3Int wPos)
    {
        if (wPos.y == genHeight)
        {
            AddWeed(wPos);
            AddFlower(wPos);
            AddTree(wPos);
            AddBigTree(wPos);
            // 地表，使用草
            return BlockTypeEnum.Grass;
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
    protected void AddBigTree(Vector3Int wPos)
    {
        TreeData treeData = new TreeData
        {
            addRateMin = 1,
            addRateMax = 100,
            minHeight = 10,
            maxHeight = 15,
            treeTrunk = BlockTypeEnum.Oak,
            treeLeaves = BlockTypeEnum.LeavesOak,
            trunkRange = 2,
            leavesRange = 2,
        };
        AddBigTree(wPos, treeData);
    }

    protected void AddTree(Vector3Int wPos)
    {
        TreeData treeData = new TreeData
        {
            addRateMin = 1,
            addRateMax = 100,
            minHeight = 3,
            maxHeight = 7,
            treeTrunk = BlockTypeEnum.Oak,
            treeLeaves = BlockTypeEnum.LeavesOak,
            leavesRange = 2,
        };
        AddTree(wPos, treeData);
    }

    protected void AddWeed(Vector3Int wPos)
    {
        WeedData weedData = new WeedData
        {
            addRateMin = 1,
            addRateMax = 10,
            listWeedType = new List<BlockTypeEnum> { BlockTypeEnum.Weed_Long, BlockTypeEnum.Weed_Normal, BlockTypeEnum.Weed_Short }
        };
        AddWeed(wPos, weedData);
    }

    protected void AddFlower(Vector3Int wPos)
    {
        FlowerData flowersData = new FlowerData
        {
            addRateMin = 1,
            addRateMax = 100,
            listFlowerType = new List<BlockTypeEnum> { BlockTypeEnum.Sunflower, BlockTypeEnum.Rose, BlockTypeEnum.Chrysanthemum }
        };
        AddFlower(wPos, flowersData);
    }

}