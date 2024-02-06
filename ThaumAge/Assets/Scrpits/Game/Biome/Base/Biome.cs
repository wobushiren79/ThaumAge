using System;
using System.Collections.Generic;
using TreeEditor;
using UnityEditor;
using UnityEngine;

public class Biome
{
    //生态类型
    public BiomeTypeEnum biomeType;
    //生态数据
    public BiomeInfoBean biomeInfo;
    //生态数据
    public Terrain3DCShaderNoiseLayer terrain3DCShaderNoise;
    public Terrain3DShaderOreData[] terrain3DCShaderOre;
    public Biome(BiomeTypeEnum biomeType)
    {
        this.biomeType = biomeType;
        biomeInfo = BiomeHandler.Instance.manager.GetBiomeInfo(this.biomeType);
        terrain3DCShaderNoise = new Terrain3DCShaderNoiseLayer();
        terrain3DCShaderNoise.biomeId = (int)biomeInfo.id;
        terrain3DCShaderNoise.frequency = biomeInfo.frequency;
        terrain3DCShaderNoise.amplitude = biomeInfo.amplitude;
        terrain3DCShaderNoise.lacunarity = biomeInfo.lacunarity;
        terrain3DCShaderNoise.octaves = biomeInfo.octaves;
        terrain3DCShaderNoise.caveMinHeight = biomeInfo.caveMinHeight;
        terrain3DCShaderNoise.caveMaxHeight = biomeInfo.caveMaxHeight;
        terrain3DCShaderNoise.caveScale = biomeInfo.caveScale;
        terrain3DCShaderNoise.caveThreshold = biomeInfo.caveThreshold;
        terrain3DCShaderNoise.caveFrequency = biomeInfo.caveFrequency;
        terrain3DCShaderNoise.caveAmplitude = biomeInfo.caveAmplitude;
        terrain3DCShaderNoise.caveOctaves = biomeInfo.caveOctaves;
        terrain3DCShaderNoise.groundMinHeigh = biomeInfo.groundMinHeigh;
        terrain3DCShaderNoise.oceanMinHeight = biomeInfo.oceanMinHeight;
        terrain3DCShaderNoise.oceanMaxHeight = biomeInfo.oceanMaxHeight;

        terrain3DCShaderNoise.oceanScale = biomeInfo.oceanScale;
        terrain3DCShaderNoise.oceanThreshold = biomeInfo.oceanThreshold;
        terrain3DCShaderNoise.oceanAmplitude = biomeInfo.oceanAmplitude;
        terrain3DCShaderNoise.oceanFrequency = biomeInfo.oceanFrequency;
        //设置矿石数据
        string oreDataStr = biomeInfo.oreData;
        if (oreDataStr.IsNull())
        {
            terrain3DCShaderOre = new Terrain3DShaderOreData[0];
        }
        else
        {
            string[] oreDataStrArray = oreDataStr.Split('&');
            terrain3DCShaderOre = new Terrain3DShaderOreData[oreDataStrArray.Length];
            for (int i = 0; i < oreDataStrArray.Length; i++)
            {
                string[] itemOreDataStrArray = oreDataStrArray[i].Split('_');
                Terrain3DShaderOreData terrain3DShaderOre = new Terrain3DShaderOreData();
                terrain3DShaderOre.oreId = int.Parse(itemOreDataStrArray[0]);
                terrain3DShaderOre.oreDensity = float.Parse(itemOreDataStrArray[1]);
                terrain3DShaderOre.oreMinHeight = int.Parse(itemOreDataStrArray[2]);
                terrain3DShaderOre.oreMaxHeight = int.Parse(itemOreDataStrArray[3]);
                terrain3DCShaderOre[i] = terrain3DShaderOre;
            }
        }

        //如果是测试生态 直接获取GameLauncher里的数据
        if (biomeType == BiomeTypeEnum.Test)
        {
            if (GameHandler.Instance.launcher is GameLauncher gameLauncher)
            {
                terrain3DCShaderNoise.biomeId = (int)biomeInfo.id;
                terrain3DCShaderNoise = gameLauncher.testTerrain3DCShaderNoise;
            }
        }
    }

    /// <summary>
    /// 创建结构方框
    /// </summary>
    public virtual void CreateBlockStructure(Chunk chunk, int blockId, BlockStructureEnum blockStructure, Vector3Int baseWorldPosition)
    {
        switch (blockStructure)
        {
            case BlockStructureEnum.NormalTree:
                CreateBlockStructureForNormalTree(blockId, baseWorldPosition);
                break;
            case BlockStructureEnum.NormalTreeSnow:
                CreateBlockStructureForNormalTreeSnow(blockId, baseWorldPosition);
                break;
            case BlockStructureEnum.DeadWood:
                CreateBlockStructureForDeadWood(blockId, baseWorldPosition);
                break;
            case BlockStructureEnum.FallDownTree:
                CreateBlockStructureForFallDownTree(blockId, baseWorldPosition);
                break;
            case BlockStructureEnum.Cactus:
                CreateBlockStructureForCactus(blockId, baseWorldPosition);
                break;
        }
    }

    /// <summary>
    /// 创建普通树
    /// </summary>
    /// <param name="blockType"></param>
    /// <param name="baseWorldPosition"></param>
    public virtual void CreateBlockStructureForNormalTree(int blockId, Vector3Int baseWorldPosition)
    {
        BiomeCreateTreeTool.CreateNormalTree(baseWorldPosition, blockId, (int)BlockTypeEnum.LeavesOak);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="blockId"></param>
    /// <param name="baseWorldPosition"></param>
    public virtual void CreateBlockStructureForNormalTreeSnow(int blockId, Vector3Int baseWorldPosition)
    {
        BiomeCreateTreeTool.CreateNormalTreeSnow(baseWorldPosition, blockId, (int)BlockTypeEnum.LeavesOak);
    }

    /// <summary>
    /// 创建枯木
    /// </summary>
    /// <param name="blockId"></param>
    /// <param name="baseWorldPosition"></param>
    public virtual void CreateBlockStructureForDeadWood(int blockId, Vector3Int baseWorldPosition)
    {
        //增加枯木
        BiomeCreatePlantTool.AddDeadwood(baseWorldPosition);
    }

    /// <summary>
    /// 创建倒下的树
    /// </summary>
    public virtual void CreateBlockStructureForFallDownTree(int blockId, Vector3Int baseWorldPosition)
    {
        BiomeCreateTreeTool.AddTreeForFallDown(baseWorldPosition, blockId);
    }

    /// <summary>
    /// 创建仙人掌
    /// </summary>
    /// <param name="blockId"></param>
    /// <param name="baseWorldPosition"></param>
    public virtual void CreateBlockStructureForCactus(int blockId, Vector3Int baseWorldPosition)
    {
        BiomeCreateTreeTool.AddCactus(baseWorldPosition, blockId);
    }

    /// <summary>
    /// 初始化生态方块
    /// </summary>
    public BlockTypeEnum InitBiomeBlock(Chunk chunk, Vector3Int localPos)
    {
        return BlockTypeEnum.None;
        //if (localPos.y == 0)
        //{
        //    return BlockTypeEnum.Foundation;
        //}
        //else if (localPos.y > chunkTerrainData.maxHeight)
        //{
        //    return GetBlockForMaxHeightUp(chunk, localPos, chunkTerrainData);
        //}
        //else
        //{
        //    //int worldX = localPos.x + chunk.chunkData.positionForWorld.x;
        //    //int worldZ = localPos.z + chunk.chunkData.positionForWorld.z;

        //    //float heightGradient = Mathf.Pow(Mathf.Clamp01(localPos.y / 128f), 2f);
        //    ////洞穴生成
        //    //float c1 = SimplexNoiseUtil.CalcPixel3D(worldX, localPos.y, worldZ, 0.1f);
        //    //float c2 = SimplexNoiseUtil.CalcPixel3D(worldX, localPos.y, worldZ, 0.04f);
        //    //float c3 = SimplexNoiseUtil.CalcPixel3D(worldX, localPos.y, worldZ, 0.02f);
        //    //float c4 = SimplexNoiseUtil.CalcPixel3D(worldX, localPos.y, worldZ, 0.01f);

        //    //c1 += (heightGradient);
        //    //if (c1 < .5 && c2 < .5 && c3 < .5 && c4 < .5)
        //    //{
        //    //    return BlockTypeEnum.None;
        //    //}

        //    ////矿物生成
        //    //float oreCopper = SimplexNoiseUtil.CalcPixel3D(worldX + oreCopperOffsetX, localPos.y, worldZ, oreCopperSize);
        //    //if (oreCopper < oreCopperOdds)
        //    //{
        //    //    return BlockTypeEnum.OreCopper;
        //    //}

        //    //float oreIron = SimplexNoiseUtil.CalcPixel3D(worldX + oreIronOffsetX, localPos.y, worldZ, oreIronSize);
        //    //if (oreIron < oreIronOdds)
        //    //{
        //    //    return BlockTypeEnum.OreIron;
        //    //}

        //    //float oreSilver = SimplexNoiseUtil.CalcPixel3D(worldX + oreSilverOffsetX, localPos.y, worldZ, oreSilverSize);
        //    //if (oreSilver < oreSilverOdds)
        //    //{
        //    //    return BlockTypeEnum.OreSilver;
        //    //}

        //    //float oreGold = SimplexNoiseUtil.CalcPixel3D(worldX + oreGoldOffsetX, localPos.y, worldZ, oreGoldSize);
        //    //if (oreGold < oreGoldOdds)
        //    //{
        //    //    return BlockTypeEnum.OreGold;
        //    //}

        //    //float oreTin = SimplexNoiseUtil.CalcPixel3D(worldX + oreTinOffsetX, localPos.y, worldZ, oreTinSize);
        //    //if (oreTin < oreTinOdds)
        //    //{
        //    //    return BlockTypeEnum.OreTin;
        //    //}

        //    //float oreAluminum = SimplexNoiseUtil.CalcPixel3D(worldX + oreAluminumOffsetX, localPos.y, worldZ, oreAluminumSize);
        //    //if (oreAluminum < oreAluminumOdds)
        //    //{
        //    //    return BlockTypeEnum.OreAluminum;
        //    //}

        //    //float oreZinc  = SimplexNoiseUtil.CalcPixel3D(worldX + oreZincOffsetX, localPos.y, worldZ, oreZincSize);
        //    //if (oreZinc < oreZincOdds)
        //    //{
        //    //    return BlockTypeEnum.OreZinc;
        //    //}
        //    return GetBlockForMaxHeightDown(chunk, localPos, chunkTerrainData);
        //}
    }
}