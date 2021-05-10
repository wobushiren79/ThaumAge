using UnityEditor;
using UnityEngine;

public class LightManager : BaseManager
{
    protected Light _mainLight;

    public Color mainLightTopStart;
    public Color mainLightMiddleStart;
    public Color mainLightBottomStart;

    public Color mainLightTopEnd;
    public Color mainLightMiddleEnd;
    public Color mainLightBottomEnd;

    public void InitData()
    {
        mainLightTopStart = new Color(0f, 0f, 1f, 1f);
        mainLightMiddleStart = new Color(0.3f, 0.7f, 1f, 1f);
        mainLightBottomStart = new Color(1f, 1f, 1f, 1f);

        mainLightTopEnd = new Color(0f, 0f, 0.047f, 1f);
        mainLightMiddleEnd = new Color(0.02f, 0.07f, 0.093f, 1f);
        mainLightBottomEnd = new Color(0.1f, 0.1f, 0.1f, 1f);
    }

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
    /// 设置天空盒颜色
    /// </summary>
    /// <param name="color"></param>
    public void SetSkyBoxColor(Color colorTop, Color colorMiddle, Color colorBottom)
    {
        VolumeHandler.Instance.manager.SetGradientSkyColor(colorTop, colorMiddle, colorBottom);
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