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
    //Ƶ��
    public float frequency;
    //���
    public float amplitude;
    //��С�߶�
    public int minHeight;
    //��С
    public float scale;

    public BiomeTypeEnum GetBiomeType()
    {
        return (BiomeTypeEnum)id;
    }
}