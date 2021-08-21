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

    protected BiomeInfoBean[] arrayBiomeInfo = new BiomeInfoBean[EnumUtil.GetEnumMaxIndex<BiomeTypeEnum>() + 1];
    protected BuildingInfoBean[] arrayBuildingInfo = new BuildingInfoBean[EnumUtil.GetEnumMaxIndex<BuildingTypeEnum>() + 1];

    //世界生态字典
    public Dictionary<int, Biome[]> dicWorldBiome = new Dictionary<int, Biome[]>();

    public virtual void Awake()
    {
        controllerForBiome = new BiomeInfoController(this, this);
        controllerForBiome.GetAllBiomeInfoData(InitBiomeInfo);
        controllerForBuilding = new BuildingInfoController(this, this);
        controllerForBuilding.GetAllBuildingInfoData(InitBuildingInfo);
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
    /// 根据世界类型获取生态数据
    /// </summary>
    /// <param name="worldType"></param>
    /// <returns></returns>
    public virtual Biome[] GetBiomeListByWorldType(WorldTypeEnum worldType)
    {
        //如果已经有了
        if(dicWorldBiome.TryGetValue((int)worldType,out Biome[] arrayBiome))
        {
            return arrayBiome;
        }
        else
        {
            switch (worldType)
            {
                case WorldTypeEnum.Test:
                    arrayBiome= new Biome[1];
                    arrayBiome[0] = new BiomeTest();
                    break;
                case WorldTypeEnum.Main:
                    arrayBiome = new Biome[7];
                    arrayBiome[0] = new BiomePrairie();
                    arrayBiome[1] = new BiomeForest();
                    arrayBiome[2] = new BiomeDesert();
                    arrayBiome[3] = new BiomeMagicForest();
                    arrayBiome[4] = new BiomeVolcano();
                    arrayBiome[5] = new BiomeMountain();
                    arrayBiome[6] = new BiomeOcean();
                    break;
                case WorldTypeEnum.Launch:
                    arrayBiome = new Biome[1];
                    arrayBiome[0] = new BiomeMain();
                    break;
                default:
                    arrayBiome = new Biome[0];
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