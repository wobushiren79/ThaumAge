using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static BiomeCreatePlantTool;
using static BiomeCreateTool;

public class BiomeOcean : Biome
{
    //海洋
    public BiomeOcean() : base(BiomeTypeEnum.Ocean)
    {

    }

    public override BlockTypeEnum GetBlockType(Chunk chunk, BiomeInfoBean biomeInfo, int genHeight, Vector3Int localPos, Vector3Int wPos)
    {
        base.GetBlockType(chunk, biomeInfo, genHeight, localPos, wPos);
        int trueHeight = BiomeHandler.Instance.GetHeightData(wPos, 0.025f, 30f, 30);
        if (wPos.y == trueHeight)
        {
            AddFlower(wPos);
            return BlockTypeEnum.Dirt;
        }
        if (wPos.y < trueHeight && wPos.y > trueHeight - 2)
        {
            //中使用泥土
            return BlockTypeEnum.Dirt;
        }
        else if (wPos.y == 0)
        {
            //基础
            return BlockTypeEnum.Foundation;
        }
        else if (wPos.y <= trueHeight - 2)
        {
            //石头
            return BlockTypeEnum.Stone;
        }
        else
        {
            //海水
            return BlockTypeEnum.Water;
        }
    }

    public void AddFlower(Vector3Int wPos)
    {
        BiomeForPlantData flowersData = new BiomeForPlantData
        {
            addRate = 0.005f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.FlowerWater }
        };
        BiomeCreatePlantTool.AddFlower(101, wPos, flowersData);
    }
}