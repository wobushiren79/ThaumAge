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

    public override BlockTypeEnum GetBlockType(Chunk chunk, Vector3Int localPos, ChunkTerrainData terrainData)
    {
        base.GetBlockType(chunk, localPos, terrainData);
        int trueHeight = 10;
        if (localPos.y == trueHeight)
        {
            Vector3Int wPos = localPos + chunk.chunkData.positionForWorld;
            AddFlower(wPos);
            return BlockTypeEnum.Dirt;
        }
        if (localPos.y < trueHeight && localPos.y > trueHeight - 2)
        {
            //中使用泥土
            return BlockTypeEnum.Dirt;
        }
        else if (localPos.y == 0)
        {
            //基础
            return BlockTypeEnum.Foundation;
        }
        else if (localPos.y <= trueHeight - 2)
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