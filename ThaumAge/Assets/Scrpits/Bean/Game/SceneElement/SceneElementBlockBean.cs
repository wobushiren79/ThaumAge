using UnityEditor;
using UnityEngine;
using System;

[Serializable]
public class SceneElementBlockBean 
{
    //元素类型
    public int elementalType;
    //位置
    public Vector3Int position;
    //生效时间（默认3秒）
    public float timeForWork = 10;

    public ElementalTypeEnum GetElementalType()
    {
        return (ElementalTypeEnum)elementalType;
    }
}