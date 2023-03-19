using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class LightManager : BaseManager
{
    protected Light _sunLight;
    protected HDAdditionalLightData _sunLightHD;
    protected Light _moonLight;
    protected HDAdditionalLightData _moonLightHD;

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
                _sunLight = FindWithTag<Light>(TagInfo.Tag_Sun);
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

    public Light moonLight
    {
        get
        {
            if (_moonLight == null)
            {
                _moonLight = FindWithTag<Light>(TagInfo.Tag_Moon);

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