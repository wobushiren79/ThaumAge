﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockMagma : BlockWater
{
    public override void BaseAddTris(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, ChunkMeshData chunkMeshData)
    {
        int index = chunkMeshData.verts.Count;

        List<int> trisMagma = chunkMeshData.dicTris[(int)BlockMaterialEnum.Magma];

        trisMagma.Add(index);
        trisMagma.Add(index + 1);
        trisMagma.Add(index + 2);

        trisMagma.Add(index);
        trisMagma.Add(index + 2);
        trisMagma.Add(index + 3);
    }

    public override void InitBlock(Chunk chunk, Vector3Int localPosition)
    {
        if (chunk == null)
            return;
        InitSmoke(chunk, localPosition);
        chunk.RegisterEventUpdate(localPosition,1);
    }


    public override void RefreshBlock(Chunk chunk, Vector3Int localPosition)
    {
        base.RefreshBlock(chunk, localPosition);
        InitSmoke(chunk, localPosition);
    }

    /// <summary>
    /// 初始化烟雾
    /// </summary>
    public void InitSmoke(Chunk chunk, Vector3Int localPosition)
    {
        GetCloseBlockByDirection(chunk, localPosition, DirectionEnum.UP, out Block block, out Chunk blockChunk);
        if (block == null || block.blockType == BlockTypeEnum.None)
        {
            //如果上方是空得 则实例化冒烟特效
            CreateBlockModel(chunk, localPosition);
        }
        else
        {
            //如果上方有物体，则不冒烟
            DestoryBlockModel(chunk, localPosition);
        }
    }

}