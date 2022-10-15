using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockShapeNone : BlockShape
{
    public BlockShapeNone(Block block) : base(block)
    {

    }
    /// <summary>
    /// 添加坐标点
    /// </summary>
    /// <param name="corner"></param>
    /// <param name="up"></param>
    /// <param name="right"></param>
    /// <param name="verts"></param>
    public override void BaseAddVertsUVsColors(
        Chunk chunk, Vector3Int localPosition, BlockDirectionEnum blockDirection,
        Vector3[] vertsAdd, Vector2[] uvsAdd, Color[] colorsAdd)
    {
        if (block.blockInfo.collider_state == 1)
            AddVerts(localPosition, blockDirection, chunk.chunkMeshData.vertsCollider, vertsColliderAdd);

        if (block.blockInfo.trigger_state == 1)
            AddVerts(localPosition, blockDirection, chunk.chunkMeshData.vertsTrigger, vertsColliderAdd);
    }

    /// <summary>
    /// 添加索引
    /// </summary>
    /// <param name="index"></param>
    /// <param name="tris"></param>
    /// <param name="indexCollider"></param>
    /// <param name="trisCollider"></param>
    public override void BaseAddTris(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, int[] trisAdd)
    {
        List<int> trisCollider = chunk.chunkMeshData.trisCollider;
        List<int> trisTrigger = chunk.chunkMeshData.trisTrigger;

        if (block.blockInfo.collider_state == 1)
        {
            int colliderIndex = chunk.chunkMeshData.vertsCollider.Count;
            AddTris(colliderIndex, trisCollider, trisColliderAdd);
        }
        if (block.blockInfo.trigger_state == 1)
        {
            int triggerIndex = chunk.chunkMeshData.vertsTrigger.Count;
            AddTris(triggerIndex, trisTrigger, trisColliderAdd);
        }
    }

    /// <summary>
    /// 获取完整的mesh数据
    /// </summary>
    /// <returns></returns>
    public override Mesh GetCompleteMeshData(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertsColliderAdd;
        mesh.triangles = trisColliderAdd;
        mesh.uv = uvColliderAdd;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        return mesh;
    }
}