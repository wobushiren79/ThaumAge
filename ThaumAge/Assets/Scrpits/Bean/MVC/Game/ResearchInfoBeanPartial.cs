using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class ResearchInfoBean
{
    /// <summary>
    /// 获取解锁前置研究
    /// </summary>
    /// <returns></returns>
    public int[] GetUnlockPreResearch()
    {
        return unlock_pre_research.SplitForArrayInt(',');
    }
}

public partial class ResearchInfoCfg
{
    /// <summary>
    /// 通过类型获取数据
    /// </summary>
    /// <param name="researchType"></param>
    /// <returns></returns>
    public static List<ResearchInfoBean> GetResearchInfoByType(int researchType)
    {
        List<ResearchInfoBean> listData = new List<ResearchInfoBean>();
        var allData = GetAllData();
        foreach (var itemData in allData)
        {
            ResearchInfoBean itemInfo = itemData.Value;
            if (researchType == itemInfo.type_research)
            {
                listData.Add(itemInfo);
            }
        }
        return listData;
    }

    /// <summary>
    /// 通过具体的类型获取数据
    /// </summary>
    /// <param name="researchDetailsType"></param>
    /// <returns></returns>
    public static List<ResearchInfoBean> GetResearchInfoByType(int researchType, int researchDetailsType)
    {
        List<ResearchInfoBean> listData = new List<ResearchInfoBean>();
        List <ResearchInfoBean> listTypeData = GetResearchInfoByType(researchType);
        foreach (var itemData in listTypeData)
        {
            if (researchDetailsType == itemData.type_details)
            {
                listData.Add(itemData);
            }
        }
        return listData;
    }
}