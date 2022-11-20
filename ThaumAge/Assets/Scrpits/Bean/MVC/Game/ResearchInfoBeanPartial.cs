using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class ResearchInfoBeanPartial
{
  
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
}