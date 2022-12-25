using UnityEditor;
using UnityEngine;

public partial class MagicInstrumentInfoCfg
{

    /// <summary>
    /// 通过道具ID获取
    /// </summary>
    /// <param name="itemId"></param>
    /// <returns></returns>
    public static MagicInstrumentInfoBean GetItemDataByItemId(long itemId)
    {
        var allData = GetAllData();
        foreach (var itemData in allData)
        {
            if (itemData.Value.item_id == itemId)
            {
                return itemData.Value;
            }
        }
        return null;
    }

}