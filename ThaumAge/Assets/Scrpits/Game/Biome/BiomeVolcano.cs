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

    public override BlockTypeEnum GetBlockType(Chunk chunk, Vector3Int localPos, ChunkTerrainData terrainData)
    {
        base.GetBlockType(chunk, localPos, terrainData);
        float noise = (terrainData.maxHeight - biomeInfo.min_height) / biomeInfo.amplitude;
        if (noise >= 0.9f)
        {
            if (localPos.y >= terrainData.maxHeight - 3)
            {
                return BlockTypeEnum.None;
            }
            else if (localPos.y > 20 && localPos.y < terrainData.maxHeight - 1)
            {
                return BlockTypeEnum.Magma;
            }
            else
            {
                return BlockTypeEnum.StoneVolcanic;
            }
        }
        if (terrainData.maxHeight == localPos.y)
        {
            Vector3Int wPos = localPos + chunk.chunkData.positionForWorld;
            AddDeadwood(wPos);
            AddFireFlower(wPos);
        }
        return BlockTypeEnum.StoneVolcanic;
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