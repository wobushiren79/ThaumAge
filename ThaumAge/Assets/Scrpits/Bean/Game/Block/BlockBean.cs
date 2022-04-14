using System;
using UnityEngine;

[Serializable]
public class BlockBean
{
    //方块的本地坐标
    public Vector3Int localPosition;

    //方块类型
    public ushort blockId;
    //方向
    public byte direction;
    //方块数据
    public string meta;

    public BlockBean()
    {

    }

    public BlockBean(Vector3Int localposition, BlockTypeEnum blockType = BlockTypeEnum.None, BlockDirectionEnum direction = BlockDirectionEnum.UpForward, string meta = null)
    {
        SetData(localposition, blockType, direction, meta);
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="blockType"></param>
    /// <param name="localposition"></param>
    /// <param name="worldPosition"></param>
    public void SetData(Vector3Int localPosition, BlockTypeEnum blockType = BlockTypeEnum.None, BlockDirectionEnum direction = BlockDirectionEnum.UpForward, string meta = null)
    {
        this.localPosition = localPosition;

        this.blockId = (ushort)blockType;
        this.direction = (byte)direction;
        this.meta = meta;
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
    public T GetBlockMeta<T>()
    {
        if (meta.IsNull())
            return default;
        T data = JsonUtil.FromJson<T>(meta);
        return data;
    }

    public BlockTypeEnum GetBlockType()
    {
        return (BlockTypeEnum)blockId;
    }

    public DirectionEnum GetDirection()
    {
        if (direction == 0)
            return DirectionEnum.UP;
        return (DirectionEnum)direction;
    }
}

public struct BlockTempBean
{
    //世界坐标
    public int worldX;
    public int worldY;
    public int worldZ;

    //方块类型
    public int blockId;
    //方向
    public byte direction;
    //方块数据
    public string meta;

    public BlockTempBean(BlockTypeEnum blockType, int worldX, int worldY, int worldZ)
    {
        this.blockId = (int)blockType;
        direction = (byte)BlockDirectionEnum.UpForward;
        this.worldX = worldX;
        this.worldY = worldY;
        this.worldZ = worldZ;
        meta = null;
    }

    public BlockTempBean(BlockTypeEnum blockType, BlockDirectionEnum direction, int worldX, int worldY, int worldZ)
    {
        this.blockId = (int)blockType;
        this.direction = (byte)direction;
        this.worldX = worldX;
        this.worldY = worldY;
        this.worldZ = worldZ;
        meta = null;
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
        if (meta.IsNull())
            return default;
        T data = JsonUtil.FromJson<T>(meta);
        return data;
    }

    public BlockTypeEnum GetBlockType()
    {
        return (BlockTypeEnum)blockId;
    }

    public BlockDirectionEnum GetDirection()
    {
        if (direction == 0)
            return BlockDirectionEnum.UpForward;
        return (BlockDirectionEnum)direction;
    }
}