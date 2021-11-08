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
    public override void BuildBlock(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, ChunkMeshData chunkMeshData)
    {
        base.BuildBlock(chunk, localPosition, direction, chunkMeshData);

        if (blockType != BlockTypeEnum.None)
        {
            //Left
            BuildFace(localPosition, direction, DirectionEnum.Left, localPosition + new Vector3(1f / 16f, 0, 0), Vector3.up, Vector3.forward, false, chunkMeshData);
            //Right
            BuildFace(localPosition, direction, DirectionEnum.Right, localPosition + new Vector3(15f / 16f, 0, 0), Vector3.up, Vector3.forward, true, chunkMeshData);

            //Bottom
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Down))
                BuildFace(localPosition, direction, DirectionEnum.Down, localPosition, Vector3.forward, Vector3.right, false, chunkMeshData);
            //Top
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.UP))
                BuildFace(localPosition, direction, DirectionEnum.UP, localPosition + new Vector3Int(0, 1, 0), Vector3.forward, Vector3.right, true, chunkMeshData);

            //Front
            BuildFace(localPosition, direction, DirectionEnum.Forward, localPosition + new Vector3(0, 0, 1f / 16f), Vector3.up, Vector3.right, true, chunkMeshData);
            //Back
            BuildFace(localPosition, direction, DirectionEnum.Back, localPosition + new Vector3(0, 0, 15f / 16f), Vector3.up, Vector3.right, false, chunkMeshData);
        }
    }
}