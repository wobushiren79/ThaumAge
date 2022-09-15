using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightHDBase : BaseMonoBehaviour
{
    protected UniversalAdditionalLightData hdAdditionalLightData;
    protected void Awake()
    {
        hdAdditionalLightData = GetComponent<UniversalAdditionalLightData>();
        if (hdAdditionalLightData != null)
        {
            GameConfigBean gameConfig =  GameDataHandler.Instance.manager.GetGameConfig();
            //设置阴影质量
            //hdAdditionalLightData.SetShadowResolutionLevel(gameConfig.shadowResolutionLevel);
            //hdAdditionalLightData.slopeBias = 0;
            //hdAdditionalLightData.normalBias = 5;
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