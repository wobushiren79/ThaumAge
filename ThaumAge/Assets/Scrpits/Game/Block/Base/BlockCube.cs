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
    public override void BuildBlock(
        List<Vector3> verts, List<Vector2> uvs, List<int> tris,
        List<Vector3> vertsCollider, List<int> trisCollider)
    {
        BlockTypeEnum blockType = blockData.GetBlockType();
        if (blockType != BlockTypeEnum.None)
        {
            //Left
            if (CheckNeedBuildFace(position + new Vector3Int(-1, 0, 0)))
                BuildFace(DirectionEnum.Left,  blockData, position, Vector3.up, Vector3.forward, false, verts, uvs, tris, vertsCollider, trisCollider);
            //Right
            if (CheckNeedBuildFace(position + new Vector3Int(1, 0, 0)))
                BuildFace(DirectionEnum.Right, blockData, position + new Vector3Int(1, 0, 0), Vector3.up, Vector3.forward, true, verts, uvs, tris, vertsCollider, trisCollider);

            //Bottom
            if (CheckNeedBuildFace(position + new Vector3Int(0, -1, 0)))
                BuildFace(DirectionEnum.Down, blockData, position, Vector3.forward, Vector3.right, false, verts, uvs, tris, vertsCollider, trisCollider);
            //Top
            if (CheckNeedBuildFace(position + new Vector3Int(0, 1, 0)))
                BuildFace(DirectionEnum.UP, blockData, position + new Vector3Int(0, 1, 0), Vector3.forward, Vector3.right, true, verts, uvs, tris, vertsCollider, trisCollider);

            //Front
            if (CheckNeedBuildFace(position + new Vector3Int(0, 0, -1)))
                BuildFace(DirectionEnum.Front, blockData, position, Vector3.up, Vector3.right, true, verts, uvs, tris, vertsCollider, trisCollider);
            //Back
            if (CheckNeedBuildFace(position + new Vector3Int(0, 0, 1)))
                BuildFace(DirectionEnum.Back, blockData, position + new Vector3Int(0, 0, 1), Vector3.up, Vector3.right, false, verts, uvs, tris, vertsCollider, trisCollider);
        }
    }

    /// <summary>
    /// 构建方块的面
    /// </summary>
    /// <param name="blockData"></param>
    /// <param name="corner"></param>
    /// <param name="up"></param>
    /// <param name="right"></param>
    /// <param name="reversed"></param>
    /// <param name="verts"></param>
    /// <param name="uvs"></param>
    /// <param name="tris"></param>
    public override void BuildFace(BlockBean blockData, Vector3 corner, 
        List<Vector3> verts, List<Vector2> uvs, List<int> tris, 
        List<Vector3> vertsCollider, List<int> trisCollider)
    {

    }

    public  void BuildFace(DirectionEnum direction, BlockBean blockData, Vector3 corner, Vector3 up, Vector3 right, bool reversed,
        List<Vector3> verts, List<Vector2> uvs, List<int> tris, 
        List<Vector3> vertsCollider, List<int> trisCollider)
    {
        int index = verts.Count;
        int indexCollider = vertsCollider.Count;
        AddVerts(corner, up, right, verts,vertsCollider);
        AddUVs(direction, blockData, uvs);
        AddTris(index, indexCollider, reversed, tris,trisCollider);
    }
    public override void AddVerts(Vector3 corner,  List<Vector3> verts, List<Vector3> vertsCollider)
    {

    }

    public void AddVerts(Vector3 corner, Vector3 up, Vector3 right, List<Vector3> verts, List<Vector3> vertsCollider)
    {
        verts.Add(corner);
        verts.Add(corner + up);
        verts.Add(corner + up + right);
        verts.Add(corner + right);

        vertsCollider.Add(corner);
        vertsCollider.Add(corner + up);
        vertsCollider.Add(corner + up + right);
        vertsCollider.Add(corner + right);
    }

    public override void AddUVs(BlockBean blockData, List<Vector2> uvs)
    {

    }

    public void AddUVs(DirectionEnum direction, BlockBean blockData, List<Vector2> uvs)
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
        uvs.Add(uvStartPosition);
        uvs.Add(uvStartPosition + new Vector2(0, uvWidth));
        uvs.Add(uvStartPosition + new Vector2(uvWidth, uvWidth));
        uvs.Add(uvStartPosition + new Vector2(uvWidth, 0));
    }
    public override void AddTris(int index, List<int> tris, int indexCollider, List<int> trisCollider)
    {

    }

    public void AddTris(int index, int indexCollider, bool reversed, List<int> tris, List<int> trisCollider)
    {
        if (reversed)
        {
            tris.Add(index + 0);
            tris.Add(index + 1);
            tris.Add(index + 2);

            tris.Add(index + 0);
            tris.Add(index + 2);
            tris.Add(index + 3);

            trisCollider.Add(indexCollider + 0);
            trisCollider.Add(indexCollider + 1);
            trisCollider.Add(indexCollider + 2);

            trisCollider.Add(indexCollider + 0);
            trisCollider.Add(indexCollider + 2);
            trisCollider.Add(indexCollider + 3);
        }
        else
        {
            tris.Add(index + 0);
            tris.Add(index + 2);
            tris.Add(index + 1);

            tris.Add(index + 0);
            tris.Add(index + 3);
            tris.Add(index + 2);

            trisCollider.Add(indexCollider + 0);
            trisCollider.Add(indexCollider + 2);
            trisCollider.Add(indexCollider + 1);

            trisCollider.Add(indexCollider + 0);
            trisCollider.Add(indexCollider + 3);
            trisCollider.Add(indexCollider + 2);
        }
    }

}