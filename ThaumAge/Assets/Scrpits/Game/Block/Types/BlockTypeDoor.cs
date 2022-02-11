using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockTypeDoor : Block
{

    public override void InitBlock(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum blockDirection, int state)
    {
        base.InitBlock(chunk, localPosition, blockDirection, state);
        //如果是放置
        if (state == 1)
        {
            CreateLinkBlock(chunk, localPosition, blockDirection, new List<Vector3Int>() { Vector3Int.up });
        }
    }

    public override void CreateBlockModel(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum blockDirection)
    {
        //如果有模型。则创建模型
        if (!blockInfo.model_name.IsNull())
        {
            //获取数据
            BlockBean blockData = chunk.GetBlockData(localPosition);
            if (blockData != null)
            {
                BlockDoorBean blockDoorData = FromMetaData<BlockDoorBean>(blockData.meta);
                if (blockDoorData != null)
                {
                    if (blockDoorData.level == 1)
                        return;
                }
            }
            chunk.listBlockModelUpdate.Enqueue(localPosition);
        }
    }

    public override void DestoryBlock(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
    {
        base.DestoryBlock(chunk, localPosition, direction);
        DestoryLinkBlock(chunk, localPosition, direction, new List<Vector3Int>() { Vector3Int.up });
    }
}