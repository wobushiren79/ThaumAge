using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Biome
{
    //生态类型
    public BiomeTypeEnum biomeType;
    //生态数据
    public BiomeInfoBean biomeInfo;
    //生态数据
    public Terrain3DCShaderNoiseLayers[] terrain3DCShaderNoises;

    public Biome(BiomeTypeEnum biomeType)
    {
        this.biomeType = biomeType;
        biomeInfo = BiomeHandler.Instance.manager.GetBiomeInfo(this.biomeType);
        if (terrain3DCShaderNoises == null)
        {
            terrain3DCShaderNoises = new Terrain3DCShaderNoiseLayers[1];
            Terrain3DCShaderNoiseLayers itemTemp = new Terrain3DCShaderNoiseLayers();
            itemTemp.biomeId = (int)biomeInfo.id;
            itemTemp.frequency = biomeInfo.frequency;
            itemTemp.amplitude = biomeInfo.amplitude;
            itemTemp.lacunarity = biomeInfo.lacunarity;
            itemTemp.octaves = biomeInfo.octaves;
            itemTemp.caveScale = biomeInfo.caveScale;
            itemTemp.caveThreshold = biomeInfo.caveThreshold;
            itemTemp.caveFrequency = biomeInfo.caveFrequency;
            itemTemp.caveAmplitude = biomeInfo.caveAmplitude;
            itemTemp.caveOctaves = biomeInfo.caveOctaves;
            itemTemp.groundMinHeigh = biomeInfo.groundMinHeigh;
            terrain3DCShaderNoises[0] = itemTemp;
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