﻿using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Security.Cryptography;

public class BlockTypeWater : BlockBaseLiquid
{
    public override bool CheckIsSameType(Chunk closeChunk, Block closeBlock)
    {
        return CheckIsSameTypeWater(closeChunk, closeBlock);
    }

    public static bool CheckIsSameTypeWater(Chunk closeChunk, Block closeBlock)
    {
        switch (closeBlock.blockType)
        {
            case BlockTypeEnum.Water:
            case BlockTypeEnum.CoralRed:
            case BlockTypeEnum.CoralBlue:
            case BlockTypeEnum.CoralYellow:
            case BlockTypeEnum.Seaweed:
                return true;
            default:
                return false;
        }
    }
}