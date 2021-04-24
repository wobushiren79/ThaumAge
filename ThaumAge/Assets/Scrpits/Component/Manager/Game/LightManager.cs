using UnityEditor;
using UnityEngine;

public class LightManager : BaseManager
{
    protected Light _mainLight;
    protected Material matSkybox;

    public Light mainLight
    {
        get
        {
            if (_mainLight == null)
            {
                _mainLight = FindWithTag<Light>(TagInfo.Tag_MainLight);

            }
            return _mainLight;
        }
    }

    /// <summary>
    /// 天空盒颜色ID
    /// </summary>
    protected int namdIdSkyBoxExposure;
    protected int namdIdSkyBoxColor;

    private void Awake()
    {
        matSkybox = RenderSettings.skybox;

        int propertyIndexSkyBoxExposure = matSkybox.shader.FindPropertyIndex("_Exposure");
        namdIdSkyBoxExposure = matSkybox.shader.GetPropertyNameId(propertyIndexSkyBoxExposure);

        int propertyIndexSkyBoxColor  = matSkybox.shader.FindPropertyIndex("_Tint");
        namdIdSkyBoxColor = matSkybox.shader.GetPropertyNameId(propertyIndexSkyBoxColor);
    }

    /// <summary>
    /// 设置天空和曝光
    /// </summary>
    /// <param name="data"></param>
    public void SetSkyBoxExposure(float data)
    {
        matSkybox.SetFloat(namdIdSkyBoxExposure, data);
    }

    /// <summary>
    /// 设置天空盒颜色
    /// </summary>
    /// <param name="color"></param>
    public void SetSkyBoxColor(Color color)
    {
        matSkybox.SetColor(namdIdSkyBoxColor, color);
    }

    /// <summary>
    /// 设置主光照颜色
    /// </summary>
    public void SetMainLightColor(Color color)
    {
        mainLight.color = color;
    }

    /// <summary>
    /// 设置环境光
    /// </summary>
    /// <param name="color"></param>
    public void SetAmbientLight(Color color)
    {
        RenderSettings.ambientLight = color;
    }

}