﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockShapePlantCrossOblique : BlockShapeCrossOblique , IBlockPlant
{
    public BlockShapePlantCrossOblique() : base()
    {
        this.InitPlantVert(vertsAdd);
    }

    public override void BaseAddUVs(Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {
        Vector2[] uvsAdd = this.GetUVsAddForPlant(chunk, localPosition, blockInfo, uvWidth);
        AddUVs(chunk.chunkMeshData.uvs, uvsAdd);
    }

    public override void InitBlock(Chunk chunk, Vector3Int localPosition)
    {
        base.InitBlock(chunk, localPosition);
        this.InitPlantData(chunk, localPosition);
    }

    public override void EventBlockUpdateForMin(Chunk chunk, Vector3Int localPosition)
    {
        base.EventBlockUpdateForMin(chunk, localPosition);
        this.RefreshPlant(chunk, localPosition, blockInfo);
    }
}