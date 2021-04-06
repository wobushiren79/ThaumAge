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
    public long link_id;
    //频率
    public float frequency;
    //振幅
    public float amplitude;
    //最小高度
    public int minHeight;
    //大小
    public float scale;
    //名字
    public string name;

    public BiomeTypeEnum GetBiomeType()
    {
        return (BiomeTypeEnum)id;
    }
}