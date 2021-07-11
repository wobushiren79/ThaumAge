using System;
using UnityEditor;
using UnityEngine;

public class BlockMagma : BlockWater
{
    public override void AddTris(Chunk.ChunkRenderData chunkData)
    {
        int index = chunkData.verts.Count;

        chunkData.dicTris[BlockMaterialEnum.Magma].Add(index + 0);
        chunkData.dicTris[BlockMaterialEnum.Magma].Add(index + 1);
        chunkData.dicTris[BlockMaterialEnum.Magma].Add(index + 2);

        chunkData.dicTris[BlockMaterialEnum.Magma].Add(index + 0);
        chunkData.dicTris[BlockMaterialEnum.Magma].Add(index + 2);
        chunkData.dicTris[BlockMaterialEnum.Magma].Add(index + 3);
    }

    public override void InitBlock(Chunk chunk)
    {
        if (chunk == null)
            return;
        InitSmoke(chunk, localPosition);
        chunk.RegisterEventUpdate(localPosition);
    }
        

    public override void RefreshBlock()
    {
        base.RefreshBlock();
        InitSmoke(chunk, localPosition);
    }
    
    /// <summary>
    /// 初始化烟雾
    /// </summary>
    public void InitSmoke(Chunk chunk,Vector3Int localPosition)
    {
        GetCloseBlockByDirection(DirectionEnum.UP, out BlockTypeEnum blockType, out bool hasChunk);
        if (blockType == BlockTypeEnum.None)
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