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
    //Ƶ��
    public float frequency;
    //���
    public float amplitude;
    //��С�߶�
    public int min_height;
    //��С
    public float scale;
    //��������
    public int iterate_number;
    //ˮƽ��߶�
    public int water_height;

    public BiomeTypeEnum GetBiomeType()
    {
        return (BiomeTypeEnum)id;
    }
    
    /// <summary>
    /// ��ȡˮƽ��߶�
    /// </summary>
    /// <returns></returns>
    public int GetWaterPlaneHeight()
    {
        return water_height;
    }
}