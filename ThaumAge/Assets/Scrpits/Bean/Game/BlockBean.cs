using System;
using UnityEngine;

[Serializable]
public class BlockBean 
{
    //方块类型
    public long blockId;
    //方块位置
    public Vector3IntBean position;
    //方块数据
    public string blockData;

    public BlockBean(BlockTypeEnum blockType, Vector3Int position)
    {
        this.blockId = (long)blockType;
        this.position = new Vector3IntBean(position);
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
        if (CheckUtil.StringIsNull(blockData))
            return default;
        T data = JsonUtil.FromJson<T>(blockData);
        return data;
    }
}