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
                BuildFace(blockType, position, Vector3.up, Vector3.forward, false, verts, uvs, tris);
            //Right
            if (CheckNeedBuildFace( position + new Vector3Int(1, 0, 0)))
                BuildFace(blockType, position + new Vector3Int(1, 0, 0), Vector3.up, Vector3.forward, true, verts, uvs, tris);

            //Bottom
            if (CheckNeedBuildFace(position + new Vector3Int(0, -1, 0)))
                BuildFace(blockType, position, Vector3.forward, Vector3.right, false, verts, uvs, tris);
            //Top
            if (CheckNeedBuildFace(position + new Vector3Int(0, 1, 0)))
                BuildFace(blockType, position + new Vector3Int(0, 1, 0), Vector3.forward, Vector3.right, true, verts, uvs, tris);

            //Front
            if (CheckNeedBuildFace(position + new Vector3Int(0, 0, -1)))
                BuildFace(blockType, position, Vector3.up, Vector3.right, true, verts, uvs, tris);
            //Back
            if (CheckNeedBuildFace( position + new Vector3Int(0, 0, 1)))
                BuildFace(blockType, position + new Vector3Int(0, 0, 1), Vector3.up, Vector3.right, false, verts, uvs, tris);
        }
    }

    /// <summary>
    /// 构建方块的面
    /// </summary>
    /// <param name="blockType"></param>
    /// <param name="corner"></param>
    /// <param name="up"></param>
    /// <param name="right"></param>
    /// <param name="reversed"></param>
    /// <param name="verts"></param>
    /// <param name="uvs"></param>
    /// <param name="tris"></param>
    public override void BuildFace(BlockTypeEnum blockType, Vector3 corner, Vector3 up, Vector3 right, bool reversed, List<Vector3> verts, List<Vector2> uvs, List<int> tris)
    {
        int index = verts.Count;

        verts.Add(corner);
        verts.Add(corner + up);
        verts.Add(corner + up + right);
        verts.Add(corner + right);

        Vector2 uvWidth = new Vector2(0.25f, 0.25f);
        Vector2 uvCorner = new Vector2(0.00f, 0.75f);

        uvCorner.x += (float)(blockType - 1) / 4;
        uvs.Add(uvCorner);
        uvs.Add(new Vector2(uvCorner.x, uvCorner.y + uvWidth.y));
        uvs.Add(new Vector2(uvCorner.x + uvWidth.x, uvCorner.y + uvWidth.y));
        uvs.Add(new Vector2(uvCorner.x + uvWidth.x, uvCorner.y));

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