using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockCross : Block
{
    public override void BuildBlock(List<Vector3> verts, List<Vector2> uvs, List<int> tris,List<Vector3> vertsCollider, List<int> trisCollider)
    {
        BlockTypeEnum blockType = blockData.GetBlockType();
        if (blockType != BlockTypeEnum.None)
        {
            BuildFace(blockData, position, verts, uvs, tris, vertsCollider, trisCollider);
        }
    }
    public override void BuildFace(BlockBean blockData, Vector3 corner, List<Vector3> verts, List<Vector2> uvs, List<int> tris, List<Vector3> vertsCollider, List<int> trisCollider)
    {
        int index = verts.Count;
        int indexCollider = vertsCollider.Count;
        AddVerts(corner, verts, vertsCollider);
        AddUVs(blockData, uvs);
        AddTris(index, tris, indexCollider, trisCollider);
    }

    public override void AddTris(int index, List<int> tris, int indexCollider, List<int> trisCollider)
    {
        tris.Add(index + 0);
        tris.Add(index + 1);
        tris.Add(index + 2);

        tris.Add(index + 0);
        tris.Add(index + 2);
        tris.Add(index + 3);

        tris.Add(index + 0);
        tris.Add(index + 2);
        tris.Add(index + 1);

        tris.Add(index + 0);
        tris.Add(index + 3);
        tris.Add(index + 2);

        tris.Add(index + 4);
        tris.Add(index + 5);
        tris.Add(index + 6);

        tris.Add(index + 4);
        tris.Add(index + 6);
        tris.Add(index + 7);

        tris.Add(index + 4);
        tris.Add(index + 6);
        tris.Add(index + 5);

        tris.Add(index + 4);
        tris.Add(index + 7);
        tris.Add(index + 6);
    }

    public override void AddUVs(BlockBean blockData, List<Vector2> uvs)
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
        else
        {
            uvStartPosition = Vector2.zero;
        }
        uvs.Add(uvStartPosition);
        uvs.Add(uvStartPosition + new Vector2(0, uvWidth));
        uvs.Add(uvStartPosition + new Vector2(uvWidth, uvWidth));
        uvs.Add(uvStartPosition + new Vector2(uvWidth, 0));

        uvs.Add(uvStartPosition);
        uvs.Add(uvStartPosition + new Vector2(0, uvWidth));
        uvs.Add(uvStartPosition + new Vector2(uvWidth, uvWidth));
        uvs.Add(uvStartPosition + new Vector2(uvWidth, 0));
    }

    public override void AddVerts(Vector3 corner, List<Vector3> verts, List<Vector3> vertsCollider)
    {
        verts.Add(corner + new Vector3(0.5f, 0, 0));
        verts.Add(corner + new Vector3(0.5f, 1, 0));
        verts.Add(corner + new Vector3(0.5f, 1, 1));
        verts.Add(corner + new Vector3(0.5f, 0, 1));

        verts.Add(corner + new Vector3(0, 0, 0.5f));
        verts.Add(corner + new Vector3(0, 1, 0.5f));
        verts.Add(corner + new Vector3(1, 1, 0.5f));
        verts.Add(corner + new Vector3(1, 0, 0.5f));
    }


}