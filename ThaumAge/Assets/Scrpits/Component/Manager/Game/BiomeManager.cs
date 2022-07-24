using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BiomeManager : BaseManager
    , IBiomeInfoView
    , IBuildingInfoView
{
    protected BiomeInfoController controllerForBiome;
    protected BuildingInfoController controllerForBuilding;

    protected BiomeInfoBean[] arrayBiomeInfo = new BiomeInfoBean[EnumExtension.GetEnumMaxIndex<BiomeTypeEnum>() + 1];
    protected BuildingInfoBean[] arrayBuildingInfo = new BuildingInfoBean[EnumExtension.GetEnumMaxIndex<BuildingTypeEnum>() + 1];

    //地形数据
    public Dictionary<BiomeTypeEnum, Biome> dicBiome = new Dictionary<BiomeTypeEnum, Biome>();
    //世界生态字典
    public Dictionary<int, BiomeTypeEnum[]> dicWorldBiome = new Dictionary<int, BiomeTypeEnum[]>();
    //缓存的地形数据
    public Dictionary<string, BiomeMapData> dicWorldChunkTerrainDataPool = new Dictionary<string, BiomeMapData>();


    //地形生成计算shader
    public ComputeShader terrainCShader;
    //路径-地形生成计算shader （使用标签）
    public static string pathForTerrainCShader = "Assets/ComputeShader/TerrainCShader.compute";

    public virtual void Awake()
    {
        controllerForBiome = new BiomeInfoController(this, this);
        controllerForBiome.GetAllBiomeInfoData(InitBiomeInfo);
        controllerForBuilding = new BuildingInfoController(this, this);
        controllerForBuilding.GetAllBuildingInfoData(InitBuildingInfo);
    }

    public virtual void LoadResources(Action loadComplete)
    {
        if (terrainCShader != null)
        {
            loadComplete?.Invoke();
            return;
        }
        //加载所有方块材质球
        LoadAddressablesUtil.LoadAssetAsync<ComputeShader>(pathForTerrainCShader, (data) =>
        {
            terrainCShader = data.Result;
            loadComplete?.Invoke();
        });
    }

    /// <summary>
    /// 初始化生态信息
    /// </summary>
    /// <param name="listData"></param>
    public void InitBiomeInfo(List<BiomeInfoBean> listData)
    {
        for (int i = 0; i < listData.Count; i++)
        {
            BiomeInfoBean itemInfo = listData[i];
            arrayBiomeInfo[itemInfo.id] = itemInfo;
        }
    }

    /// <summary>
    /// 初始化建筑信息
    /// </summary>
    /// <param name="listData"></param>
    public void InitBuildingInfo(List<BuildingInfoBean> listData)
    {
        for (int i = 0; i < listData.Count; i++)
        {
            BuildingInfoBean itemInfo = listData[i];
            arrayBuildingInfo[itemInfo.id] = itemInfo;
        }
    }

    /// <summary>
    /// 获取生态信息
    /// </summary>
    /// <param name="biomeType"></param>
    /// <returns></returns>
    public BiomeInfoBean GetBiomeInfo(BiomeTypeEnum biomeType)
    {
        return GetBiomeInfo((int)biomeType);
    }

    public BiomeInfoBean GetBiomeInfo(int biomeId)
    {
        return arrayBiomeInfo[biomeId];
    }

    /// <summary>
    /// 获取建筑信息
    /// </summary>
    /// <param name="biomeType"></param>
    /// <returns></returns>
    public BuildingInfoBean GetBuildingInfo(BuildingTypeEnum buildingType)
    {
        return GetBuildingInfo((int)buildingType);
    }

    public BuildingInfoBean GetBuildingInfo(int buildingId)
    {
        return arrayBuildingInfo[buildingId];
    }

    /// <summary>
    /// 获取生态数据
    /// </summary>
    /// <returns></returns>
    public virtual Biome GetBiome(BiomeTypeEnum biomeType)
    {
        //如果已经有了
        if (dicBiome.TryGetValue(biomeType, out Biome biome))
        {
            return biome;
        }
        else
        {
            //通过反射获取类
            biome = ReflexUtil.CreateInstance<Biome>($"Biome{biomeType.GetEnumName()}");
            if (biome != null) 
            {
                dicBiome.Add(biomeType,biome);
            }
            return biome;
        }
    }

    /// <summary>
    /// 根据世界类型获取生态数据
    /// </summary>
    /// <param name="worldType"></param>
    /// <returns></returns>
    public virtual BiomeTypeEnum[] GetBiomeListByWorldType(WorldTypeEnum worldType)
    {
        //如果已经有了
        if (dicWorldBiome.TryGetValue((int)worldType, out BiomeTypeEnum[] arrayBiome))
        {
            return arrayBiome;
        }
        else
        {
            switch (worldType)
            {
                case WorldTypeEnum.Test:
                    arrayBiome = new BiomeTypeEnum[1];
                    arrayBiome[0] = BiomeTypeEnum.Forest;
                    break;
                case WorldTypeEnum.Main:
                    arrayBiome = new BiomeTypeEnum[1];
                    arrayBiome[0] = BiomeTypeEnum.Forest;
                    break;
                case WorldTypeEnum.Launch:
                    arrayBiome = new BiomeTypeEnum[1];
                    arrayBiome[0] = BiomeTypeEnum.Main;
                    break;
                default:
                    arrayBiome = new BiomeTypeEnum[1];
                    arrayBiome[0] = BiomeTypeEnum.Test;
                    break;
            }
            dicWorldBiome.Add((int)worldType, arrayBiome);
            return arrayBiome;
        }
    }

    #region 数据回调
    public void GetBiomeInfoSuccess<T>(T data, Action<T> action)
    {
        action?.Invoke(data);
    }

    public void GetBiomeInfoFail(string failMsg, Action action)
    {
        LogUtil.Log("获取生态数据失败");
    }

    public void GetBuildingInfoSuccess<T>(T data, Action<T> action)
    {
        action?.Invoke(data);
    }

    public void GetBuildingInfoFail(string failMsg, Action action)
    {
        LogUtil.Log("获取建筑数据失败");
    }
    #endregion
}