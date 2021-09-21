using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class VolumeManager : BaseManager
{
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

    //基础设置
    protected Volume _volume;
    public Volume volume
    {
        get
        {
            if (_volume == null)
            {
                _volume = FindWithTag<Volume>(TagInfo.Tag_Volume);

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
}