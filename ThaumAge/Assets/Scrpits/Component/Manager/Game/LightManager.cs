using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager : BaseManager
{
    protected Light _sunLight;
    protected Light _moonLight;

    protected List<UniversalAdditionalLightData> listHDLightData = new List<UniversalAdditionalLightData>();

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

    /// <summary>
    /// 增加HD光照信息
    /// </summary>
    /// <param name="itemData"></param>
    public void AddHDLightData(UniversalAdditionalLightData itemData)
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
    public void RemoveHDLightData(UniversalAdditionalLightData itemData)
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
    public List<UniversalAdditionalLightData> GetAllHDLightData()
    {
        return listHDLightData;
    }
}