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
        float[] offsetBorder = blockInfo.GetOffsetBorder();
        float leftOffsetBorder = offsetBorder[0];
        float rightOffsetBorder = offsetBorder[1];
        float downOffsetBorder = offsetBorder[2];
        float upOffsetBorder = offsetBorder[3];
        float forwardOffsetBorder = offsetBorder[4];
        float backOffsetBorder = offsetBorder[5];
        if (blockType != BlockTypeEnum.None)
        {
            //Left
            if (leftOffsetBorder == 0 ? CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Left) : true)
                BuildFace(chunk, localPosition, direction, chunkMeshData, DirectionEnum.Left, new Vector3(localPosition.x + leftOffsetBorder, localPosition.y, localPosition.z), Vector3.up, Vector3.forward, false);
            //Right
            if (rightOffsetBorder == 1 ? CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Right) : true)
                BuildFace(chunk, localPosition, direction, chunkMeshData, DirectionEnum.Right, new Vector3(localPosition.x + rightOffsetBorder, localPosition.y, localPosition.z), Vector3.up, Vector3.forward, true);

            //Bottom
            if (downOffsetBorder == 0 ? CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Down) : true)
                BuildFace(chunk, localPosition, direction, chunkMeshData, DirectionEnum.Down, new Vector3(localPosition.x, localPosition.y + downOffsetBorder, localPosition.z), Vector3.forward, Vector3.right, false);
            //Top
            if (upOffsetBorder == 1 ? CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.UP) : true)
                BuildFace(chunk, localPosition, direction, chunkMeshData, DirectionEnum.UP, new Vector3(localPosition.x, localPosition.y + upOffsetBorder, localPosition.z), Vector3.forward, Vector3.right, true);

            //Front
            if (forwardOffsetBorder == 0 ? CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Forward) : true)
                BuildFace(chunk, localPosition, direction, chunkMeshData, DirectionEnum.Forward, new Vector3(localPosition.x, localPosition.y, localPosition.z + forwardOffsetBorder), Vector3.up, Vector3.right, true);
            //Back
            if (backOffsetBorder == 1 ? CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Back) : true)
                BuildFace(chunk, localPosition, direction, chunkMeshData, DirectionEnum.Back, new Vector3(localPosition.x, localPosition.y, localPosition.z + backOffsetBorder), Vector3.up, Vector3.right, false);
        }
    }


    /// <summary>
    /// 检测是否需要构建面
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    /// <param name="direction"></param>
    /// <param name="closeDirection"></param>
    /// <returns></returns>
    public override bool CheckNeedBuildFace(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, DirectionEnum closeDirection)
    {
        if (localPosition.y == 0) return false;
        GetCloseRotateBlockByDirection(chunk, localPosition, direction, closeDirection, out Block closeBlock, out Chunk closeBlockChunk);
        if (closeBlock == null || closeBlock.blockType == BlockTypeEnum.None)
        {
            if (closeBlockChunk)
            {
                //只是空气方块
                return true;
            }
            else
            {
                //还没有生成chunk
                return false;
            }
        }
        BlockShapeEnum blockShape = closeBlock.blockInfo.GetBlockShape();
        switch (blockShape)
        {
            case BlockShapeEnum.Cube:
            case BlockShapeEnum.CubeCuboid:
                return false;
            default:
                return true;
        }
    }
}