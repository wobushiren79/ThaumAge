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
            terrain3DCShaderOre = new Terrain3DShaderOreData[1];
            Terrain3DShaderOreData terrain3DShaderOre = new Terrain3DShaderOreData();
            terrain3DShaderOre.oreId = 0;
            terrain3DShaderOre.oreDensity = 0;
            terrain3DShaderOre.oreMinHeight = 0;
            terrain3DShaderOre.oreMaxHeight = 0;
            terrain3DCShaderOre[0] = terrain3DShaderOre;
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
    public virtual void CreateBlockBuilding(Chunk chunk, int blockId, int blockBuilding, Vector3Int baseWorldPosition)
    {
        BuildingBaseType buildingType = BiomeHandler.Instance.manager.GetBuildingType(blockBuilding);
        buildingType.CreateBuilding(blockId, baseWorldPosition);
    }
}