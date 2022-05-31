using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static BiomeCreatePlantTool;
using static BiomeCreateTool;
using static BiomeCreateTreeTool;

public class BiomeDesert : Biome
{
    //沙漠
    public BiomeDesert() : base(BiomeTypeEnum.Desert)
    {
    }

    public override BlockTypeEnum GetBlockType(Chunk chunk, Vector3Int localPos, ChunkTerrainData terrainData)
    {
        base.GetBlockType(chunk, localPos, terrainData);
        if (localPos.y == terrainData.maxHeight)
        {
            Vector3Int wPos = localPos + chunk.chunkData.positionForWorld;
            AddCactus(wPos);
            AddWeed(wPos);
            AddFlower(wPos);
        }
        if (localPos.y <= terrainData.maxHeight && localPos.y > terrainData.maxHeight - 30)
        {
            return BlockTypeEnum.Sand;
        }
        if (localPos.y <= terrainData.maxHeight - 30 && localPos.y > terrainData.maxHeight - 35)
        {
            return BlockTypeEnum.Dirt;
        }
        else if (localPos.y == 0)
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
        BiomeForTreeData cactusData = new BiomeForTreeData();
        cactusData.addRate = 0.1f;
        cactusData.minHeight = 1;
        cactusData.maxHeight = 5;
        cactusData.treeTrunk = BlockTypeEnum.Cactus;
        BiomeCreateTreeTool.AddCactus(1, startPosition, cactusData);
    }

    protected void AddFlower(Vector3Int wPos)
    {
        BiomeForPlantData flowersData = new BiomeForPlantData
        {
            addRate = 0.01f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.FlowerFire }
        };
        BiomeCreatePlantTool.AddFlower(101, wPos, flowersData);
    }

    protected void AddWeed(Vector3Int wPos)
    {
        BiomeForPlantData weedData = new BiomeForPlantData
        {
            addRate = 0.02f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.WeedLong, BlockTypeEnum.WeedNormal, BlockTypeEnum.WeedShort }
        };
        BiomeCreatePlantTool.AddPlant(201, wPos, weedData);
    }
}