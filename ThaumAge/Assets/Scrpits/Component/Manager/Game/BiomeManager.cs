﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BiomeManager : BaseManager
    , IBuildingInfoView
{
    protected BuildingInfoController controllerForBuilding;

    protected Dictionary<int, BuildingInfoBean> dicBuildingInfo = new Dictionary<int, BuildingInfoBean>();

    //地形数据
    public Dictionary<BiomeTypeEnum, Biome> dicBiome = new Dictionary<BiomeTypeEnum, Biome>();
    //世界生态字典
    public Dictionary<WorldTypeEnum, BiomeTypeEnum[]> dicWorldBiomeType = new Dictionary<WorldTypeEnum, BiomeTypeEnum[]>();
    //世界生态数据
    public static Dictionary<WorldTypeEnum, BiomeInfoBean[]> dicWorldBiomeData = new Dictionary<WorldTypeEnum, BiomeInfoBean[]>();

    //地形生成计算shader
    public ComputeShader terrainCShader;
    //路径-地形生成计算shader （使用标签）
    public static string pathForTerrainCShader = "Assets/ComputeShader/TerrainCShader.compute";

    //生态区块大小
    public int biomeChunkSize = 2;

    public virtual void Awake()
    {
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
    /// 初始化建筑信息
    /// </summary>
    /// <param name="listData"></param>
    public void InitBuildingInfo(List<BuildingInfoBean> listData)
    {
        dicBuildingInfo.Clear();
        for (int i = 0; i < listData.Count; i++)
        {
            BuildingInfoBean itemInfo = listData[i];
            dicBuildingInfo.Add((int)itemInfo.id, itemInfo);
        }
    }

    /// <summary>
    /// 获取生态信息
    /// </summary>
    public BiomeInfoBean GetBiomeInfo(BiomeTypeEnum biomeType)
    {
        return GetBiomeInfo((int)biomeType);
    }

    /// <summary>
    /// 获取生态信息
    /// </summary>
    public BiomeInfoBean GetBiomeInfo(int biomeId)
    {
        return  BiomeInfoCfg.GetItemData(biomeId);
    }

    /// <summary>
    /// 获取建筑信息
    /// </summary>
    public BuildingInfoBean GetBuildingInfo(BuildingTypeEnum buildingType)
    {
        return GetBuildingInfo((int)buildingType);
    }

    /// <summary>
    /// 获取建筑信息
    /// </summary>
    public BuildingInfoBean GetBuildingInfo(int buildingId)
    {
        return base.GetDataById(buildingId, dicBuildingInfo);
    }

    /// <summary>
    /// 获取指定区块的生态类型
    /// </summary>
    /// <returns></returns>
    public virtual BiomeTypeEnum GetBiomeType(Vector3Int chunkWorldPosition, int chunkWidth, WorldTypeEnum worldType, int seed)
    {
        BiomeTypeEnum[] biomeTypes = GetBiomeTypeListByWorldType(worldType);
        float posX = chunkWorldPosition.x - (chunkWorldPosition.x % (chunkWidth * biomeChunkSize));
        float posZ = chunkWorldPosition.z - (chunkWorldPosition.z % (chunkWidth * biomeChunkSize));
        Vector3 centerPos = new Vector3(posX, posZ);
        int randomRate = WorldRandTools.Range(0, biomeTypes.Length, centerPos, (uint)seed);
        return biomeTypes[randomRate];
    }

    /// <summary>
    /// 获取生态数据
    /// </summary>
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
            Biome biomeTarget = ReflexUtil.CreateInstance<Biome>($"Biome{biomeType.GetEnumName()}");
            if (biomeTarget != null)
            {
                dicBiome.Add(biomeType, biomeTarget);
            }
            return biomeTarget;
        }
    }

    /// <summary>
    /// 根据世界类型获取生态数据
    /// </summary>
    /// <param name="worldType"></param>
    /// <returns></returns>
    public virtual BiomeTypeEnum[] GetBiomeTypeListByWorldType(WorldTypeEnum worldType)
    {
        //如果已经有了
        if (dicWorldBiomeType.TryGetValue(worldType, out BiomeTypeEnum[] arrayBiome))
        {
            return arrayBiome;
        }
        else
        {
            var targetWorldInfo = WorldInfoCfg.GetItemData((int)worldType);
            string biomeContent = targetWorldInfo.biome_content;
            int[] biomesData = biomeContent.SplitForArrayInt(',');
            arrayBiome = new BiomeTypeEnum[biomesData.Length];
            for (int i = 0; i < biomesData.Length; i++)
            {
                int biomeId = biomesData[i];
                arrayBiome[i] = (BiomeTypeEnum)biomeId;
            }
            dicWorldBiomeType.Add(worldType, arrayBiome);
            return arrayBiome;
        }
    }

    /// <summary>
    /// 获取世界的生态信息
    /// </summary>
    /// <param name="worldType"></param>
    /// <param name="chunk"></param>
    /// <returns></returns>
    public virtual BiomeInfoBean[] GetBiomeDataByWorldType(WorldTypeEnum worldType, Chunk chunk = null)
    {
        //如果已经有了
        if (dicWorldBiomeData.TryGetValue(worldType, out BiomeInfoBean[] arrayBiome))
        {
            return arrayBiome;
        }
        else
        {
            //获取该世界的所有生态
            BiomeTypeEnum[] listBiome = GetBiomeTypeListByWorldType(worldType);
            arrayBiome = new BiomeInfoBean[listBiome.Length];
            for (int i = 0; i < listBiome.Length; i++)
            {
                BiomeTypeEnum itemBiomeType = listBiome[i];
                arrayBiome[i] = BiomeInfoCfg.GetItemData((long)itemBiomeType);
            }

            dicWorldBiomeData.Add(worldType, arrayBiome);
            return arrayBiome;
        }
    }

    #region 数据回调

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