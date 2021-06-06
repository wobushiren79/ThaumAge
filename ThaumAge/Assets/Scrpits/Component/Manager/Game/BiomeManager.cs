using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BiomeManager : BaseManager,IBiomeInfoView
{
    protected BiomeInfoController controllerForBiome;

    protected BiomeInfoBean[] arrayBiomeInfo = new BiomeInfoBean[EnumUtil.GetEnumMaxIndex<BiomeTypeEnum>()+1];

    public List<Biome> listBiomeForMain = new List<Biome>();

    public virtual void Awake()
    {
        controllerForBiome = new BiomeInfoController(this, this);
        controllerForBiome.GetAllBiomeInfoData(InitBiomeInfo);
    }

    /// <summary>
    /// 初始化方块信息
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
                    listBiomeForMain.Add(new BiomeMagicForest());
                    listBiomeForMain.Add(new BiomeVolcano());
                    listBiomeForMain.Add(new BiomeMountain());
                    listBiomeForMain.Add(new BiomeOcean());
                }
                listBiome = listBiomeForMain;
                break;
        }
        return listBiome;
    }

    #region 方块数据回调
    public void GetBiomeInfoSuccess<T>(T data, Action<T> action)
    {
        action?.Invoke(data);
    }

    public void GetBiomeInfoFail(string failMsg, Action action)
    {
        LogUtil.Log("获取生态数据失败");
    }
    #endregion
}