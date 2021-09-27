using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
public class BlockCube : Block
{
    public BlockCube() : base()
    {

    }

    public BlockCube(BlockTypeEnum blockType) : base(blockType)
    {

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
                BuildFace(localPosition, direction, DirectionEnum.Left, localPosition, Vector3.up, Vector3.forward, false, chunkData);
            //Right
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Right))
                BuildFace(localPosition, direction, DirectionEnum.Right, localPosition + new Vector3Int(1, 0, 0), Vector3.up, Vector3.forward, true, chunkData);

            //Bottom
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Down))
                BuildFace(localPosition, direction, DirectionEnum.Down, localPosition, Vector3.forward, Vector3.right, false, chunkData);
            //Top
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.UP))
                BuildFace(localPosition, direction, DirectionEnum.UP, localPosition + new Vector3Int(0, 1, 0), Vector3.forward, Vector3.right, true, chunkData);

            //Forward
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Forward))
                BuildFace(localPosition, direction, DirectionEnum.Forward, localPosition, Vector3.up, Vector3.right, true, chunkData);
            //Back
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Back))
                BuildFace(localPosition, direction, DirectionEnum.Back, localPosition + new Vector3Int(0, 0, 1), Vector3.up, Vector3.right, false, chunkData);
        }
    }

    public override void BuildBlockNoCheck(Chunk chunk, Vector3Int localPosition, DirectionEnum direction, Chunk.ChunkRenderData chunkData)
    {
        base.BuildBlock(chunk, localPosition, direction, chunkData);
        if (blockType != BlockTypeEnum.None)
        {
            BuildFace(localPosition, direction, DirectionEnum.Left, localPosition, Vector3.up, Vector3.forward, false, chunkData);
            BuildFace(localPosition, direction, DirectionEnum.Right, localPosition + new Vector3Int(1, 0, 0), Vector3.up, Vector3.forward, true, chunkData);
            BuildFace(localPosition, direction, DirectionEnum.Down, localPosition, Vector3.forward, Vector3.right, false, chunkData);
            BuildFace(localPosition, direction, DirectionEnum.UP, localPosition + new Vector3Int(0, 1, 0), Vector3.forward, Vector3.right, true, chunkData);
            BuildFace(localPosition, direction, DirectionEnum.Forward, localPosition, Vector3.up, Vector3.right, true, chunkData);
            BuildFace(localPosition, direction, DirectionEnum.Back, localPosition + new Vector3Int(0, 0, 1), Vector3.up, Vector3.right, false, chunkData);
        }
    }


    /// <summary>
    /// 构建方块的面
    /// </summary>
    /// <param name="localPosition"></param>
    /// <param name="direction"></param>
    /// <param name="buildDirection"></param>
    /// <param name="corner"></param>
    /// <param name="up"></param>
    /// <param name="right"></param>
    /// <param name="reversed"></param>
    /// <param name="chunkData"></param>
    public void BuildFace(Vector3Int localPosition, DirectionEnum direction, DirectionEnum buildDirection, Vector3 corner, Vector3 up, Vector3 right, bool reversed, Chunk.ChunkRenderData chunkData)
    {
        AddTris(reversed, chunkData);
        AddVerts(localPosition, direction, corner, up, right, chunkData);
        AddUVs(buildDirection, chunkData);
    }

    public virtual void AddVerts(Vector3Int localPosition, DirectionEnum direction, Vector3 corner, Vector3 up, Vector3 right, Chunk.ChunkRenderData chunkData)
    {
        base.AddVerts(localPosition, direction, corner, chunkData);

        AddVert(localPosition, direction, chunkData.verts, corner);
        AddVert(localPosition, direction, chunkData.verts, corner + up);
        AddVert(localPosition, direction, chunkData.verts, corner + up + right);
        AddVert(localPosition, direction, chunkData.verts, corner + right);

        AddVert(localPosition, direction, chunkData.vertsCollider, corner);
        AddVert(localPosition, direction, chunkData.vertsCollider, corner + up);
        AddVert(localPosition, direction, chunkData.vertsCollider, corner + up + right);
        AddVert(localPosition, direction, chunkData.vertsCollider, corner + right);
    }

    public void AddUVs(DirectionEnum direction, Chunk.ChunkRenderData chunkData)
    {
        base.AddUVs(chunkData);

        List<Vector2Int> listData = blockInfo.GetUVPosition();

        Vector2 uvStartPosition;
        if (listData.IsNull())
        {
            uvStartPosition = Vector2.zero;
        }
        else if (listData.Count == 1)
        {
            //只有一种面
            uvStartPosition = new Vector2(uvWidth * listData[0].y, uvWidth * listData[0].x);
        }
        else if (listData.Count == 3)
        {
            //3种面  上 中 下
            switch (direction)
            {
                case DirectionEnum.UP:
                    uvStartPosition = new Vector2(uvWidth * listData[0].y, uvWidth * listData[0].x);
                    break;
                case DirectionEnum.Down:
                    uvStartPosition = new Vector2(uvWidth * listData[2].y, uvWidth * listData[2].x);
                    break;
                default:
                    uvStartPosition = new Vector2(uvWidth * listData[1].y, uvWidth * listData[1].x);
                    break;
            }
        }
        else
        {
            uvStartPosition = Vector2.zero;
        }
        chunkData.uvs.Add(uvStartPosition);
        chunkData.uvs.Add(uvStartPosition + new Vector2(0, uvWidth));
        chunkData.uvs.Add(uvStartPosition + new Vector2(uvWidth, uvWidth));
        chunkData.uvs.Add(uvStartPosition + new Vector2(uvWidth, 0));
    }

    public void AddTris(bool reversed, Chunk.ChunkRenderData chunkData)
    {
        base.AddTris(chunkData);

        int index = chunkData.verts.Count;
        int indexCollider = chunkData.vertsCollider.Count;
        if (reversed)
        {
            chunkData.dicTris[BlockMaterialEnum.Normal].Add(index + 0);
            chunkData.dicTris[BlockMaterialEnum.Normal].Add(index + 1);
            chunkData.dicTris[BlockMaterialEnum.Normal].Add(index + 2);

            chunkData.dicTris[BlockMaterialEnum.Normal].Add(index + 0);
            chunkData.dicTris[BlockMaterialEnum.Normal].Add(index + 2);
            chunkData.dicTris[BlockMaterialEnum.Normal].Add(index + 3);

            chunkData.trisCollider.Add(indexCollider + 0);
            chunkData.trisCollider.Add(indexCollider + 1);
            chunkData.trisCollider.Add(indexCollider + 2);

            chunkData.trisCollider.Add(indexCollider + 0);
            chunkData.trisCollider.Add(indexCollider + 2);
            chunkData.trisCollider.Add(indexCollider + 3);
        }
        else
        {
            chunkData.dicTris[BlockMaterialEnum.Normal].Add(index + 0);
            chunkData.dicTris[BlockMaterialEnum.Normal].Add(index + 2);
            chunkData.dicTris[BlockMaterialEnum.Normal].Add(index + 1);

            chunkData.dicTris[BlockMaterialEnum.Normal].Add(index + 0);
            chunkData.dicTris[BlockMaterialEnum.Normal].Add(index + 3);
            chunkData.dicTris[BlockMaterialEnum.Normal].Add(index + 2);

            chunkData.trisCollider.Add(indexCollider + 0);
            chunkData.trisCollider.Add(indexCollider + 2);
            chunkData.trisCollider.Add(indexCollider + 1);

            chunkData.trisCollider.Add(indexCollider + 0);
            chunkData.trisCollider.Add(indexCollider + 3);
            chunkData.trisCollider.Add(indexCollider + 2);
        }
    }

}