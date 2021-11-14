using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static BiomeCreateTool;

public class BiomeMountain : Biome
{
    //高山
    public BiomeMountain() : base(BiomeTypeEnum.Mountain)
    {

    }

    public override BlockTypeEnum GetBlockType(Chunk chunk, BiomeInfoBean biomeInfo, int genHeight, Vector3Int localPos, Vector3Int wPos)
    {
        base.GetBlockType(chunk, biomeInfo, genHeight, localPos, wPos);
        if (wPos.y == genHeight)
        {
            AddWeed(wPos);
            AddFlower(wPos);
            // 地表，使用草
            return BlockTypeEnum.GrassWild;
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
            listFlowerType = new List<BlockTypeEnum> { BlockTypeEnum.FlowerGold }
        };
        BiomeCreateTool.AddFlower(101, wPos, flowersData);
    }

    protected void AddWeed(Vector3Int wPos)
    {
        BiomeForPlantData weedData = new BiomeForPlantData
        {
            addRate = 0.05f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.Weed_Long, BlockTypeEnum.Weed_Normal, BlockTypeEnum.Weed_Short }
        };
        BiomeCreateTool.AddPlant(201, wPos, weedData);
    }
}