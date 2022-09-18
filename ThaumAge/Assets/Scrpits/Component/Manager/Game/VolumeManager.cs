using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class VolumeManager : BaseManager
{
    //Volume的资源路径
    protected static string PathResVolume = "Assets/Prefabs/Render/Volume.prefab";
    //颜色天空
    protected GradientSky _gradientSky;
    public GradientSky gradientSky
    {
        get
        {
            if (_gradientSky == null)
            {
                volumeProfile.TryGet(out _gradientSky);
            }
            return _gradientSky;
        }
    }

    //物理天空
    protected PhysicallyBasedSky _physicallyBasedSky;
    public PhysicallyBasedSky physicallyBasedSky
    {
        get
        {
            if (_physicallyBasedSky == null)
            {
                volumeProfile.TryGet(out _physicallyBasedSky);
            }
            return _physicallyBasedSky;
        }
    }

    //阴影设置
    protected HDShadowSettings _shadowSettings;
    public HDShadowSettings shadowSettings
    {
        get
        {
            if (_shadowSettings == null)
            {
                volumeProfile.TryGet(out _shadowSettings);
            }
            return _shadowSettings;
        }
    }

    //远近模糊
    protected DepthOfField _depthOfField;
    public DepthOfField depthOfField
    {
        get
        {
            if (_depthOfField == null)
            {
                volumeProfile.TryGet(out _depthOfField);
            }
            return _depthOfField;
        }
    }

    //颜色调整
    protected ColorAdjustments _colorAdjustments;
    public ColorAdjustments colorAdjustments
    {
        get
        {
            if (_colorAdjustments == null)
            {
                volumeProfile.TryGet(out _colorAdjustments);
            }
            return _colorAdjustments;
        }
    }

    //雾
    protected Fog _fog;
    public Fog fog
    {
        get
        {
            if (_fog == null)
            {
                volumeProfile.TryGet(out _fog);
            }
            return _fog;
        }
    }

    //基础设置
    protected Volume _volume;
    public Volume volume
    {
        get
        {
            if (_volume == null)
            {
                _volume = FindWithTag<Volume>(TagInfo.Tag_Volume);
                if (_volume == null)
                {
                    GameObject objVolumeModel = LoadAddressablesUtil.LoadAssetSync<GameObject>(PathResVolume);
                    GameObject objVolume = Instantiate(gameObject, objVolumeModel);
                    objVolume.transform.localPosition = Vector3.zero;
                    _volume = objVolume.GetComponent<Volume>();
                }
            }
            return _volume;
        }
    }


    protected VolumeProfile _volumeProfile;
    public VolumeProfile volumeProfile
    {
        get
        {
            if (_volumeProfile == null)
            {
                _volumeProfile = volume.profile;
            }
            return _volumeProfile;
        }
    }

    /// <summary>
    /// 设置阴影距离
    /// </summary>
    /// <param name="dis"></param>
    public void SetShadowsDistance(float dis)
    {
        shadowSettings.maxShadowDistance.overrideState = true;
        shadowSettings.maxShadowDistance.value = dis;
    }

    /// <summary>
    /// 设置天空颜色
    /// </summary>
    /// <param name="colorTop"></param>
    /// <param name="colorMiddle"></param>
    /// <param name="colorBottom"></param>
    public void SetGradientSkyColor(Color colorTop, Color colorMiddle, Color colorBottom)
    {
        gradientSky.top.overrideState = true;
        gradientSky.top.value = colorTop;
        gradientSky.middle.overrideState = true;
        gradientSky.middle.value = colorMiddle;
        gradientSky.bottom.overrideState = true;
        gradientSky.bottom.value = colorBottom;
    }

    /// <summary>
    /// 设置天空颜色
    /// </summary>
    /// <param name="colorZenith"></param>
    /// <param name="colorHorizon"></param>
    /// <param name="colorGround"></param>
    public void SetPhysicallyBasedSkyColor(Color colorZenith, Color colorHorizon, Color colorGround)
    {
        physicallyBasedSky.zenithTint.overrideState = true;
        physicallyBasedSky.zenithTint.value = colorZenith;

        physicallyBasedSky.horizonTint.overrideState = true;
        physicallyBasedSky.horizonTint.value = colorHorizon;

        physicallyBasedSky.groundTint.overrideState = true;
        physicallyBasedSky.groundTint.value = colorGround;
    }

    /// <summary>
    /// 设置远景模糊
    /// </summary>
    public void SetDepthOfField(float nearStart,float nearEnd,float farStart, float farEnd)
    {
        depthOfField.nearFocusStart.overrideState = true;
        depthOfField.nearFocusStart.value = nearStart;
        depthOfField.nearFocusEnd.overrideState = true;
        depthOfField.nearFocusEnd.value = nearEnd;

        depthOfField.farFocusStart.overrideState = true;
        depthOfField.farFocusStart.value = farStart;
        depthOfField.farFocusEnd.overrideState = true;
        depthOfField.farFocusEnd.value = farEnd;
    }

    /// <summary>
    /// 颜色调整
    /// </summary>
    /// <param name="postExposure">曝光</param>
    /// <param name="contrast">反差</param>
    /// <param name="colorFilter">彩色滤光片</param>
    /// <param name="hueShift">色相偏移</param>
    /// <param name="saturation">饱和</param>
    public void SetColorAdjustments(Color colorFilter, float postExposure,float contrast,float hueShift,float saturation)
    {
        colorAdjustments.postExposure.overrideState = true;
        colorAdjustments.postExposure.value = postExposure;

        colorAdjustments.contrast.overrideState = true;
        colorAdjustments.contrast.value = contrast;

        colorAdjustments.colorFilter.overrideState = true;
        colorAdjustments.colorFilter.value = colorFilter;

        colorAdjustments.hueShift.overrideState = true;
        colorAdjustments.hueShift.value = postExposure;

        colorAdjustments.saturation.overrideState = true;
        colorAdjustments.saturation.value = postExposure;
    }

    /// <summary>
    /// 设置雾
    /// </summary>
    /// <param name="enabled"></param>
    public void SetFog(bool enabled)
    {
        fog.enabled.overrideState = true;
        fog.enabled.value = enabled;
    }
}