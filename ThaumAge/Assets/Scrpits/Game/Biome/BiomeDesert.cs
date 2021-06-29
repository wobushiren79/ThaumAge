using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static BiomeCreateTool;

public class BiomeDesert : Biome
{
    //沙漠
    public BiomeDesert() : base(BiomeTypeEnum.Desert)
    {
    }

    public override BlockTypeEnum GetBlockType(Chunk chunk, BiomeInfoBean biomeInfo, int genHeight, Vector3Int localPos, Vector3Int wPos)
    {
        base.GetBlockType(chunk, biomeInfo, genHeight, localPos, wPos);
        if (wPos.y == genHeight)
        {
            AddCactus(wPos);
            AddWeed(wPos);
            AddFlower(wPos);
        }
        if (wPos.y <= genHeight && wPos.y > genHeight - 30)
        {
            return BlockTypeEnum.Sand;
        }
        if (wPos.y <= genHeight - 30 && wPos.y > genHeight - 35)
        {
            return BlockTypeEnum.Dirt;
        }
        else if (wPos.y == 0)
        {
            //基础
            return BlockTypeEnum.Foundation;
        }
        else
        {
            return BlockTypeEnum.Stone;
        }
    }

    public void AddCactus(Vector3Int startPosition)
    {
        BiomeForCactusData cactusData = new BiomeForCactusData();
        cactusData.addRate = 0.1f;
        cactusData.minHeight = 1;
        cactusData.maxHeight = 5;
        cactusData.cactusType = BlockTypeEnum.Cactus;
        BiomeCreateTool.AddCactus(1, startPosition, cactusData);
    }

    protected void AddFlower(Vector3Int wPos)
    {
        BiomeForFlowerData flowersData = new BiomeForFlowerData
        {
            addRate = 0.01f,
            listFlowerType = new List<BlockTypeEnum> { BlockTypeEnum.FlowerFire }
        };
        BiomeCreateTool.AddFlower(101, wPos, flowersData);
    }

    protected void AddWeed(Vector3Int wPos)
    {
        BiomeForPlantData weedData = new BiomeForPlantData
        {
            addRate = 0.02f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.Weed_Long, BlockTypeEnum.Weed_Normal, BlockTypeEnum.Weed_Short }
        };
        BiomeCreateTool.AddPlant(201, wPos, weedData);
    }
}