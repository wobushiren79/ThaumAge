using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static BiomeCreatePlantTool;
using static BiomeCreateTool;

public class BiomeVolcano : Biome
{
    //火山
    public BiomeVolcano() : base(BiomeTypeEnum.Volcano)
    {
    }

    public override BlockTypeEnum GetBlockForMaxHeightDown(Chunk chunk, Vector3Int localPos, ChunkTerrainData terrainData)
    {
        if (localPos.y == terrainData.maxHeight)
        {
            Vector3Int wPos = localPos + chunk.chunkData.positionForWorld;
            int maxHight = biomeInfo.min_height + (int)biomeInfo.amplitude0 / 2;

            // 地表
            return BlockTypeEnum.StoneVolcanic;
        }
        if (localPos.y < terrainData.maxHeight && localPos.y > terrainData.maxHeight - 50)
        {
            //中使用泥土
            return BlockTypeEnum.StoneVolcanic;
        }
        else if (localPos.y == 0)
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

    /// <summary>
    /// 增加枯木
    /// </summary>
    /// <param name="startPosition"></param>
    public void AddDeadwood(Vector3Int startPosition)
    {
        BiomeCreatePlantTool.AddDeadwood(101, 0.005f, startPosition);
    }

    /// <summary>
    /// 增加火焰花
    /// </summary>
    /// <param name="wPos"></param>
    public void AddFireFlower(Vector3Int wPos)
    {
        BiomeForPlantData flowersData = new BiomeForPlantData
        {
            addRate = 0.005f,
            listPlantType = new List<BlockTypeEnum> { BlockTypeEnum.FlowerFire }
        };
        BiomeCreatePlantTool.AddFlower(201, wPos, flowersData);
    }
}