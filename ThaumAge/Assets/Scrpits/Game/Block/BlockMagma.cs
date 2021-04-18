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
}