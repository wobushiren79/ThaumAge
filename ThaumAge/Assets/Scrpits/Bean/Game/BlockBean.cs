﻿using System;
using UnityEngine;

[Serializable]
public class BlockBean 
{
    //方块类型
    public ushort blockId;
    //方向
    public byte direction;
    //方块数据
    public string meta;
    //方块位置
    public Vector3IntBean localPosition;
    //方块的世界坐标
    public Vector3IntBean worldPosition;

    public BlockBean()
    {

    }

    public BlockBean(BlockTypeEnum blockType, Vector3Int worldPosition)
    {
        SetData(blockType, Vector3Int.zero, worldPosition, DirectionEnum.UP);
    }
    public BlockBean(BlockTypeEnum blockType, Vector3Int worldPosition, DirectionEnum direction)
    {
        SetData(blockType, Vector3Int.zero, worldPosition, direction);
    }
    public BlockBean(BlockTypeEnum blockType, Vector3Int localposition,Vector3Int worldPosition)
    {
        SetData(blockType, localposition, worldPosition,DirectionEnum.UP);
    }
    public BlockBean(BlockTypeEnum blockType, Vector3Int localposition, Vector3Int worldPosition, DirectionEnum direction)
    {
        SetData(blockType, localposition, worldPosition, direction);
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="blockType"></param>
    /// <param name="localposition"></param>
    /// <param name="worldPosition"></param>
    public void SetData(BlockTypeEnum blockType, Vector3Int localposition, Vector3Int worldPosition, DirectionEnum direction)
    {
        this.blockId = (ushort)blockType;
        this.localPosition = new Vector3IntBean(localposition);
        this.worldPosition = new Vector3IntBean(worldPosition);
        this.direction = (byte)direction;
    }

    /// <summary>
    /// 获取方块类型
    /// </summary>
    /// <returns></returns>
    public BlockTypeEnum GetBlockType()
    {
        return (BlockTypeEnum)blockId;
    }

    public void SetBlockType(BlockTypeEnum blockType)
    {
        this.blockId = (ushort)blockType;
    }

    public void SetBlockType(ushort blockType)
    {
        this.blockId = blockType;
    }

    /// <summary>
    /// 获取方块数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetBlockData<T>()
    {
        if (CheckUtil.StringIsNull(meta))
            return default;
        T data = JsonUtil.FromJson<T>(meta);
        return data;
    }

    /// <summary>
    /// 获取方向
    /// </summary>
    /// <returns></returns>
    public DirectionEnum GetDirection()
    {
        if (direction == 0)
            return DirectionEnum.UP;
       return (DirectionEnum)direction;
    }
}