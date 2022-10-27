using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static BiomeCreateTool;

public class Biome
{
    public BiomeTypeEnum biomeType;
    public BiomeInfoBean biomeInfo;

    public static int oreCopperOffsetX = 10000;//铜
    public static float oreCopperSize = 0.1f;//铜
    public static float oreCopperOdds = 0.1f;

    public static int oreIronOffsetX = 20000;//铁
    public static float oreIronSize = 0.1f;//铁
    public static float oreIronOdds = 0.1f;

    public static int oreSilverOffsetX = 30000;//银
    public static float oreSilverSize = 0.05f;//银
    public static float oreSilverOdds = 0.1f;

    public static int oreGoldOffsetX = 40000;//金
    public static float oreGoldSize = 0.01f;//金
    public static float oreGoldOdds = 0.1f;

    public static int oreTinOffsetX = 50000;//锡
    public static float oreTinSize = 0.05f;//锡
    public static float oreTinOdds = 0.1f;

    public static int oreAluminumOffsetX = 60000;//铝
    public static float oreAluminumSize = 0.05f;//铝
    public static float oreAluminumOdds = 0.1f;

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
            int worldX = localPos.x + chunk.chunkData.positionForWorld.x;
            int worldZ = localPos.z + chunk.chunkData.positionForWorld.z;

            float heightGradient = Mathf.Pow(Mathf.Clamp01(localPos.y / 128f), 2f);
            //洞穴生成
            float c1 = SimplexNoiseUtil.CalcPixel3D(worldX, localPos.y, worldZ, 0.1f);
            float c2 = SimplexNoiseUtil.CalcPixel3D(worldX, localPos.y, worldZ, 0.04f);
            float c3 = SimplexNoiseUtil.CalcPixel3D(worldX, localPos.y, worldZ, 0.02f);
            float c4 = SimplexNoiseUtil.CalcPixel3D(worldX, localPos.y, worldZ, 0.01f);

            c1 += (heightGradient);
            if (c1 < .5 && c2 < .5 && c3 < .5 && c4 < .5)
            {
                return BlockTypeEnum.None;
            }

            //矿物生成
            float oreCopper = SimplexNoiseUtil.CalcPixel3D(worldX + oreCopperOffsetX, localPos.y, worldZ, oreCopperSize);
            if (oreCopper < oreCopperOdds)
            {
                return BlockTypeEnum.OreCopper;
            }

            float oreIron = SimplexNoiseUtil.CalcPixel3D(worldX + oreIronOffsetX, localPos.y, worldZ, oreIronSize);
            if (oreIron < oreIronOdds)
            {
                return BlockTypeEnum.OreIron;
            }

            float oreSilver = SimplexNoiseUtil.CalcPixel3D(worldX + oreSilverOffsetX, localPos.y, worldZ, oreSilverSize);
            if (oreSilver < oreSilverOdds)
            {
                return BlockTypeEnum.OreSilver;
            }

            float oreGold = SimplexNoiseUtil.CalcPixel3D(worldX + oreGoldOffsetX, localPos.y, worldZ, oreGoldSize);
            if (oreGold < oreGoldOdds)
            {
                return BlockTypeEnum.OreGold;
            }

            float oreTin = SimplexNoiseUtil.CalcPixel3D(worldX + oreTinOffsetX, localPos.y, worldZ, oreTinSize);
            if (oreTin < oreTinOdds)
            {
                return BlockTypeEnum.OreTin;
            }

            float oreAluminum = SimplexNoiseUtil.CalcPixel3D(worldX + oreAluminumOffsetX, localPos.y, worldZ, oreAluminumSize);
            if (oreAluminum < oreAluminumOdds)
            {
                return BlockTypeEnum.OreAluminum;
            }
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