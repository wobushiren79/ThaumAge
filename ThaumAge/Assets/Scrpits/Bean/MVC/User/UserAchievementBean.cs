using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class UserAchievementBean
{
    //模块解锁列表
    public List<int> listBookModelDetailsUnlock = new List<int>();
    //获得过的道具
    public List<long> listGetItemsUnlock = new List<long>();

    /// <summary>
    /// 检测是否解锁模块
    /// </summary>
    public bool CheckUnlockBookModelDetails(int modelDetailsId)
    {
        if (listBookModelDetailsUnlock.Contains(modelDetailsId))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 解锁模块
    /// </summary>
    /// <param name="modelDetailsId"></param>
    public void UnlockBookModelDetails(int modelDetailsId)
    {
        if (!listBookModelDetailsUnlock.Contains(modelDetailsId))
        {
            listBookModelDetailsUnlock.Add(modelDetailsId);
        }
    }

    /// <summary>
    /// 解锁获得过的道具
    /// </summary>
    public void UnlockGetItems(long itemId)
    {
        if (!listGetItemsUnlock.Contains(itemId))
            listGetItemsUnlock.Add(itemId);
    }

    /// <summary>
    /// 检测是否解锁过获得过的道具
    /// </summary>
    /// <param name="itemId"></param>
    /// <returns></returns>
    public bool CheckUnlockGetItems(long itemId)
    {
        if (listGetItemsUnlock.Contains(itemId))
        {
            return true;
        }
        return false;
    }

    public bool CheckUnlockGetItems(long[] itemIds)
    {
        for (int i = 0; i < itemIds.Length; i++)
        {
            bool isUnlock = CheckUnlockGetItems(itemIds[i]);
            if (isUnlock == false)
                return false;
        }
        return true;
    }
}