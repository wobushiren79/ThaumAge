using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockCubeCuboid : BlockCube
{
    /// <summary>
    /// 构建方块的六个面
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="position"></param>
    /// <param name="blockData"></param>
    /// <param name="verts"></param>
    /// <param name="uvs"></param>
    /// <param name="tris"></param>
    public override void BuildBlock(
        List<Vector3> verts, List<Vector2> uvs, List<int> tris,
        List<Vector3> vertsCollider, List<int> trisCollider,
        List<int> trisBothFace)
    {
        BlockTypeEnum blockType = blockData.GetBlockType();
        if (blockType != BlockTypeEnum.None)
        {
            //Left
            if (CheckNeedBuildFace(position + new Vector3Int(-1, 0, 0)))
                BuildFace(DirectionEnum.Left, blockData, position + new Vector3(1f / 16f, 0, 0), Vector3.up, Vector3.forward, false, verts, uvs, tris, vertsCollider, trisCollider);
            //Right
            if (CheckNeedBuildFace(position + new Vector3Int(1, 0, 0)))
                BuildFace(DirectionEnum.Right, blockData, position + new Vector3(15f / 16f, 0, 0), Vector3.up, Vector3.forward, true, verts, uvs, tris, vertsCollider, trisCollider);

            //Bottom
            if (CheckNeedBuildFace(position + new Vector3Int(0, -1, 0)))
                BuildFace(DirectionEnum.Down, blockData, position, Vector3.forward, Vector3.right, false, verts, uvs, tris, vertsCollider, trisCollider);
            //Top
            if (CheckNeedBuildFace(position + new Vector3Int(0, 1, 0)))
                BuildFace(DirectionEnum.UP, blockData, position + new Vector3Int(0, 1, 0), Vector3.forward, Vector3.right, true, verts, uvs, tris, vertsCollider, trisCollider);

            //Front
            if (CheckNeedBuildFace(position + new Vector3Int(0, 0, -1)))
                BuildFace(DirectionEnum.Front, blockData, position + new Vector3(0, 0, 1f / 16f), Vector3.up, Vector3.right, true, verts, uvs, tris, vertsCollider, trisCollider);
            //Back
            if (CheckNeedBuildFace(position + new Vector3Int(0, 0, 1)))
                BuildFace(DirectionEnum.Back, blockData, position + new Vector3(0, 0, 15f / 16f), Vector3.up, Vector3.right, false, verts, uvs, tris, vertsCollider, trisCollider);
        }
    }
}