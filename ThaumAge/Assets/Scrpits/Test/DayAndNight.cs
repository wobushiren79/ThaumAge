using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class DayAndNight : BaseMonoBehaviour
{
    public HDAdditionalLightData sunLightData;
    public HDAdditionalLightData moonLightData;

    [Header("时间设置")]
    [Range(0, 24f)]
    public float currentTime;
    public float timeSpeed = 1f;

    [Header("当前时间")]
    public string currentTimeString;

    [Header("太阳设置")]
    public Light sunLight;
    [Range(0, 90f)]
    public float sunLatitude = 20f;//纬度
    [Range(-180, 180f)]
    public float sunLongitude = -90f;//经度
    public float sunIntensity = 1f;
    public AnimationCurve sunIntensityMultiplier;    //光照强度
    public AnimationCurve sunLightTemperatureCurve;    //光照温度

    [Header("月亮设置")]
    public Light moonLight;
    [Range(0, 90f)]
    public float moonLatitude = 40f;//纬度
    [Range(-180, 180f)]
    public float moonLongitude = 90f;//经度
    public float moonIntensity = 1f;
    public AnimationCurve moonIntensityMultiplier;    //光照强度
    public AnimationCurve moonLightTemperatureCurve;    //光照温度

    public bool isDay = true;

    void Start()
    {
        UpdateTimeText();
        CheckShadowStatus();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime * timeSpeed;
        if (currentTime >= 24)
        {
            currentTime = 0;
        }
        UpdateTimeText();
        UpdateLight();
        CheckShadowStatus();
    }

    private void OnValidate()
    {
        UpdateLight();
        CheckShadowStatus();
    }

    /// <summary>
    /// 更新时间显示
    /// </summary>
    public void UpdateTimeText()
    {
        currentTimeString = $"{Mathf.Floor(currentTime).ToString("00")}:{((currentTime % 1) * 60).ToString("00")}";
    }

    /// <summary>
    /// 更新光照
    /// </summary>
    public void UpdateLight()
    {
        //设置太阳和月亮的角度
        float sunRotation = currentTime / 24f * 360;
        sunLight.transform.localRotation = (Quaternion.Euler(sunLatitude - 90, sunLongitude, 0) * Quaternion.Euler(0, sunRotation, 0));
        moonLight.transform.localRotation = (Quaternion.Euler(90 - moonLatitude, moonLongitude, 0) * Quaternion.Euler(0, sunRotation, 0));

        float normalizedTime = currentTime / 24f;

        //设置光照强度变化
        float sunIntensityCurve = sunIntensityMultiplier.Evaluate(normalizedTime);
        float moonIntensityCurve = moonIntensityMultiplier.Evaluate(normalizedTime);
        if (sunLightData == null)
        {
            sunLightData = sunLight.GetComponent<HDAdditionalLightData>();
        }
        if (moonLightData == null)
        {
            moonLightData = moonLight.GetComponent<HDAdditionalLightData>();
        }

        if (sunLightData != null)
        {
            sunLightData.intensity = sunIntensityCurve * sunIntensity;
        }
        if (moonLightData != null)
        {
            moonLightData.intensity = moonIntensityCurve * moonIntensity;
        }

        if (sunLight != null)
        {
            float temperatureMultiplier = sunLightTemperatureCurve.Evaluate(normalizedTime);
            sunLight.colorTemperature = temperatureMultiplier * 10000f;
        }
        if (moonLight != null)
        {
            float temperatureMultiplier = moonLightTemperatureCurve.Evaluate(normalizedTime);
            moonLight.colorTemperature = temperatureMultiplier * 10000f;
        }

    }


    /// <summary>
    /// 检测阴影状态
    /// </summary>
    public void CheckShadowStatus()
    {
        //设置阴影
        if (currentTime >= 6f && currentTime <= 18f)
        {
            if (sunLightData != null)
                sunLightData.EnableShadows(true);
            if (moonLightData != null)
                moonLightData.EnableShadows(false);
            isDay = true;
        }
        else
        {
            if (sunLightData != null)
                sunLightData.EnableShadows(false);
            if (moonLightData != null)
                moonLightData.EnableShadows(true);
            isDay = false;
        }

        //设置光照
        if (sunLight != null)
        {
            if (currentTime >= 5.7f && currentTime <= 18.3f)
            {
                sunLight.gameObject.SetActive(true);
            }
            else
            {
                sunLight.gameObject.SetActive(false);
            }
        }


        //设置光照
        if (moonLight != null)
        {
            if (currentTime >= 6.3f && currentTime <= 17.7f)
            {
                moonLight.gameObject.SetActive(false);
            }
            else
            {
                moonLight.gameObject.SetActive(true);
            }
        }

    }
}
