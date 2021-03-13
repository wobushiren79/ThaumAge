
using System;

[Serializable]
public class BlockBean 
{
    //方块类型
    public long blockId;
    //方块位置
    public Vector3IntBean position;

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
}