using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class DayAndNight : BaseMonoBehaviour
{
    public HDAdditionalLightData sunLightData;
    public HDAdditionalLightData moonLightData;

    [Header("ʱ������")]
    [Range(0, 24f)]
    public float currentTime;
    public float timeSpeed = 1f;

    [Header("��ǰʱ��")]
    public string currentTimeString;

    [Header("̫������")]
    public Light sunLight;
    [Range(0, 90f)]
    public float sunLatitude = 20f;//γ��
    [Range(-180, 180f)]
    public float sunLongitude = -90f;//����
    public float sunIntensity = 1f;
    public AnimationCurve sunIntensityMultiplier;    //����ǿ��
    public AnimationCurve sunLightTemperatureCurve;    //�����¶�

    [Header("��������")]
    public Light moonLight;
    [Range(0, 90f)]
    public float moonLatitude = 40f;//γ��
    [Range(-180, 180f)]
    public float moonLongitude = 90f;//����
    public float moonIntensity = 1f;
    public AnimationCurve moonIntensityMultiplier;    //����ǿ��
    public AnimationCurve moonLightTemperatureCurve;    //�����¶�

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
    /// ����ʱ����ʾ
    /// </summary>
    public void UpdateTimeText()
    {
        currentTimeString = $"{Mathf.Floor(currentTime).ToString("00")}:{((currentTime % 1) * 60).ToString("00")}";
    }

    /// <summary>
    /// ���¹���
    /// </summary>
    public void UpdateLight()
    {
        //����̫���������ĽǶ�
        float sunRotation = currentTime / 24f * 360;
        sunLight.transform.localRotation = (Quaternion.Euler(sunLatitude - 90, sunLongitude, 0) * Quaternion.Euler(0, sunRotation, 0));
        moonLight.transform.localRotation = (Quaternion.Euler(90 - moonLatitude, moonLongitude, 0) * Quaternion.Euler(0, sunRotation, 0));

        float normalizedTime = currentTime / 24f;

        //���ù���ǿ�ȱ仯
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
    /// �����Ӱ״̬
    /// </summary>
    public void CheckShadowStatus()
    {
        //������Ӱ
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

        //���ù���
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


        //���ù���
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
