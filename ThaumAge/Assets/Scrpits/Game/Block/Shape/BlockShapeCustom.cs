using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockShapeCustom : BlockShape
{
    //自定义形状方块所有的数据
    public MeshData blockMeshData;
    public Vector3[] vertsColliderAddCustom;
    public int[] trisColliderAddCustom;
    public override void InitData(Block block)
    {
        base.InitData(block);
        blockMeshData = block.blockInfo.GetBlockMeshData();
        vertsAdd = blockMeshData.vertices;
        trisAdd = blockMeshData.triangles;
        uvsAdd = blockMeshData.uv;

        if (!blockMeshData.verticesCollider.IsNull())
            vertsColliderAddCustom = blockMeshData.verticesCollider;

        if (!blockMeshData.trianglesCollider.IsNull())
            trisColliderAddCustom = blockMeshData.trianglesCollider;
    }

    public override void BuildBlock(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
    {
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

            BuildFace(chunk, localPosition, direction, vertsAdd);

            chunk.chunkMeshData.AddMeshIndexData(localPosition,
                     startVertsIndex, vertsAdd.Length, startTrisIndex, trisAdd.Length,
                     startVertsColliderIndex, vertsColliderAddCustom.Length, startTrisColliderIndex, trisColliderAddCustom.Length);
        }
    }


    public override void BaseAddUVs(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
    {
        base.BaseAddUVs(chunk, localPosition, direction);
        AddUVs(chunk.chunkMeshData.uvs, uvsAdd);
    }

    #region 增加三角
    public override void BaseAddTris(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
    {
        base.BaseAddTris(chunk, localPosition, direction);

        int index = chunk.chunkMeshData.verts.Count;

        List<int> trisData = chunk.chunkMeshData.dicTris[block.blockInfo.material_type];
        List<int> trisCollider = chunk.chunkMeshData.trisCollider;
        List<int> trisTrigger = chunk.chunkMeshData.trisTrigger;

        AddTris(index, trisData, trisAdd);
        if (block.blockInfo.collider_state == 1)
        {
            int colliderIndex = chunk.chunkMeshData.vertsCollider.Count;
            AddTris(colliderIndex, trisCollider, trisColliderAddCustom);
        }
        else if (block.blockInfo.trigger_state == 1)
        {
            int triggerIndex = chunk.chunkMeshData.vertsTrigger.Count;
            AddTris(triggerIndex, trisTrigger, trisColliderAddCustom);
        }
    }
    #endregion

    #region 增加顶点
    public override void BaseAddVerts(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, Vector3[] vertsAdd)
    {
        base.BaseAddVerts(chunk, localPosition, direction, vertsAdd);
        AddVerts(localPosition, direction, chunk.chunkMeshData.verts, vertsAdd);
        if (block.blockInfo.collider_state == 1)
            AddVerts(localPosition, direction, chunk.chunkMeshData.vertsCollider, vertsColliderAddCustom);
        else if (block.blockInfo.trigger_state == 1)
            AddVerts(localPosition, direction, chunk.chunkMeshData.vertsTrigger, vertsColliderAddCustom);
    }
    #endregion
}