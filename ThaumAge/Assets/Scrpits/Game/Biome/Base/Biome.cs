using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static BiomeCreateTool;

public class Biome
{
    public BiomeTypeEnum biomeType;
    public BiomeInfoBean biomeInfo;

    public Biome(BiomeTypeEnum biomeType)
    {
        this.biomeType = biomeType;
        biomeInfo = BiomeHandler.Instance.manager.GetBiomeInfo(this.biomeType);
    }


    /// <summary>
    /// 获取方块类型
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPos"></param>
    /// <param name="terrainData"></param>
    /// <returns></returns>
    public virtual BlockTypeEnum GetBlockType(Chunk chunk, Vector3Int localPos, ChunkTerrainData terrainData)
    {
        return BlockTypeEnum.Stone;
    }

    public virtual void GetBlockTypeForChunk(Chunk chunk, BiomeMapData biomeMapData)
    {
        //生成洞穴 不放在每一个方块里去检测 提升效率
        //BiomeCreateTool.BiomeForCaveData caveData = new BiomeCreateTool.BiomeForCaveData();
        //caveData.minDepth = 100;
        //caveData.maxDepth = 200;
        //caveData.minSize = 3;
        //caveData.maxSize = 5;
        //BiomeCreateTool.AddCave(this, mapData, caveData);
    }


    protected virtual ChunkTerrainData GetTerrainData(Chunk chunk, BiomeMapData biomeMapData, int xPosition, int zPosition)
    {
        ChunkTerrainData targetTerrainData = biomeMapData.arrayChunkTerrainData[xPosition * chunk.chunkData.chunkWidth + zPosition];
        return targetTerrainData;
    }
}