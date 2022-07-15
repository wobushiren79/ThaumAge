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
    public float frequency0;
    public float frequency1;
    public float frequency2;

    //���
    public float amplitude0;
    public float amplitude1;
    public float amplitude2;

    //��С
    public float scale0;
    public float scale1;
    public float scale2;

    //��������
    public int iterate_number0;
    public int iterate_number1;
    public int iterate_number2;

    //��С�߶�
    public int min_height;
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