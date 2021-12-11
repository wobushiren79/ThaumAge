using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockShapeCustom : Block
{
    public MeshData blockMeshData;

    public override void SetData(BlockTypeEnum blockType)
    {
        base.SetData(blockType);
        blockMeshData = blockInfo.GetBlockMeshData();
        vertsAdd = blockMeshData.vertices;
        trisAdd = blockMeshData.triangles;
        uvsAdd = blockMeshData.uv;

        if (!blockMeshData.verticesCollider.IsNull())
            vertsColliderAdd = blockMeshData.verticesCollider;

        if (!blockMeshData.trianglesCollider.IsNull())
            trisColliderAdd = blockMeshData.trianglesCollider;
    }

    public override void BuildBlock(Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {
        base.BuildBlock(chunk, localPosition, direction);
        if (blockType != BlockTypeEnum.None)
        {
            int startVertsIndex = chunk.chunkMeshData.verts.Count;
            int startTrisIndex = chunk.chunkMeshData.dicTris[blockInfo.material_type].Count;

            int startVertsColliderIndex = 0;
            int startTrisColliderIndex = 0;

            if (blockInfo.collider_state == 1)
            {
                startVertsColliderIndex = chunk.chunkMeshData.vertsCollider.Count;
                startTrisColliderIndex = chunk.chunkMeshData.trisCollider.Count;
            }
            else if (blockInfo.trigger_state == 1)
            {
                startVertsColliderIndex = chunk.chunkMeshData.vertsTrigger.Count;
                startTrisColliderIndex = chunk.chunkMeshData.trisTrigger.Count;
            }

            BuildFace(chunk, localPosition, direction, vertsAdd);

            chunk.chunkMeshData.AddMeshIndexData(localPosition,
                     startVertsIndex, vertsAdd.Length, startTrisIndex, trisAdd.Length,
                     startVertsColliderIndex, vertsColliderAdd.Length, startTrisColliderIndex, trisColliderAdd.Length);
        }
    }

    public override void BaseAddTris(Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {
        base.BaseAddTris(chunk, localPosition, direction);

        int index = chunk.chunkMeshData.verts.Count;

        List<int> trisData = chunk.chunkMeshData.dicTris[blockInfo.material_type];
        List<int> trisCollider = chunk.chunkMeshData.trisCollider;
        List<int> trisTrigger = chunk.chunkMeshData.trisTrigger;

        AddTris(index, trisData, trisAdd);
        if (blockInfo.collider_state == 1)
        {
            int colliderIndex = chunk.chunkMeshData.vertsCollider.Count;
            AddTris(colliderIndex, trisCollider, trisColliderAdd);
        }
        if (blockInfo.trigger_state == 1)
        {
            int triggerIndex = chunk.chunkMeshData.vertsTrigger.Count;
            AddTris(triggerIndex, trisTrigger, trisColliderAdd);
        }
    }

    public override void BaseAddUVs(Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {
        base.BaseAddUVs(chunk, localPosition, direction);
        AddUVs(chunk.chunkMeshData.uvs, uvsAdd);
    }

    public override void BaseAddVerts(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, Vector3[] vertsAdd)
    {
        base.BaseAddVerts(chunk, localPosition, direction, vertsAdd);
        AddVerts(localPosition, direction, chunk.chunkMeshData.verts, vertsAdd);
        if (blockInfo.collider_state == 1)
            AddVerts(localPosition, direction, chunk.chunkMeshData.vertsCollider, vertsColliderAdd);
        if (blockInfo.trigger_state == 1)
            AddVerts(localPosition, direction, chunk.chunkMeshData.vertsTrigger, vertsColliderAdd);
    }
}