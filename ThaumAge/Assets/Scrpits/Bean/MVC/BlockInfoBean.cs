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

    public string offset_border;//边偏移

    public float weight;//重量

    public int rotate_state;//是否能旋转 0不能旋转 1能旋转

    public int life;//生命值

    public int material_type = 1;//材质类型

    public int collider_state;//是否碰撞 0没有碰撞 1有碰撞

    public int trigger_state;//是否触碰 0没有触碰 1有触碰

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
   
    /// <summary>
    /// 获取材质类型
    /// </summary>
    /// <returns></returns>
    public BlockMaterialEnum GetBlockMaterialType()
    {
        return (BlockMaterialEnum)material_type;
    }

    protected Vector2Int[] arrayUVData;

    /// <summary>
    /// 获取UV坐标点
    /// </summary>
    /// <returns></returns>
    public Vector2Int[] GetUVPosition()
    {
        if (arrayUVData.IsNull())
        {
            if (uv_position.IsNull())
                return arrayUVData;
            string[] uvArrary = StringUtil.SplitBySubstringForArrayStr(uv_position, '|');
            arrayUVData = new Vector2Int[uvArrary.Length];
            for (int i = 0; i < uvArrary.Length; i++)
            {
                string uvItemStr = uvArrary[i];
                int[] uvPositionArray = StringUtil.SplitBySubstringForArrayInt(uvItemStr, ',');
                arrayUVData[i] = new Vector2Int(uvPositionArray[0], uvPositionArray[1]);
            }
        }
        return arrayUVData;
    }

    protected float[] offsetBorder;

    /// <summary>
    /// 获取偏移边坐标
    /// </summary>
    /// <returns></returns>
    public float[] GetOffsetBorder()
    {
        if (offsetBorder.IsNull())
        {
            offsetBorder = StringUtil.SplitBySubstringForArrayFloat(offset_border, '|');
        }
        return offsetBorder;
    }

}