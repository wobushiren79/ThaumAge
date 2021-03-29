using System;
using UnityEngine;

[Serializable]
public class BlockBean 
{
    //方块类型
    public long blockId;
    //方块位置
    public Vector3IntBean position;
    //方块的世界坐标
    public Vector3IntBean worldPosition;
    //方块数据
    public string meta;

    public BlockBean()
    {

    }

    public BlockBean(BlockTypeEnum blockType, Vector3Int position,Vector3Int worldPosition)
    {
        this.blockId = (long)blockType;
        this.position = new Vector3IntBean(position);
        this.worldPosition= new Vector3IntBean(worldPosition);
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
        this.blockId = (long)blockType;
    }

    public void SetBlockType(long blockType)
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
}