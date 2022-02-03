using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockShapeCustom : BlockShape
{
    public override void InitData(Block block)
    {
        base.InitData(block);
        block.blockMeshData = block.blockInfo.GetBlockMeshData();
        vertsAdd = block.blockMeshData.vertices;
        trisAdd = block.blockMeshData.triangles;
        block.uvsAdd = block.blockMeshData.uv;

        if (!block.blockMeshData.verticesCollider.IsNull())
            block.vertsColliderAddCustom = block.blockMeshData.verticesCollider;

        if (!block.blockMeshData.trianglesCollider.IsNull())
            block.trisColliderAddCustom = block.blockMeshData.trianglesCollider;
    }

    public override void BuildBlock(Block block, Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {
        base.BuildBlock(block, chunk, localPosition, direction);
        if (block.blockType != BlockTypeEnum.None)
        {
            int startVertsIndex = chunk.chunkMeshData.verts.Count;
            int startTrisIndex = chunk.chunkMeshData.dicTris[block.blockInfo.material_type].Count;

            int startVertsColliderIndex = 0;
            int startTrisColliderIndex = 0;

            if (block.blockInfo.collider_state == 1)
            {
                startVertsColliderIndex = chunk.chunkMeshData.vertsCollider.Count;
                startTrisColliderIndex = chunk.chunkMeshData.trisCollider.Count;
            }
            else if (block.blockInfo.trigger_state == 1)
            {
                startVertsColliderIndex = chunk.chunkMeshData.vertsTrigger.Count;
                startTrisColliderIndex = chunk.chunkMeshData.trisTrigger.Count;
            }

            BuildFace(block, chunk, localPosition, direction, vertsAdd);

            chunk.chunkMeshData.AddMeshIndexData(localPosition,
                     startVertsIndex, vertsAdd.Length, startTrisIndex, trisAdd.Length,
                     startVertsColliderIndex, block.vertsColliderAddCustom.Length, startTrisColliderIndex, block.trisColliderAddCustom.Length);
        }
    }

    public override void BaseAddTris(Block block, Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {
        base.BaseAddTris(block, chunk, localPosition, direction);

        int index = chunk.chunkMeshData.verts.Count;

        List<int> trisData = chunk.chunkMeshData.dicTris[block.blockInfo.material_type];
        List<int> trisCollider = chunk.chunkMeshData.trisCollider;
        List<int> trisTrigger = chunk.chunkMeshData.trisTrigger;

        AddTris(index, trisData, trisAdd);
        if (block.blockInfo.collider_state == 1)
        {
            int colliderIndex = chunk.chunkMeshData.vertsCollider.Count;
            AddTris(colliderIndex, trisCollider, block.trisColliderAddCustom);
        }
        if (block.blockInfo.trigger_state == 1)
        {
            int triggerIndex = chunk.chunkMeshData.vertsTrigger.Count;
            AddTris(triggerIndex, trisTrigger, block.trisColliderAddCustom);
        }
    }

    public override void BaseAddUVs(Block block, Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {
        base.BaseAddUVs(block, chunk, localPosition, direction);
        AddUVs(chunk.chunkMeshData.uvs, block.uvsAdd);
    }

    public override void BaseAddVerts(Block block, Chunk chunk, Vector3Int localPosition, DirectionEnum direction, Vector3[] vertsAdd)
    {
        base.BaseAddVerts(block, chunk, localPosition, direction, vertsAdd);
        AddVerts(block, localPosition, direction, chunk.chunkMeshData.verts, vertsAdd);
        if (block.blockInfo.collider_state == 1)
            AddVerts(block, localPosition, direction, chunk.chunkMeshData.vertsCollider, block.vertsColliderAddCustom);
        if (block.blockInfo.trigger_state == 1)
            AddVerts(block, localPosition, direction, chunk.chunkMeshData.vertsTrigger, block.vertsColliderAddCustom);
    }
}