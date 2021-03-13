/*
* FileName: BlockInfo 
* Author: AppleCoffee 
* CreateTime: 2021-03-11-15:39:10 
*/

using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class BlockInfoBean : BaseBean
{
    public long block_id;

    public int shape;

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
}