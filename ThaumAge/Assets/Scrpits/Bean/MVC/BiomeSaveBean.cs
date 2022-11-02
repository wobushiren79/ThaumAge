/*
* FileName: BiomeSave 
* Author: AppleCoffee 
* CreateTime: 2022-11-01-14:57:35 
*/

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class BiomeSaveBean : BaseBean
{
    public string userId;
    public int worldType;
    public Vector4[] arrayBiomeData = new Vector4[0];

    /// <summary>
    /// 设置数据
    /// </summary>
    public bool SetData(int x, int z, int value)
    {
        for (int i = 0; i < arrayBiomeData.Length; i++)
        {
            Vector4 itemData = arrayBiomeData[i];
            if (x == itemData.x && z == itemData.y)
            {
                return false;
            }
        }
        Vector4[] arrayNewData = new Vector4[arrayBiomeData.Length + 1];
        if (arrayBiomeData.Length != 0)
            Array.Copy(arrayBiomeData, 0, arrayNewData, 0, arrayBiomeData.Length);
        arrayBiomeData = arrayNewData;
        arrayBiomeData[arrayBiomeData.Length - 1] = new Vector4(x, z, value, 0);
        return true;
    }


    public WorldTypeEnum GetWorldType()
    {
        return (WorldTypeEnum)worldType;
    }

    /// <summary>
    /// 获取最近的3个点
    /// </summary>
    /// <param name="chunk"></param>
    /// <returns></returns>
    public Vector4[] GetCloseBiomeSave(Chunk chunk)
    {
        float dis1 = float.MaxValue;
        float dis2 = float.MaxValue;
        float dis3 = float.MaxValue;

        Vector4[] arrayData = new Vector4[]
        {
            new Vector4(0,0,-1,0),
            new Vector4(0,0,-1,0),
            new Vector4(0,0,-1,0),
        };
        for (int i = 0; i < arrayBiomeData.Length; i++)
        {
            Vector4 itemData = arrayBiomeData[i];
            float dis = Vector3Int.Distance(chunk.chunkData.positionForWorld, new Vector3Int((int)itemData.x, 0, (int)itemData.y));
            if (dis <= dis1)
            {
                dis1 = dis;
                arrayData[0] = itemData;
            }
            if (dis <= dis2 && dis > dis1)
            {
                dis2 = dis;
                arrayData[1] = itemData;
            }
            if (dis <= dis3 && dis > dis2)
            {
                dis3 = dis;
                arrayData[2] = itemData;
            }
        }
        return arrayData;
    }
}