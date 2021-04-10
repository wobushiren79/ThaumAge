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
    public override bool CheckNeedBuildFace(Vector3Int position)
    {
        if (position.y < 0) return false;
        //检测旋转
        Vector3Int checkPosition = Vector3Int.RoundToInt(RotatePosition(position, localPosition));
        //获取方块
        Block block = WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(checkPosition + chunk.worldPosition);
        if (block == null)
            return false;
        BlockShapeEnum blockShape = block.blockInfo.GetBlockShape();
        switch (blockShape)
        {
            case BlockShapeEnum.Cube:
            case BlockShapeEnum.Liquid:
                return false;
            default:
                return true;
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
    public override void BuildBlock(Chunk.ChunkData chunkData)
    {
        base.BuildBlock(chunkData);

        BlockTypeEnum blockType = blockData.GetBlockType();
        if (blockType != BlockTypeEnum.None)
        {
            //Left
            if (CheckNeedBuildFace(localPosition + new Vector3Int(-1, 0, 0)))
                BuildFace(DirectionEnum.Left, blockData, localPosition, Vector3.up, Vector3.forward, chunkData);
            //Right
            if (CheckNeedBuildFace(localPosition + new Vector3Int(1, 0, 0)))
                BuildFace(DirectionEnum.Right, blockData, localPosition + new Vector3Int(1, 0, 0), Vector3.up, Vector3.forward, chunkData);

            //Bottom
            if (CheckNeedBuildFace(localPosition + new Vector3Int(0, -1, 0)))
                BuildFace(DirectionEnum.Down, blockData, localPosition, Vector3.forward, Vector3.right, chunkData);
            //Top
            if (CheckNeedBuildFace(localPosition + new Vector3Int(0, 1, 0)))
                BuildFace(DirectionEnum.UP, blockData, localPosition + new Vector3Int(0, 1, 0), Vector3.forward, Vector3.right, chunkData);

            //Front
            if (CheckNeedBuildFace(localPosition + new Vector3Int(0, 0, -1)))
                BuildFace(DirectionEnum.Forward, blockData, localPosition, Vector3.up, Vector3.right, chunkData);
            //Back
            if (CheckNeedBuildFace(localPosition + new Vector3Int(0, 0, 1)))
                BuildFace(DirectionEnum.Back, blockData, localPosition + new Vector3Int(0, 0, 1), Vector3.up, Vector3.right, chunkData);
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
    public void BuildFace(DirectionEnum direction, BlockBean blockData, Vector3 corner, Vector3 up, Vector3 right,Chunk.ChunkData chunkData)
    {
        AddTris(chunkData);
        AddVerts(corner, up, right, chunkData);
        AddUVs(direction, blockData, chunkData);
    }

    public virtual void AddVerts(Vector3 corner, Vector3 up, Vector3 right, Chunk.ChunkData chunkData)
    {
        chunkData.verts.Add(corner);
        chunkData.verts.Add(corner + up);
        chunkData.verts.Add(corner + up + right);
        chunkData.verts.Add(corner + right);
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
        //chunkData.uvs.Add(uvStartPosition);
        //chunkData.uvs.Add(uvStartPosition + new Vector2(0, uvWidth));
        //chunkData.uvs.Add(uvStartPosition + new Vector2(uvWidth, uvWidth));
        //chunkData.uvs.Add(uvStartPosition + new Vector2(uvWidth, 0));

        chunkData.uvs.Add(Vector2.zero);
        chunkData.uvs.Add(Vector2.zero + new Vector2(0, 1));
        chunkData.uvs.Add(Vector2.zero + new Vector2(1, 1));
        chunkData.uvs.Add(Vector2.zero + new Vector2(1, 0));
    }

    public override void AddTris(Chunk.ChunkData chunkData)
    {
        int index = chunkData.verts.Count;

        chunkData.trisLiquid.Add(index + 0);
        chunkData.trisLiquid.Add(index + 1);
        chunkData.trisLiquid.Add(index + 2);

        chunkData.trisLiquid.Add(index + 0);
        chunkData.trisLiquid.Add(index + 2);
        chunkData.trisLiquid.Add(index + 3);
    }
}