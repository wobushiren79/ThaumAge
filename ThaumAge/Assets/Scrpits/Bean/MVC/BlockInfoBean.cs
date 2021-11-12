/*
* FileName: BlockInfo 
* Author: AppleCoffee 
* CreateTime: 2021-03-11-15:39:10 
*/

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class BlockInfoBean : BaseBean
{
    public int link_id;

    public string model_name;

    public int shape;//形状

    public string uv_position;//uv位置

    public float weight;//重量

    public int rotate_state;//是否能旋转 0不能旋转 1能旋转

    public int life;//生命值

    public int plough_state;//耕地状态 0不能耕地 1能耕地

    public int plough_change;//耕地后改变的方块

    public int plant_state;//种植状态 0不能种植 1能种植



    /// <summary>
    /// 获取方块类型
    /// </summary>
    /// <returns></returns>
    public BlockTypeEnum GetBlockType()
    {
        return (BlockTypeEnum)id;
    }

    /// <summary>
    /// 获取方块形状
    /// </summary>
    /// <returns></returns>
    public BlockShapeEnum GetBlockShape()
    {
        return (BlockShapeEnum)shape;
    }

    protected List<Vector2Int> listUVData = new List<Vector2Int>();
    /// <summary>
    /// 获取UV坐标点
    /// </summary>
    /// <returns></returns>
    public List<Vector2Int> GetUVPosition()
    {
        if (listUVData.IsNull())
        {
            listUVData = new List<Vector2Int>();
            if (uv_position.IsNull())
                return listUVData;
            string[] uvArrary = StringUtil.SplitBySubstringForArrayStr(uv_position, '|');
            for (int i = 0; i < uvArrary.Length; i++)
            {
                string uvItemStr = uvArrary[i];
                int[] uvPositionArray = StringUtil.SplitBySubstringForArrayInt(uvItemStr, ',');
                listUVData.Add(new Vector2Int(uvPositionArray[0], uvPositionArray[1]));
            }
        }
        return listUVData;
    }
}