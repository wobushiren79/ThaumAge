using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockTypeMagma : BlockTypeWater
{
    public override bool CheckIsSameType(Chunk closeChunk, Block closeBlock)
    {
        return CheckIsSameTypeMagma(closeChunk, closeBlock);
    }

    public static bool CheckIsSameTypeMagma(Chunk closeChunk, Block closeBlock)
    {
        switch (closeBlock.blockType)
        {
            case BlockTypeEnum.Magma:
                return true;
            default:
                return false;
        }
    }
}