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
    //解锁过的研究
    public List<int> listResearchUnlock = new List<int>();
    public List<ProgressBean> listResearchProgress = new List<ProgressBean>();

    /// <summary>
    /// 检测是否解锁对应研究
    /// </summary>
    public bool CheckUnlockResearch(int researchId)
    {
        if (listResearchUnlock.Contains(researchId))
        {
            return true;
        }
        return false;
    }

    public bool CheckUnlockResearch(int[] arrayResearchId)
    {
        foreach (var itemData in arrayResearchId)
        {
            bool isUnlock = CheckUnlockResearch(itemData);
            if (!isUnlock)
                return false;
        }
        return true;
    }

    /// <summary>
    /// 解锁进度
    /// </summary>
    public void UnlockResearch(int researchId)
    {
        if (!listResearchUnlock.Contains(researchId))
        {
            listResearchUnlock.Add(researchId);
        }
    }

    /// <summary>
    /// 获取研究进度
    /// </summary>
    public ProgressBean GetResearchProgressData(int researchId)
    {
        foreach (var itemData in listResearchProgress)
        {
            if (itemData.id == researchId)
            {
                return itemData;
            }
        }
        return null;
    }

    /// <summary>
    /// 增加研究
    /// </summary>
    public void AddResearchProgressData(int researchId,int maxProNum)
    {
        foreach (var itemData in listResearchProgress)
        {
            if (itemData.id == researchId)
            {
                return;
            }
        }
        ProgressBean progressData = new ProgressBean();
        progressData.id = researchId;
        progressData.progress = 0;
        progressData.proNumCurrent = 0;
        progressData.proNumMax = maxProNum;
        listResearchProgress.Add(progressData);
    }

    /// <summary>
    /// 解锁研究
    /// </summary>
    public float ResearchProgressChange(int researchId, int addProNum)
    {
        for (int i = 0; i < listResearchProgress.Count; i++)
        {
            var itemData = listResearchProgress[i];
            if (itemData.id == researchId)
            {
                float progress = itemData.AddProgressNum(addProNum);
                if (progress >= 1)
                {
                    UnlockResearch(researchId);
                    listResearchProgress.Remove(itemData);
                }
                return progress;
            }
        }
        return 0;
    }

    /// <summary>
    /// 刷新所有研究进度
    /// </summary>
    /// <param name="addPro"></param>
    public bool ResearchProgressChange(int addProNum)
    {
        if (listResearchProgress.IsNull())
            return false;
        for (int i = 0; i < listResearchProgress.Count; i++)
        {
            var itemData = listResearchProgress[i];
            float progress = itemData.AddProgressNum(addProNum);
            if (progress >= 1)
            {
                UnlockResearch((int)itemData.id);
                listResearchProgress.Remove(itemData);
                //展示弹窗
                var researchInfo = ResearchInfoCfg.GetItemData(itemData.id);
                IconHandler.Instance.manager.GetItemsSpriteByName((researchInfo.icon_key), (sprite) =>
                {
                    string contentToast = string.Format(TextHandler.Instance.GetTextById(30008), researchInfo.GetContent());
                    UIHandler.Instance.ToastHint<ToastView>(sprite, contentToast);
                    //播放音效
                    AudioHandler.Instance.PlaySound(1101);
                });
            }
        }
        return true;
    }

    /// <summary>
    /// 检测是否解锁模块
    /// </summary>
    public bool CheckUnlockBookModel(string unlockDataStr)
    {
        int[] unlockIds = unlockDataStr.SplitForArrayInt('&');
        for (int i = 0; i < unlockIds.Length; i++)
        {
            if (!listBookModelDetailsUnlock.Contains(unlockIds[i]))
            {
                return false;
            }
        }
        return true;
    }

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

    /// <summary>
    /// 检测是否解锁（只要解锁其中一个就行）
    /// </summary>
    /// <param name="itemIds"></param>
    /// <returns></returns>
    public bool CheckUnlockGetItemsForOr(long[] itemIds)
    {
        for (int i = 0; i < itemIds.Length; i++)
        {
            bool isUnlock = CheckUnlockGetItems(itemIds[i]);
            if (isUnlock == true)
                return true;
        }
        return false;
    }

    /// <summary>
    /// 检测是否解锁（需要解锁全部）
    /// </summary>
    /// <param name="itemIds"></param>
    /// <returns></returns>
    public bool CheckUnlockGetItemsForAnd(long[] itemIds)
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