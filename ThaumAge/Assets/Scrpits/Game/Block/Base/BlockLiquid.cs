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
    public void BuildFace(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, ChunkMeshData chunkMeshData, Vector3 corner, Vector3 up, Vector3 right)
    {
        AddTris(chunk, localPosition, direction,chunkMeshData);
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
                BuildFace(chunk, localPosition, DirectionEnum.Left, chunkMeshData, localPosition, Vector3.up, Vector3.forward);
            //Right
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Right))
                BuildFace(chunk, localPosition, DirectionEnum.Right, chunkMeshData, localPosition + new Vector3Int(1, 0, 0), Vector3.up, Vector3.forward);

            //Bottom
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Down))
                BuildFace(chunk, localPosition, DirectionEnum.Down, chunkMeshData, localPosition, Vector3.forward, Vector3.right);
            //Top
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.UP))
                BuildFace(chunk, localPosition, DirectionEnum.UP, chunkMeshData, localPosition + new Vector3Int(0, 1, 0), Vector3.forward, Vector3.right);

            //Forward
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Forward))
                BuildFace(chunk, localPosition, DirectionEnum.Forward, chunkMeshData, localPosition, Vector3.up, Vector3.right);
            //Back
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Back))
                BuildFace(chunk, localPosition, DirectionEnum.Back, chunkMeshData, localPosition + new Vector3Int(0, 0, 1), Vector3.up, Vector3.right);
        }
    }


    public virtual void AddVerts(Vector3Int localPosition, DirectionEnum direction, Vector3 corner, Vector3 up, Vector3 right, ChunkMeshData chunkMeshData)
    {
        List<Vector3> verts = chunkMeshData.verts;

        AddVert(localPosition, direction, verts, corner);
        AddVert(localPosition, direction, verts, corner + up);
        AddVert(localPosition, direction, verts, corner + up + right);
        AddVert(localPosition, direction, verts, corner + right);

        AddVert(localPosition, direction, chunkMeshData.vertsTrigger, corner);
        AddVert(localPosition, direction, chunkMeshData.vertsTrigger, corner + up);
        AddVert(localPosition, direction, chunkMeshData.vertsTrigger, corner + up + right);
        AddVert(localPosition, direction, chunkMeshData.vertsTrigger, corner + right);
    }

    public virtual void AddUVs(DirectionEnum direction, ChunkMeshData chunkMeshData)
    {
        List<Vector2> uvs = chunkMeshData.uvs;
        uvs.Add(Vector2.zero);
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));
    }

    public override void AddTris(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, ChunkMeshData chunkMeshData)
    {
        int index = chunkMeshData.verts.Count;
        int triggerIndex = chunkMeshData.vertsTrigger.Count;

        List<int> trisWater = chunkMeshData.dicTris[(int)BlockMaterialEnum.Water];

        trisWater.Add(index);
        trisWater.Add(index + 1);
        trisWater.Add(index + 2);

        trisWater.Add(index);
        trisWater.Add(index + 2);
        trisWater.Add(index + 3);

        chunkMeshData.trisTrigger.Add(triggerIndex + 0);
        chunkMeshData.trisTrigger.Add(triggerIndex + 1);
        chunkMeshData.trisTrigger.Add(triggerIndex + 2);

        chunkMeshData.trisTrigger.Add(triggerIndex + 0);
        chunkMeshData.trisTrigger.Add(triggerIndex + 2);
        chunkMeshData.trisTrigger.Add(triggerIndex + 3);
    }
}