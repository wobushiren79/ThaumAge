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
        GetCloseBlockByDirection(DirectionEnum.UP, out BlockTypeEnum blockType, out bool hasChunk);
        if (blockType == BlockTypeEnum.None)
        {
            //如果上方是空得 则实例化冒烟特效
            base.InitBlock(chunk);
        }
        else
        {
            DestoryBlock(chunk);
            chunk.RegisterEventUpdate(localPosition, FlowUpdate);
        }
    }

    public override void RefreshBlock()
    {
        base.RefreshBlock();
        InitBlock(chunk);
    }

}