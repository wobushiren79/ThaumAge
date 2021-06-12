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
    public override bool CheckNeedBuildFace(DirectionEnum direction, out BlockTypeEnum closeBlock)
    {
        closeBlock = BlockTypeEnum.None;
        if (localPosition.y == 0)
            return false;
        GetCloseRotateBlockByDirection(direction, out closeBlock, out bool hasChunk);
        if (closeBlock == BlockTypeEnum.None)
        {
            if (hasChunk)
            {
                return true;
            }
            else
            {
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
    public void BuildFace(DirectionEnum direction, Vector3 corner, Vector3 up, Vector3 right, Chunk.ChunkRenderData chunkData)
    {
        AddTris(chunkData);
        AddVerts(corner, up, right, chunkData);
        AddUVs(direction,chunkData);
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
    public override void BuildBlock(Chunk.ChunkRenderData chunkData)
    {
        base.BuildBlock(chunkData);

        if (blockType != BlockTypeEnum.None)
        {
            //Left
            if (CheckNeedBuildFace(DirectionEnum.Left))
                BuildFace(DirectionEnum.Left, localPosition, Vector3.up, Vector3.forward, chunkData);
            //Right
            if (CheckNeedBuildFace(DirectionEnum.Right))
                BuildFace(DirectionEnum.Right, localPosition + new Vector3Int(1, 0, 0), Vector3.up, Vector3.forward, chunkData);

            //Bottom
            if (CheckNeedBuildFace(DirectionEnum.Down))
                BuildFace(DirectionEnum.Down, localPosition, Vector3.forward, Vector3.right, chunkData);
            //Top
            if (CheckNeedBuildFace(DirectionEnum.UP))
                BuildFace(DirectionEnum.UP, localPosition + new Vector3Int(0, 1, 0), Vector3.forward, Vector3.right, chunkData);

            //Forward
            if (CheckNeedBuildFace(DirectionEnum.Forward))
                BuildFace(DirectionEnum.Forward, localPosition, Vector3.up, Vector3.right, chunkData);
            //Back
            if (CheckNeedBuildFace(DirectionEnum.Back))
                BuildFace(DirectionEnum.Back, localPosition + new Vector3Int(0, 0, 1), Vector3.up, Vector3.right, chunkData);
        }
    }


    public virtual void AddVerts(Vector3 corner, Vector3 up, Vector3 right, Chunk.ChunkRenderData chunkData)
    {
        AddVert(chunkData.verts, corner);
        AddVert(chunkData.verts, corner + up);
        AddVert(chunkData.verts, corner + up + right);
        AddVert(chunkData.verts, corner + right);

        AddVert(chunkData.vertsTrigger, corner);
        AddVert(chunkData.vertsTrigger, corner + up);
        AddVert(chunkData.vertsTrigger, corner + up + right);
        AddVert(chunkData.vertsTrigger, corner + right);
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