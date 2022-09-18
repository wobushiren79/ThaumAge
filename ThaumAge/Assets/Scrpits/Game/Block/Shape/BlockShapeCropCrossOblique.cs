﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockShapeCropCrossOblique : BlockShapeCrossOblique
{
    public BlockShapeCropCrossOblique(Block block) : base(block)
    {
        vertsAdd = BlockBaseCrop.InitCropVert(vertsAdd);
    }

    /// <summary>
    /// 获取UVAdd
    /// </summary>
    public static Vector2[] GetUVsAddForCrop(Chunk chunk, Block block, Vector3Int localPosition, BlockInfoBean blockInfo)
    {
        BlockBean blockData = chunk.GetBlockData(localPosition);
        BlockMetaCrop blockCropData = BlockBaseCrop.FromMetaData<BlockMetaCrop>(blockData.meta);

        Vector2 uvStart = BlockBaseCrop.GetUVStartPosition(block, blockInfo, blockCropData);
        Vector2[] uvsAdd = new Vector2[]
        {
            new Vector2(uvStart.x,uvStart.y),
            new Vector2(uvStart.x,uvStart.y + uvWidth),
            new Vector2(uvStart.x + uvWidth,uvStart.y + uvWidth),
            new Vector2(uvStart.x + uvWidth,uvStart.y),

            new Vector2(uvStart.x,uvStart.y),
            new Vector2(uvStart.x,uvStart.y + uvWidth),
            new Vector2(uvStart.x + uvWidth,uvStart.y + uvWidth),
            new Vector2(uvStart.x + uvWidth,uvStart.y)
        };
        return uvsAdd;
    }

    public override void BaseAddVertsUVsColors(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum blockDirection,
        Vector3[] vertsAdd, Vector2[] uvsAdd, Color[] colorsAdd)
    {
        Vector2[] uvsAddNew = GetUVsAddForCrop(chunk, block, localPosition, block.blockInfo);
        base.BaseAddVertsUVsColors(chunk, localPosition, blockDirection, vertsAdd, uvsAddNew, colorsAdd);
    }
}