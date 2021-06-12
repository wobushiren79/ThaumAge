using System;
using UnityEngine;

[Serializable]
public class BlockBean
{
    //方块的本地坐标
    public Vector3Int localPosition;
    //方块的世界坐标
    public Vector3Int worldPosition;

    //方块类型
    public ushort blockId;
    //方向
    public byte direction;
    //方块数据
    public string meta;

    public BlockBean()
    {

    }
    public BlockBean(Vector3Int worldPosition, BlockTypeEnum blockType = BlockTypeEnum.None, DirectionEnum direction = DirectionEnum.UP)
    {
        SetData(Vector3Int.zero, worldPosition, blockType, direction);
    }

    public BlockBean(Vector3Int localposition, Vector3Int worldPosition, BlockTypeEnum blockType = BlockTypeEnum.None, DirectionEnum direction = DirectionEnum.UP)
    {
        SetData(localposition, worldPosition, blockType, direction);
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="blockType"></param>
    /// <param name="localposition"></param>
    /// <param name="worldPosition"></param>
    public void SetData(Vector3Int localPosition, Vector3Int worldPosition, BlockTypeEnum blockType = BlockTypeEnum.None, DirectionEnum direction = DirectionEnum.UP)
    {
        this.localPosition = localPosition;
        this.worldPosition = worldPosition;

        this.blockId = (ushort)blockType;
        this.direction = (byte)direction;
        this.meta = null;
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

//public struct BlockBean
//{
//    //方块的本地坐标
//    public Vector3Int localPosition;
//    //方块的世界坐标
//    public Vector3Int worldPosition;

//    //方块类型
//    public ushort blockId;
//    //方向
//    public byte direction;
//    //方块数据
//    public string meta;

//    public BlockBean(Vector3Int worldPosition, BlockTypeEnum blockType = BlockTypeEnum.None, DirectionEnum direction = DirectionEnum.UP)
//    {
//        this.localPosition = Vector3Int.zero;
//        this.worldPosition = worldPosition;

//        this.blockId = (ushort)blockType;
//        this.direction = (byte)direction;
//        this.meta = null;
//    }

//    public BlockBean(Vector3Int localPosition, Vector3Int worldPosition, BlockTypeEnum blockType = BlockTypeEnum.None, DirectionEnum direction = DirectionEnum.UP)
//    {
//        this.localPosition = localPosition;
//        this.worldPosition = worldPosition;

//        this.blockId = (ushort)blockType;
//        this.direction = (byte)direction;
//        this.meta = null;
//    }

//    public BlockTypeEnum GetBlockType()
//    {
//        return (BlockTypeEnum)blockId;
//    }

//    public DirectionEnum GetDirection()
//    {
//        if (direction == 0)
//            return DirectionEnum.UP;
//        return (DirectionEnum)direction;
//    }

//}