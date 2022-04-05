using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class UserAchievementBean 
{
    //模块解锁列表
    public List<int> listBookModelDetailsUnlock = new List<int>();

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


}