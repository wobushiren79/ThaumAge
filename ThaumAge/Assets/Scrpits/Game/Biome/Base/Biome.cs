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
    /// 初始化生态方块
    /// </summary>
    public BlockTypeEnum InitBiomeBlock(Chunk chunk, Vector3Int localPos, ChunkTerrainData chunkTerrainData)
    {
        if (localPos.y > chunkTerrainData.maxHeight)
        {
            return GetBlockForMaxHeightUp(chunk, localPos, chunkTerrainData);
        }
        else
        {
            return GetBlockForMaxHeightDown(chunk, localPos, chunkTerrainData);
        }
    }

    /// <summary>
    /// 初始化生态方块
    /// </summary>
    public virtual void InitBiomeBlockForChunk(Chunk chunk, BiomeMapData biomeMapData)
    {
        //生成洞穴 不放在每一个方块里去检测 提升效率
        //BiomeCreateTool.BiomeForCaveData caveData = new BiomeCreateTool.BiomeForCaveData();
        //caveData.minDepth = 100;
        //caveData.maxDepth = 200;
        //caveData.minSize = 3;
        //caveData.maxSize = 5;
        //BiomeCreateTool.AddCave(this, mapData, caveData);
    }

    /// <summary>
    /// 地面以上
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPos"></param>
    /// <param name="terrainData"></param>
    /// <returns></returns>
    public virtual BlockTypeEnum GetBlockForMaxHeightUp(Chunk chunk, Vector3Int localPos, ChunkTerrainData terrainData)
    {
        if (localPos.y <= biomeInfo.GetWaterPlaneHeight())
        {
            Block tagetBlock = chunk.chunkData.GetBlockForLocal(localPos);
            if (tagetBlock == null || tagetBlock.blockType == BlockTypeEnum.None)
            {
                return BlockTypeEnum.Water;
            }
            return BlockTypeEnum.None;
        }
        else
        {
            return BlockTypeEnum.None;
        }
    }

    /// <summary>
    /// 地面以下 包含地面
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPos"></param>
    /// <param name="terrainData"></param>
    /// <returns></returns>
    public virtual BlockTypeEnum GetBlockForMaxHeightDown(Chunk chunk, Vector3Int localPos, ChunkTerrainData terrainData)
    {
        return BlockTypeEnum.Stone;
    }

    public virtual ChunkTerrainData GetTerrainData(Chunk chunk, BiomeMapData biomeMapData, int xPosition, int zPosition)
    {
        ChunkTerrainData targetTerrainData = biomeMapData.arrayChunkTerrainData[xPosition * chunk.chunkData.chunkWidth + zPosition];
        return targetTerrainData;
    }
}