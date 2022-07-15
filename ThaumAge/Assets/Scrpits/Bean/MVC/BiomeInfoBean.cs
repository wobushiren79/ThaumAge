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
    public float frequency0;
    public float frequency1;
    public float frequency2;

    //振幅
    public float amplitude0;
    public float amplitude1;
    public float amplitude2;

    //大小
    public float scale0;
    public float scale1;
    public float scale2;

    //迭代次数
    public int iterate_number0;
    public int iterate_number1;
    public int iterate_number2;

    //最小高度
    public int min_height;
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