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
    public override void BuildBlock(Chunk.ChunkRenderData chunkData)
    {
        base.BuildBlock(chunkData);

        if (blockType != BlockTypeEnum.None)
        {
            //Left
                BuildFace(DirectionEnum.Left, localPosition + new Vector3(1f / 16f, 0, 0), Vector3.up, Vector3.forward, false, chunkData);
            //Right
                BuildFace(DirectionEnum.Right, localPosition + new Vector3(15f / 16f, 0, 0), Vector3.up, Vector3.forward, true, chunkData);

            //Bottom
            if (CheckNeedBuildFace(DirectionEnum.Down))
                BuildFace(DirectionEnum.Down, localPosition, Vector3.forward, Vector3.right, false, chunkData);
            //Top
            if (CheckNeedBuildFace(DirectionEnum.UP))
                BuildFace(DirectionEnum.UP, localPosition + new Vector3Int(0, 1, 0), Vector3.forward, Vector3.right, true, chunkData);

            //Front
                BuildFace(DirectionEnum.Forward, localPosition + new Vector3(0, 0, 1f / 16f), Vector3.up, Vector3.right, true, chunkData);
            //Back
                BuildFace(DirectionEnum.Back, localPosition + new Vector3(0, 0, 15f / 16f), Vector3.up, Vector3.right, false, chunkData);
        }
    }
}