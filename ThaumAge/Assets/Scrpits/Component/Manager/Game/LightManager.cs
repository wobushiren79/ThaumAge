using UnityEditor;
using UnityEngine;

public class LightManager : BaseManager
{
    protected Light _mainLight;
    protected LensFlare _mainLensFlare;

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

    public LensFlare mainLensFlare
    {
        get
        {
            if (_mainLensFlare == null)
            {
                _mainLensFlare = FindWithTag<LensFlare>(TagInfo.Tag_MainLight);

            }
            return _mainLensFlare;
        }
    }

    private void Awake()
    {
        matSkybox = RenderSettings.skybox;
    }

    /// <summary>
    /// 设置天空和曝光
    /// </summary>
    /// <param name="data"></param>
    public void SetSkyBoxExposure(float data)
    {
       // matSkybox.SetFloat(namdIdSkyBoxExposure, data);
    }

    /// <summary>
    /// 设置天空盒颜色
    /// </summary>
    /// <param name="color"></param>
    public void SetSkyBoxColor(Color color)
    {
        //matSkybox.SetColor(namdIdSkyBoxColor, color);
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

    /// <summary>
    /// 设置主光照光晕
    /// </summary>
    /// <param name="isShow"></param>
    public void SetMainLensFlare(bool isShow)
    {
        if (isShow)
        {
            mainLensFlare.enabled = true;
        }
        else
        {
            mainLensFlare.enabled = false;
        }
    }

}