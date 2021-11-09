﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockLiquid : Block
{

    /// <summary>
    /// 检测是否需要构建面
    /// </summary>
    /// <param name="position"></param>
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
                return false;
            case BlockShapeEnum.Liquid:
                if (closeBlock.blockType == blockType)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            default:
                return true;
        }
    }


    /// <summary>
    /// 构建方块的面
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="corner"></param>
    /// <param name="up"></param>
    /// <param name="right"></param>
    /// <param name="chunkData"></param>
    public void BuildFace(Vector3Int localPosition, DirectionEnum direction, Vector3 corner, Vector3 up, Vector3 right, ChunkMeshData chunkMeshData)
    {
        AddTris(chunkMeshData);
        AddVerts(localPosition, direction, corner, up, right, chunkMeshData);
        AddUVs(direction, chunkMeshData);
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
    public override void BuildBlock(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, ChunkMeshData chunkMeshData)
    {
        base.BuildBlock(chunk, localPosition, direction, chunkMeshData);

        if (blockType != BlockTypeEnum.None)
        {
            //Left
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Left))
                BuildFace(localPosition, DirectionEnum.Left, localPosition, Vector3.up, Vector3.forward, chunkMeshData);
            //Right
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Right))
                BuildFace(localPosition, DirectionEnum.Right, localPosition + new Vector3Int(1, 0, 0), Vector3.up, Vector3.forward, chunkMeshData);

            //Bottom
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Down))
                BuildFace(localPosition, DirectionEnum.Down, localPosition, Vector3.forward, Vector3.right, chunkMeshData);
            //Top
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.UP))
                BuildFace(localPosition, DirectionEnum.UP, localPosition + new Vector3Int(0, 1, 0), Vector3.forward, Vector3.right, chunkMeshData);

            //Forward
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Forward))
                BuildFace(localPosition, DirectionEnum.Forward, localPosition, Vector3.up, Vector3.right, chunkMeshData);
            //Back
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Back))
                BuildFace(localPosition, DirectionEnum.Back, localPosition + new Vector3Int(0, 0, 1), Vector3.up, Vector3.right, chunkMeshData);
        }
    }


    public virtual void AddVerts(Vector3Int localPosition, DirectionEnum direction, Vector3 corner, Vector3 up, Vector3 right, ChunkMeshData chunkMeshData)
    {
        AddVert(localPosition, direction, chunkMeshData.verts, chunkMeshData.indexVert, corner);
        chunkMeshData.indexVert++;
        AddVert(localPosition, direction, chunkMeshData.verts, chunkMeshData.indexVert, corner + up);
        chunkMeshData.indexVert++;
        AddVert(localPosition, direction, chunkMeshData.verts, chunkMeshData.indexVert, corner + up + right);
        chunkMeshData.indexVert++;
        AddVert(localPosition, direction, chunkMeshData.verts, chunkMeshData.indexVert, corner + right);
        chunkMeshData.indexVert++;

        AddVert(localPosition, direction, chunkMeshData.vertsTrigger, corner);
        AddVert(localPosition, direction, chunkMeshData.vertsTrigger, corner + up);
        AddVert(localPosition, direction, chunkMeshData.vertsTrigger, corner + up + right);
        AddVert(localPosition, direction, chunkMeshData.vertsTrigger, corner + right);
    }

    public virtual void AddUVs(DirectionEnum direction, ChunkMeshData chunkMeshData)
    {
        chunkMeshData.uvs[chunkMeshData.indexUV] = Vector2.zero;
        chunkMeshData.indexUV++;
        chunkMeshData.uvs[chunkMeshData.indexUV] = Vector2.zero + new Vector2(0, 1);
        chunkMeshData.indexUV++;
        chunkMeshData.uvs[chunkMeshData.indexUV] = Vector2.zero + new Vector2(1, 1);
        chunkMeshData.indexUV++;
        chunkMeshData.uvs[chunkMeshData.indexUV] = Vector2.zero + new Vector2(1, 0);
        chunkMeshData.indexUV++;
    }

    public override void AddTris(ChunkMeshData chunkMeshData)
    {
        int index = chunkMeshData.indexVert;
        int triggerIndex = chunkMeshData.vertsTrigger.Count;

        List<int> listTrisWater = chunkMeshData.dicTris[BlockMaterialEnum.Water];

        listTrisWater.Add(index + 0);
        listTrisWater.Add(index + 1);
        listTrisWater.Add(index + 2);

        listTrisWater.Add(index + 0);
        listTrisWater.Add(index + 2);
        listTrisWater.Add(index + 3);

        chunkMeshData.trisTrigger.Add(triggerIndex + 0);
        chunkMeshData.trisTrigger.Add(triggerIndex + 1);
        chunkMeshData.trisTrigger.Add(triggerIndex + 2);

        chunkMeshData.trisTrigger.Add(triggerIndex + 0);
        chunkMeshData.trisTrigger.Add(triggerIndex + 2);
        chunkMeshData.trisTrigger.Add(triggerIndex + 3);
    }
}