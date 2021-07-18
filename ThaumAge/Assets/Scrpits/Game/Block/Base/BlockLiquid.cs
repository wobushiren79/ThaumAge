using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockLiquid : Block
{

    /// <summary>
    /// 检测是否需要构建面
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public override bool CheckNeedBuildFace(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, DirectionEnum closeDirection, out BlockTypeEnum closeBlock)
    {
        closeBlock = BlockTypeEnum.None;
        if (localPosition.y == 0) return false;
        GetCloseRotateBlockByDirection(chunk.chunkData.positionForWorld + localPosition, direction, closeDirection, out closeBlock, out bool hasChunk);
        if (closeBlock == BlockTypeEnum.None)
        {
            if (hasChunk)
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
        Block closeRegisterBlock = BlockHandler.Instance.manager.GetRegisterBlock(closeBlock);
        BlockShapeEnum blockShape = closeRegisterBlock.blockInfo.GetBlockShape();
        switch (blockShape)
        {
            case BlockShapeEnum.Cube:
                return false;
            case BlockShapeEnum.Liquid:
                if (closeBlock == blockType)
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
    public void BuildFace(Vector3Int localPosition, DirectionEnum direction, Vector3 corner, Vector3 up, Vector3 right, Chunk.ChunkRenderData chunkData)
    {
        AddTris(chunkData);
        AddVerts(localPosition, direction, corner, up, right, chunkData);
        AddUVs(direction, chunkData);
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
    public override void BuildBlock(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, Chunk.ChunkRenderData chunkData)
    {
        base.BuildBlock(chunk, localPosition, direction, chunkData);

        if (blockType != BlockTypeEnum.None)
        {
            //Left
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Left))
                BuildFace(localPosition, DirectionEnum.Left, localPosition, Vector3.up, Vector3.forward, chunkData);
            //Right
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Right))
                BuildFace(localPosition, DirectionEnum.Right, localPosition + new Vector3Int(1, 0, 0), Vector3.up, Vector3.forward, chunkData);

            //Bottom
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Down))
                BuildFace(localPosition, DirectionEnum.Down, localPosition, Vector3.forward, Vector3.right, chunkData);
            //Top
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.UP))
                BuildFace(localPosition, DirectionEnum.UP, localPosition + new Vector3Int(0, 1, 0), Vector3.forward, Vector3.right, chunkData);

            //Forward
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Forward))
                BuildFace(localPosition, DirectionEnum.Forward, localPosition, Vector3.up, Vector3.right, chunkData);
            //Back
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Back))
                BuildFace(localPosition, DirectionEnum.Back, localPosition + new Vector3Int(0, 0, 1), Vector3.up, Vector3.right, chunkData);
        }
    }


    public virtual void AddVerts(Vector3Int localPosition, DirectionEnum direction, Vector3 corner, Vector3 up, Vector3 right, Chunk.ChunkRenderData chunkData)
    {
        AddVert(localPosition, direction, chunkData.verts, corner);
        AddVert(localPosition, direction, chunkData.verts, corner + up);
        AddVert(localPosition, direction, chunkData.verts, corner + up + right);
        AddVert(localPosition, direction, chunkData.verts, corner + right);

        AddVert(localPosition, direction, chunkData.vertsTrigger, corner);
        AddVert(localPosition, direction, chunkData.vertsTrigger, corner + up);
        AddVert(localPosition, direction, chunkData.vertsTrigger, corner + up + right);
        AddVert(localPosition, direction, chunkData.vertsTrigger, corner + right);
    }

    public virtual void AddUVs(DirectionEnum direction, Chunk.ChunkRenderData chunkData)
    {
        chunkData.uvs.Add(Vector2.zero);
        chunkData.uvs.Add(Vector2.zero + new Vector2(0, 1));
        chunkData.uvs.Add(Vector2.zero + new Vector2(1, 1));
        chunkData.uvs.Add(Vector2.zero + new Vector2(1, 0));
    }

    public override void AddTris(Chunk.ChunkRenderData chunkData)
    {
        int index = chunkData.verts.Count;
        int triggerIndex = chunkData.vertsTrigger.Count;

        chunkData.dicTris[BlockMaterialEnum.Water].Add(index + 0);
        chunkData.dicTris[BlockMaterialEnum.Water].Add(index + 1);
        chunkData.dicTris[BlockMaterialEnum.Water].Add(index + 2);

        chunkData.dicTris[BlockMaterialEnum.Water].Add(index + 0);
        chunkData.dicTris[BlockMaterialEnum.Water].Add(index + 2);
        chunkData.dicTris[BlockMaterialEnum.Water].Add(index + 3);

        chunkData.trisTrigger.Add(triggerIndex + 0);
        chunkData.trisTrigger.Add(triggerIndex + 1);
        chunkData.trisTrigger.Add(triggerIndex + 2);

        chunkData.trisTrigger.Add(triggerIndex + 0);
        chunkData.trisTrigger.Add(triggerIndex + 2);
        chunkData.trisTrigger.Add(triggerIndex + 3);
    }
}