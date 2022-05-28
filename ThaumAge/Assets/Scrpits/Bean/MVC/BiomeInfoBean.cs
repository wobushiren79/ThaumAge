/*
* FileName: BiomeInfo 
* Author: AppleCoffee 
* CreateTime: 2021-03-18-17:53:13 
*/

using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class BiomeInfoBean : BaseBean
{
    public int link_id;
    //频率
    public float frequency;
    //振幅
    public float amplitude;
    //最小高度
    public int min_height;
    //大小
    public float scale;
    //迭代次数
    public int iterate_number;
    //水平面高度
    public int water_height;

    public BiomeTypeEnum GetBiomeType()
    {
        return (BiomeTypeEnum)id;
    }
    
    /// <summary>
    /// 获取水平面高度
    /// </summary>
    /// <returns></returns>
    public int GetWaterPlaneHeight()
    {
        return water_height;
    }
}