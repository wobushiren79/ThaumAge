using UnityEditor;
using UnityEngine;

public class BlockBaseLiquidSame : Block
{
    /// <summary>
    /// 检测是否是同一种类型的方块 比如水草之类的
    /// </summary>
    public virtual bool CheckNeedBuildFaceForSameType(Chunk closeChunk, Block closeBlock)
    {
        return false;
    }
}