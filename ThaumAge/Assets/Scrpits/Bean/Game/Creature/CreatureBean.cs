using UnityEditor;
using UnityEngine;
using System;

[Serializable]
public class CreatureBean
{
    //生物ID
    public string creatureId;
    //生物详情ID
    public long creatureInfoId;
    //生物类型
    public int creatureType;
    //角色状态
    public CreatureStatusBean creatureStatus;
    //数据
    public string meta;
    //出生坐标位置
    public Vector3 posForBirth;

    public CreatureBean(string creatureId)
    {
        this.creatureId = creatureId;
    }

    public T GetMetaData<T>() where T : CreatureMetaBase
    {
        return JsonUtil.FromJson<T>(meta);
    }

    public void SetMetaData<T>(T data) where T : CreatureMetaBase
    {
        meta = JsonUtil.ToJson(data);
    }

    /// <summary>
    /// 获取生物类型
    /// </summary>
    /// <returns></returns>
    public CreatureTypeEnum GetCreatureType()
    {
        return (CreatureTypeEnum)creatureType;
    }

    public void SetCreatureType(CreatureTypeEnum creatureType) 
    {
        this.creatureType = (int)creatureType;
    }

    /// <summary>
    /// 获取角色状态
    /// </summary>
    public CreatureStatusBean GetCreatureStatus()
    {
        if (creatureStatus == null)
            creatureStatus = new CreatureStatusBean();
        return creatureStatus;
    }
}