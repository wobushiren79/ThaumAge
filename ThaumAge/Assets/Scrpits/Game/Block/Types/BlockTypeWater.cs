using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Security.Cryptography;

public class BlockTypeWater : BlockBaseLiquid
{
    public override bool CheckNeedBuildFaceForSameType(Chunk closeChunk,Block closeBlock)
    {
        if (closeBlock.blockType == BlockTypeEnum.Water)
        {
            
        }
        return false;
    }
}