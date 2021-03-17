using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class BlockCube : Block
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
    public override void BuildBlock(List<Vector3> verts, List<Vector2> uvs, List<int> tris)
    {
        BlockTypeEnum blockType = blockData.GetBlockType();
        if (blockType != BlockTypeEnum.None)
        {
            //Left
            if (CheckNeedBuildFace(position + new Vector3Int(-1, 0, 0)))
                BuildFace(blockData, position, Vector3.up, Vector3.forward, false, verts, uvs, tris);
            //Right
            if (CheckNeedBuildFace(position + new Vector3Int(1, 0, 0)))
                BuildFace(blockData, position + new Vector3Int(1, 0, 0), Vector3.up, Vector3.forward, true, verts, uvs, tris);

            //Bottom
            if (CheckNeedBuildFace(position + new Vector3Int(0, -1, 0)))
                BuildFace(blockData, position, Vector3.forward, Vector3.right, false, verts, uvs, tris);
            //Top
            if (CheckNeedBuildFace(position + new Vector3Int(0, 1, 0)))
                BuildFace(blockData, position + new Vector3Int(0, 1, 0), Vector3.forward, Vector3.right, true, verts, uvs, tris);

            //Front
            if (CheckNeedBuildFace(position + new Vector3Int(0, 0, -1)))
                BuildFace(blockData, position, Vector3.up, Vector3.right, true, verts, uvs, tris);
            //Back
            if (CheckNeedBuildFace(position + new Vector3Int(0, 0, 1)))
                BuildFace(blockData, position + new Vector3Int(0, 0, 1), Vector3.up, Vector3.right, false, verts, uvs, tris);
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
    public override void BuildFace(BlockBean blockData, Vector3 corner, Vector3 up, Vector3 right, bool reversed, List<Vector3> verts, List<Vector2> uvs, List<int> tris)
    {
        int index = verts.Count;
        AddVerts(corner, up, right, verts);
        AddUVs(blockData, uvs);
        AddTris(index, reversed, tris);
    }

    public override void AddVerts(Vector3 corner, Vector3 up, Vector3 right, List<Vector3> verts)
    {
        verts.Add(corner);
        verts.Add(corner + up);
        verts.Add(corner + up + right);
        verts.Add(corner + right);
    }

    public override void AddUVs(BlockBean blockData, List<Vector2> uvs)
    {
        float uvWidth = 1 / 128f;
        BlockInfoBean blockInfo = BlockHandler.Instance.manager.GetBlockInfo(blockData.GetBlockType());
        List<Vector2Int> listData = blockInfo.GetUVPosition();
        Vector2 uvStartPosition;
        if (CheckUtil.ListIsNull(listData))
        {
            uvStartPosition = Vector2.zero;
        }
        else if (listData.Count == 1)
        {
            uvStartPosition = new Vector2(uvWidth * listData[0].y,uvWidth * listData[0].x);
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

    public override void AddTris(int index, bool reversed, List<int> tris)
    {
        if (reversed)
        {
            tris.Add(index + 0);
            tris.Add(index + 1);
            tris.Add(index + 2);
            tris.Add(index + 0);
            tris.Add(index + 2);
            tris.Add(index + 3);
        }
        else
        {
            tris.Add(index + 0);
            tris.Add(index + 2);
            tris.Add(index + 1);
            tris.Add(index + 0);
            tris.Add(index + 3);
            tris.Add(index + 2);
        }
    }

}