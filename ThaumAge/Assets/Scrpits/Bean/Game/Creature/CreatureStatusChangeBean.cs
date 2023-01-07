using UnityEditor;
using UnityEngine;
using System;

[Serializable]
public class CreatureStatusChangeBean
{
    public int changType;//类型
    public float time;//持续时间
    public float changeValue;//修改的值

    public CreatureStatusChangeBean(CreatureStatusChangeTypeEnum creatureStatusChangeType, float time, float changValue)
    {
        this.changType = (int)creatureStatusChangeType;
        this.time = time;
        this.changeValue = changValue;
    }

    public CreatureStatusChangeTypeEnum GetChangeType()
    {
        return (CreatureStatusChangeTypeEnum)changType;
    }
}