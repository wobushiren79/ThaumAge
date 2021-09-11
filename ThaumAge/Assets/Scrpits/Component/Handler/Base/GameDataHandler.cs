﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

public class GameDataHandler : BaseHandler<GameDataHandler, GameDataManager>
{
    /// <summary>
    /// 获取基础信息
    /// </summary>
    /// <param name="baseInfoId"></param>
    /// <returns></returns>
    public string GetBaseInfoStr(long baseInfoId)
    {
        BaseInfoBean baseInfo = manager.controllerForBase.GetBaseData(baseInfoId);
        return baseInfo.content;
    }
    public int GetBaseInfoInt(long baseInfoId)
    {
        BaseInfoBean baseInfo = manager.controllerForBase.GetBaseData(baseInfoId);
        return int.Parse(baseInfo.content);
    }
    public long GetBaseInfoLong(long baseInfoId)
    {
        BaseInfoBean baseInfo = manager.controllerForBase.GetBaseData(baseInfoId);
        return long.Parse(baseInfo.content);
    }
    public float GetBaseInfoFloat(long baseInfoId)
    {
        BaseInfoBean baseInfo = manager.controllerForBase.GetBaseData(baseInfoId);
        return float.Parse(baseInfo.content);
    }
    public List<long> GetBaseInfoListLong(long baseInfoId)
    {
        string dataStr = GetBaseInfoStr(baseInfoId);
        long[] arrayData= StringUtil.SplitBySubstringForArrayLong(dataStr,',');
        return arrayData.ToList();
    }
}