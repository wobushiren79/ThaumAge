using System;
using System.Collections.Generic;
public partial class GolemPressInfoBean
{
}
public partial class GolemPressInfoCfg
{
    public static Dictionary<GolemPressTypeEnum, List<GolemPressInfoBean>> dicTypePressInfo;

    /// <summary>
    /// 根据类型 获取数据
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static List<GolemPressInfoBean> GetGolemPressInfoByType(GolemPressTypeEnum type)
    {
        if (dicTypePressInfo == null)
        {
            dicTypePressInfo = new Dictionary<GolemPressTypeEnum, List<GolemPressInfoBean>>();
            var allData = GetAllData();
            foreach (var itemData in allData)
            {
                var itemInfo = itemData.Value;
                GolemPressTypeEnum golemPressType = (GolemPressTypeEnum)itemInfo.golem_part_type;
                if (dicTypePressInfo.TryGetValue(golemPressType, out List<GolemPressInfoBean> listTempData))
                {
                    listTempData.Add(itemInfo);
                }
                else
                {
                    dicTypePressInfo.Add(golemPressType,new List<GolemPressInfoBean>() { itemInfo });
                }
            }
        }
        List<GolemPressInfoBean> listData = null;
        if (dicTypePressInfo.TryGetValue(type, out listData))
        {
            return listData;
        }
        return listData;
    }
}
