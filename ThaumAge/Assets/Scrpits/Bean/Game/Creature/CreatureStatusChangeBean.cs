using UnityEditor;
using UnityEngine;
using System;

[Serializable]
public struct CreatureStatusChangeBean
{
    public int changType;//类型
    public float time;//持续时间
    public float changeValue;//修改的值

    public CreatureStatusChangeBean(AttributeTypeEnum attributeType, float time, float changValue)
    {
        this.changType = (int)attributeType;
        this.time = time;
        this.changeValue = changValue;
    }

    public AttributeTypeEnum GetChangeType()
    {
        return (AttributeTypeEnum)changType;
    }
}