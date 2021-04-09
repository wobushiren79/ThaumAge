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
    public long link_id;

    public string name;

    public int shape;

    public string uv_position;

    public float weight;

    public int rotate_state;

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
    /// 获取UV坐标点
    /// </summary>
    /// <returns></returns>
    public List<Vector2Int> GetUVPosition()
    {
        List<Vector2Int> listData = new List<Vector2Int>();
        if (CheckUtil.StringIsNull(uv_position))
            return listData;
        string[] uvArrary = StringUtil.SplitBySubstringForArrayStr(uv_position, '|');
        for (int i = 0; i < uvArrary.Length; i++)
        {
            string uvItemStr = uvArrary[i];
            int[] uvPositionArray = StringUtil.SplitBySubstringForArrayInt(uvItemStr, ',');
            listData.Add(new Vector2Int(uvPositionArray[0], uvPositionArray[1]));
        }
        return listData;
    }
}