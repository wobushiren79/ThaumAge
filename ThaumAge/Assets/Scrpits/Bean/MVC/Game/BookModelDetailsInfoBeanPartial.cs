using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class BookModelDetailsInfoBean
{
    public string title
    {
        get
        {
            return GetBaseText("title");
        }
    }

    public string content
    {
        get
        {
            return GetBaseText("content");
        }
    }

    protected List<ItemsArrayBean> listUnlockItem;

    /// <summary>
    /// 获取UI点位
    /// </summary>
    /// <returns></returns>
    public Vector2 GetMapPosition()
    {
        float[] position = map_position.SplitForArrayFloat(',');
        return new Vector2(position[0], position[1]);
    }

    /// <summary>
    /// 获取前置条件ID
    /// </summary>
    /// <returns></returns>
    public int[] GetPreShowIds()
    {
        return show_pre.SplitForArrayInt(',');
    }

    /// <summary>
    /// 检测是否满足展示前置条件
    /// </summary>
    /// <returns></returns>
    public bool CheckPreShow()
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        int[] showPreIds = GetPreShowIds();
        for (int i = 0; i < showPreIds.Length; i++)
        {
            bool isUnlock = userData.userAchievement.CheckUnlockBookModelDetails(showPreIds[i]);
            if (isUnlock == false)
                return isUnlock;
        }
        return true;
    }

    /// <summary>
    /// 获取解锁道具（持有）
    /// </summary>
    /// <returns></returns>
    public List<ItemsArrayBean> GetUnlockItems()
    {
        if (listUnlockItem == null)
            listUnlockItem = new List<ItemsArrayBean>();
        if (!unlock_items.IsNull() && listUnlockItem.Count == 0)
        {
            listUnlockItem = ItemsArrayBean.GetListItemsArrayBean(unlock_items);
        }
        return listUnlockItem;
    }
}

public partial class BookModelDetailsInfoCfg
{
    //流程书详情(按模块)
    protected static Dictionary<int, List<BookModelDetailsInfoBean>> dicBookModelDetailsInfo;

    public static List<BookModelDetailsInfoBean> GetItemDataByModel(int modelId)
    {
        if (dicBookModelDetailsInfo == null)
        {
            dicBookModelDetailsInfo = new Dictionary<int, List<BookModelDetailsInfoBean>>();
            Dictionary<long, BookModelDetailsInfoBean> dicData = GetAllData();
            foreach (var itemData in dicData)
            {
                var itemDataValue = itemData.Value;
                if (dicBookModelDetailsInfo.TryGetValue(itemDataValue.model_id, out List<BookModelDetailsInfoBean> listItemData))
                {
                    listItemData.Add(itemDataValue);
                }
                else
                {
                    dicBookModelDetailsInfo.Add(itemDataValue.model_id, new List<BookModelDetailsInfoBean>() { itemDataValue });
                }
            }
        }
        if (dicBookModelDetailsInfo.TryGetValue(modelId, out var listData))
        {
            return listData;
        }
        return null;
    }
}