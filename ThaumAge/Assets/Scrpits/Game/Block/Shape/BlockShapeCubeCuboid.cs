﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockShapeCubeCuboid : BlockShapeCube
{
    protected float leftOffsetBorder;
    protected float rightOffsetBorder;
    protected float downOffsetBorder;
    protected float upOffsetBorder;
    protected float forwardOffsetBorder;
    protected float backOffsetBorder;

    public override void InitData(Block block)
    {
        base.InitData(block);

        float[] offsetBorder = block.blockInfo.GetOffsetBorder();

        leftOffsetBorder = offsetBorder[0];
        rightOffsetBorder = offsetBorder[1];
        downOffsetBorder = offsetBorder[2];
        upOffsetBorder = offsetBorder[3];
        forwardOffsetBorder = offsetBorder[4];
        backOffsetBorder = offsetBorder[5];

        for (int i = 0; i < vertsAddLeft.Length; i++)
        {
            vertsAddLeft[i] = vertsAddLeft[i].AddX(leftOffsetBorder);
        }
        for (int i = 0; i < vertsAddRight.Length; i++)
        {
            vertsAddRight[i] = vertsAddRight[i].AddX(rightOffsetBorder);
        }
        for (int i = 0; i < vertsAddDown.Length; i++)
        {
            vertsAddDown[i] = vertsAddDown[i].AddY(downOffsetBorder);
        }
        for (int i = 0; i < vertsAddUp.Length; i++)
        {
            vertsAddUp[i] = vertsAddUp[i].AddY(upOffsetBorder);
        }
        for (int i = 0; i < vertsAddForward.Length; i++)
        {
            vertsAddForward[i] = vertsAddForward[i].AddZ(forwardOffsetBorder);
        }
        for (int i = 0; i < vertsAddBack.Length; i++)
        {
            vertsAddBack[i] = vertsAddBack[i].AddZ(backOffsetBorder);
        }
    }

    /// <summary>
    /// 构建方块的六个面
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="position"></param>
    /// <param name="blockData"></param>
    /// <param name="verts"></param>
    /// <param name="uvs"></param>
    /// <param name="tris"></param>
    public override void BuildBlock(Block block, Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
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

            int buildFaceCount = 0;

            //Left
            if (leftOffsetBorder == 0 ? CheckNeedBuildFace(block, chunk, localPosition, direction, DirectionEnum.Left) : true)
            {
                BuildFace(block, chunk, localPosition, direction, DirectionEnum.Left, vertsAddLeft, block.uvsAddLeft, false);
                buildFaceCount++;
            }
            //Right
            if (rightOffsetBorder == 0 ? CheckNeedBuildFace(block, chunk, localPosition, direction, DirectionEnum.Right) : true)
            {
                BuildFace(block, chunk, localPosition, direction, DirectionEnum.Right, vertsAddRight, block.uvsAddRight, true);
                buildFaceCount++;
            }
            //Bottom
            if (downOffsetBorder == 0 ? CheckNeedBuildFace(block, chunk, localPosition, direction, DirectionEnum.Down) : true)
            {
                BuildFace(block, chunk, localPosition, direction, DirectionEnum.Down, vertsAddDown, block.uvsAddDown, false);
                buildFaceCount++;
            }
            //Top
            if (upOffsetBorder == 0 ? CheckNeedBuildFace(block, chunk, localPosition, direction, DirectionEnum.UP) : true)
            {
                BuildFace(block, chunk, localPosition, direction, DirectionEnum.UP, vertsAddUp, block.uvsAddUp, true);
                buildFaceCount++;
            }
            //Front
            if (forwardOffsetBorder == 0 ? CheckNeedBuildFace(block, chunk, localPosition, direction, DirectionEnum.Forward) : true)
            {
                BuildFace(block, chunk, localPosition, direction, DirectionEnum.Forward, vertsAddForward, block.uvsAddForward, true);
                buildFaceCount++;
            }
            //Back
            if (backOffsetBorder == 0 ? CheckNeedBuildFace(block, chunk, localPosition, direction, DirectionEnum.Back) : true)
            {
                BuildFace(block, chunk, localPosition, direction, DirectionEnum.Back, vertsAddBack, block.uvsAddBack, false);
                buildFaceCount++;
            }
            int vertsCount = buildFaceCount * 4;
            int trisIndex = buildFaceCount * 6;

            if (vertsCount != 0)
            {
                chunk.chunkMeshData.AddMeshIndexData(localPosition,
                    startVertsIndex, vertsCount, startTrisIndex, trisIndex,
                    startVertsColliderIndex, vertsCount, startTrisColliderIndex, trisIndex);
            }
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
    public override bool CheckNeedBuildFace(Block block, Chunk chunk, Vector3Int localPosition, DirectionEnum direction, DirectionEnum closeDirection)
    {
        if (localPosition.y == 0) return false;
        GetCloseRotateBlockByDirection(block, chunk, localPosition, direction, closeDirection, out Block closeBlock, out Chunk closeBlockChunk);
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