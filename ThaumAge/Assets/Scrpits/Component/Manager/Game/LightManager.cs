using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class LightManager : BaseManager
{
    protected Light _sunLight;
    protected HDAdditionalLightData _sunLightHD;
    protected LightForCelestial _sunLightData;

    protected Light _moonLight;
    protected HDAdditionalLightData _moonLightHD;
    protected LightForCelestial _moonLightData;

    protected List<HDAdditionalLightData> listHDLightData = new List<HDAdditionalLightData>();

    protected void Awake()
    {

    }

    public Light sunLight
    {
        get
        {
            if (_sunLight == null)
            {
                _sunLight = Find<Light>("Element/Light/SunLight");
            }
            return _sunLight;
        }
    }
    public HDAdditionalLightData sunLightHD
    {
        get
        {
            if (_sunLightHD == null)
            {
                _sunLightHD = sunLight.GetComponent<HDAdditionalLightData>();
            }
            return _sunLightHD;
        }
    }
    public LightForCelestial sunLightData
    {
        get
        {
            if (_sunLightData == null)
            {
                _sunLightData = sunLight.GetComponent<LightForCelestial>();
            }
            return _sunLightData;
        }
    }

    public Light moonLight
    {
        get
        {
            if (_moonLight == null)
            {
                _moonLight = Find<Light>("Element/Light/MoonLight");

            }
            return _moonLight;
        }
    }
    public HDAdditionalLightData moonLightHD
    {
        get
        {
            if (_moonLightHD == null)
            {
                _moonLightHD = moonLight.GetComponent<HDAdditionalLightData>();
            }
            return _moonLightHD;
        }
    }
    public LightForCelestial moonLightData
    {
        get
        {
            if (_moonLightData == null)
            {
                _moonLightData = moonLight.GetComponent<LightForCelestial>();
            }
            return _moonLightData;
        }
    }

    /// <summary>
    /// 增加HD光照信息
    /// </summary>
    /// <param name="itemData"></param>
    public void AddHDLightData(HDAdditionalLightData itemData)
    {
        if (!listHDLightData.Contains(itemData))
        {
            listHDLightData.Add(itemData);
        }
    }

    /// <summary>
    /// 移除光照信息
    /// </summary>
    /// <param name="itemData"></param>
    public void RemoveHDLightData(HDAdditionalLightData itemData)
    {
        if (listHDLightData.Contains(itemData))
        {
            listHDLightData.Remove(itemData);
        }
    }

    /// <summary>
    /// 获取所有HD光照数据
    /// </summary>
    /// <returns></returns>
    public List<HDAdditionalLightData> GetAllHDLightData()
    {
        return listHDLightData;
    }
}