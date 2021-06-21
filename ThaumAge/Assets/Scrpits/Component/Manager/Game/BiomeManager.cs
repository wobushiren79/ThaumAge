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

    public List<Biome> listBiomeForMain = new List<Biome>();

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
    public List<Biome> GetBiomeListByWorldType(WorldTypeEnum worldType)
    {
        List<Biome> listBiome = new List<Biome>();
        switch (worldType)
        {
            case WorldTypeEnum.Main:
                if (CheckUtil.ListIsNull(listBiomeForMain))
                {
                    listBiomeForMain.Add(new BiomePrairie());
                    listBiomeForMain.Add(new BiomeForest());
                    listBiomeForMain.Add(new BiomeDesert());    
                    listBiomeForMain.Add(new BiomeVolcano());
                    listBiomeForMain.Add(new BiomeMagicForest());
                    listBiomeForMain.Add(new BiomeMountain());
                    listBiomeForMain.Add(new BiomeOcean());
                }
                listBiome = listBiomeForMain;
                break;
        }
        return listBiome;
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