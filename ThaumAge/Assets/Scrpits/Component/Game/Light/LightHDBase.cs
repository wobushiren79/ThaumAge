﻿using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class LightHDBase : BaseMonoBehaviour
{
    protected HDAdditionalLightData hdAdditionalLightData;
    protected void Awake()
    {
        hdAdditionalLightData = GetComponent<HDAdditionalLightData>();
        if (hdAdditionalLightData != null)
        {
            GameConfigBean gameConfig =  GameDataHandler.Instance.manager.GetGameConfig();
            //设置阴影质量
            hdAdditionalLightData.SetShadowResolutionLevel(gameConfig.shadowResolutionLevel);
            hdAdditionalLightData.slopeBias = 0;
            hdAdditionalLightData.normalBias = 5;
            hdAdditionalLightData.preserveCachedShadow = true;
            //添加数据
            LightHandler.Instance.manager.AddHDLightData(hdAdditionalLightData);
        }
    }
    public void OnDestroy()
    {
        if (hdAdditionalLightData != null)
            LightHandler.Instance.manager.RemoveHDLightData(hdAdditionalLightData);
    }
}