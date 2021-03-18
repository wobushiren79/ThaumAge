using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BiomeManager : BaseManager,IBiomeInfoView
{
    protected BiomeInfoController controllerForBiome;

    protected Dictionary<BiomeTypeEnum, BiomeInfoBean> dicBiomeInfo = new Dictionary<BiomeTypeEnum, BiomeInfoBean>();

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
        dicBiomeInfo.Clear();
        for (int i = 0; i < listData.Count; i++)
        {
            BiomeInfoBean itemInfo = listData[i];
            dicBiomeInfo.Add(itemInfo.GetBiomeType(), itemInfo);
        }
    }

    /// <summary>
    /// 获取生态信息
    /// </summary>
    /// <param name="biomeType"></param>
    /// <returns></returns>
    public BiomeInfoBean GetBiomeInfo(BiomeTypeEnum biomeType)
    {
        if (dicBiomeInfo.TryGetValue(biomeType, out BiomeInfoBean blockInfo))
        {
            return blockInfo;
        }
        return null;
    }


    public BiomeInfoBean GetBiomeInfo(long biomeId)
    {
        return GetBiomeInfo((BiomeTypeEnum)biomeId);
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