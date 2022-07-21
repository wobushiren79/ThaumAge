using UnityEditor;
using UnityEngine;

public class BlockTypeSeaweed : BlockBaseLiquidSame
{
    public override bool CheckIsSameType(Chunk closeChunk, Block closeBlock)
    {
        return BlockTypeWater.CheckIsSameTypeCommon(closeChunk, closeBlock);
    }
}