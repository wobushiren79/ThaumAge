using UnityEditor;
using UnityEngine;

public class LightForCelestial : LightHDBase
{
    [Range(0, 90f)]
    public float latitude = 20f;//纬度
    [Range(-180, 180f)]
    public float longitude = -90f;//经度
    public float intensity = 1f;
    public AnimationCurve intensityMultiplier;    //光照强度
    public AnimationCurve lightTemperatureCurve;    //光照温度
}