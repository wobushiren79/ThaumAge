using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static BiomeCreateTool;

public class BiomeMain : Biome
{

    public BiomeMain() : base(BiomeTypeEnum.Main)
    {

    }


    public override BlockTypeEnum GetBlockType(Chunk chunk, BiomeInfoBean biomeInfo, int genHeight, Vector3Int localPos, Vector3Int wPos)
    {
        base.GetBlockType(chunk, biomeInfo, genHeight, localPos, wPos);
        if (wPos.y == genHeight)
        {
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
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.WeedLong, BlockTypeEnum.WeedNormal, BlockTypeEnum.WeedShort }
        };
        BiomeCreateTool.AddPlant(333, wPos, weedData);
    }

    protected void AddFlower(Vector3Int wPos)
    {
        BiomeForFlowerData flowersData = new BiomeForFlowerData
        {
            addRate = 0.02f,
            listFlowerType = new List<BlockTypeEnum> { BlockTypeEnum.FlowerSun, BlockTypeEnum.FlowerRose, BlockTypeEnum.FlowerChrysanthemum, BlockTypeEnum.FlowerWood }
        };
        BiomeCreateTool.AddFlower(101, wPos, flowersData);
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
        BiomeCreateTool.AddTree(111, wPos, treeData);
    }

}