using System.Collections.Generic;
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
    public override void BuildBlock(Chunk.ChunkData chunkData)
    {
        base.BuildBlock(chunkData);

        BlockTypeEnum blockType = blockData.GetBlockType();
        if (blockType != BlockTypeEnum.None)
        {
            //Left
            if (CheckNeedBuildFace(localPosition + new Vector3Int(-1, 0, 0)))
                BuildFace(DirectionEnum.Left, blockData, localPosition, Vector3.up, Vector3.forward, false, chunkData);
            //Right
            if (CheckNeedBuildFace(localPosition + new Vector3Int(1, 0, 0)))
                BuildFace(DirectionEnum.Right, blockData, localPosition + new Vector3Int(1, 0, 0), Vector3.up, Vector3.forward, true, chunkData);

            //Bottom
            if (CheckNeedBuildFace(localPosition + new Vector3Int(0, -1, 0)))
                BuildFace(DirectionEnum.Down, blockData, localPosition, Vector3.forward, Vector3.right, false, chunkData);
            //Top
            if (CheckNeedBuildFace(localPosition + new Vector3Int(0, 1, 0)))
                BuildFace(DirectionEnum.UP, blockData, localPosition + new Vector3Int(0, 1, 0), Vector3.forward, Vector3.right, true, chunkData);

            //Front
            if (CheckNeedBuildFace(localPosition + new Vector3Int(0, 0, -1)))
                BuildFace(DirectionEnum.Front, blockData, localPosition, Vector3.up, Vector3.right, true, chunkData);
            //Back
            if (CheckNeedBuildFace(localPosition + new Vector3Int(0, 0, 1)))
                BuildFace(DirectionEnum.Back, blockData, localPosition + new Vector3Int(0, 0, 1), Vector3.up, Vector3.right, false, chunkData);
        }
    }

    /// <summary>
    /// 构建方块的面
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="blockData"></param>
    /// <param name="corner"></param>
    /// <param name="up"></param>
    /// <param name="right"></param>
    /// <param name="reversed"></param>
    /// <param name="verts"></param>
    /// <param name="uvs"></param>
    /// <param name="tris"></param>
    /// <param name="vertsCollider"></param>
    /// <param name="trisCollider"></param>
    public void BuildFace(DirectionEnum direction, BlockBean blockData, Vector3 corner, Vector3 up, Vector3 right, bool reversed, Chunk.ChunkData chunkData)
    {
        AddTris(reversed, chunkData);
        AddVerts(corner, up, right, chunkData);
        AddUVs(direction, blockData, chunkData);
    }

    public virtual void AddVerts(Vector3 corner, Vector3 up, Vector3 right, Chunk.ChunkData chunkData)
    {
        chunkData.verts.Add(corner);
        chunkData.verts.Add(corner + up);
        chunkData.verts.Add(corner + up + right);
        chunkData.verts.Add(corner + right);

        chunkData.vertsCollider.Add(corner);
        chunkData.vertsCollider.Add(corner + up);
        chunkData.vertsCollider.Add(corner + up + right);
        chunkData.vertsCollider.Add(corner + right);
    }

    public void AddUVs(DirectionEnum direction, BlockBean blockData, Chunk.ChunkData chunkData)
    {
        BlockInfoBean blockInfo = BlockHandler.Instance.manager.GetBlockInfo(blockData.GetBlockType());
        List<Vector2Int> listData = blockInfo.GetUVPosition();
        Vector2 uvStartPosition;
        if (CheckUtil.ListIsNull(listData))
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

    public void AddTris(bool reversed, Chunk.ChunkData chunkData)
    {
        int index = chunkData.verts.Count;
        int indexCollider = chunkData.vertsCollider.Count;
        if (reversed)
        {
            chunkData.tris.Add(index + 0);
            chunkData.tris.Add(index + 1);
            chunkData.tris.Add(index + 2);

            chunkData.tris.Add(index + 0);
            chunkData.tris.Add(index + 2);
            chunkData.tris.Add(index + 3);

            chunkData.trisCollider.Add(indexCollider + 0);
            chunkData.trisCollider.Add(indexCollider + 1);
            chunkData.trisCollider.Add(indexCollider + 2);

            chunkData.trisCollider.Add(indexCollider + 0);
            chunkData.trisCollider.Add(indexCollider + 2);
            chunkData.trisCollider.Add(indexCollider + 3);
        }
        else
        {
            chunkData.tris.Add(index + 0);
            chunkData.tris.Add(index + 2);
            chunkData.tris.Add(index + 1);

            chunkData.tris.Add(index + 0);
            chunkData.tris.Add(index + 3);
            chunkData.tris.Add(index + 2);

            chunkData.trisCollider.Add(indexCollider + 0);
            chunkData.trisCollider.Add(indexCollider + 2);
            chunkData.trisCollider.Add(indexCollider + 1);

            chunkData.trisCollider.Add(indexCollider + 0);
            chunkData.trisCollider.Add(indexCollider + 3);
            chunkData.trisCollider.Add(indexCollider + 2);
        }
    }

}