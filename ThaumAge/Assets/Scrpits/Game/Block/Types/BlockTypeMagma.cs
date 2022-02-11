using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockTypeMagma : BlockTypeWater
{
    public override void InitBlock(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, int state)
    {
        if (chunk == null)
            return;
        InitSmoke(chunk, localPosition, direction);
        chunk.RegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
    }


    public override void RefreshBlock(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
    {
        base.RefreshBlock(chunk, localPosition, direction);
        InitSmoke(chunk, localPosition, direction);
    }

    /// <summary>
    /// 初始化烟雾
    /// </summary>
    public void InitSmoke(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
    {
        GetCloseBlockByDirection(chunk, localPosition, DirectionEnum.UP, out Block block, out Chunk blockChunk);
        if (block == null || block.blockType == BlockTypeEnum.None)
        {
            //如果上方是空得 则实例化冒烟特效
            CreateBlockModel(chunk, localPosition, direction);
        }
        else
        {
            //如果上方有物体，则不冒烟
            DestoryBlockModel(chunk, localPosition, direction);
        }
    }
}