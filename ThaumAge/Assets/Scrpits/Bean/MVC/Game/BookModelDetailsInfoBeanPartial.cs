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

    protected List<ItemsBean> listUnlockItem;

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
    public List<ItemsBean> GetUnlockItems()
    {
        if (listUnlockItem == null)
            listUnlockItem = new List<ItemsBean>();
        if (!unlock_items.IsNull() && listUnlockItem.Count == 0)
        {
            string[] unlockItemsStr = unlock_items.Split('&');

            foreach (var itemUnlockStr in unlockItemsStr)
            {
                string[] unlockItemsDetailsStr = itemUnlockStr.Split(':');
                int itemId = int.Parse(unlockItemsDetailsStr[0]);
                int itemNumber = 1;
                if (unlockItemsDetailsStr.Length > 1)
                {
                    itemNumber = int.Parse(unlockItemsDetailsStr[1]);
                }
                listUnlockItem.Add(new ItemsBean(itemId, itemNumber));
            }
        }
        return listUnlockItem;
    }
}