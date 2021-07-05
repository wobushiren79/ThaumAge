using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EffectManager : BaseManager
{
    public Dictionary<string, GameObject> dicEffect = new Dictionary<string, GameObject>();


    public void CreateEffect(GameObject objContainer, string name, Action<GameObject> completeAction)
    {
        GameObject objModel = GetModel(dicEffect, "effect/effect", name);
        if (objContainer == null)
            objContainer = gameObject;
        GameObject objEffect = Instantiate(objContainer, objModel);
        completeAction?.Invoke(objEffect);
    }

    public void CreateEffectByAddressables(GameObject objContainer, string name, Action<GameObject> completeAction)
    {
        GetModelForAddressables(dicEffect, name, (objModel) =>
        {
            if (objContainer == null)
                objContainer = gameObject;
            GameObject objEffect = Instantiate(objContainer, objModel);
            completeAction?.Invoke(objEffect);
        });
    }
}