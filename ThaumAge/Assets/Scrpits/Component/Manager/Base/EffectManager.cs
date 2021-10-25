using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EffectManager : BaseManager
{
    public Dictionary<string, GameObject> dicEffectModel = new Dictionary<string, GameObject>();

    /// <summary>
    /// 创建粒子
    /// </summary>
    /// <param name="objContainer"></param>
    /// <param name="effectData"></param>
    /// <param name="completeAction"></param>
    public void CreateEffect(GameObject objContainer, EffectBean effectData, Action<GameObject> completeAction)
    {
        GetModelForAddressables(dicEffectModel, $"Assets/Prefabs/Effects/{effectData.effectName}.prefab", (obj) =>
        {
            GameObject objEffects = Instantiate(objContainer, obj);
            objEffects.transform.position = effectData.effectPosition;
            completeAction?.Invoke(objEffects);
        });
    }
}