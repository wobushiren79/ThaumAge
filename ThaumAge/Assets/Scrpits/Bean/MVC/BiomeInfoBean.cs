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
    //频率
    public float frequency;
    //振幅
    public float amplitude;
    //最小高度
    public int minHeight;
    //大小
    public float scale;

    public BiomeTypeEnum GetBiomeType()
    {
        return (BiomeTypeEnum)id;
    }
}