using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class VolumeManager : BaseManager
{
    protected Volume _volume;
    protected VolumeProfile _volumeProfile;

    public GradientSky gradientSky;

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
    /// 设置天空颜色
    /// </summary>
    /// <param name="colorTop"></param>
    /// <param name="colorMiddle"></param>
    /// <param name="colorBottom"></param>
    public void SetGradientSkyColor(Color colorTop, Color colorMiddle, Color colorBottom)
    {
        if (gradientSky == null)
        {
            volumeProfile.TryGet(out gradientSky);
        }
        gradientSky.top.overrideState = true;
        gradientSky.top.value = colorTop;
        gradientSky.middle.overrideState = true;
        gradientSky.middle.value = colorMiddle;
        gradientSky.bottom.overrideState = true;
        gradientSky.bottom.value = colorBottom;
    }
}