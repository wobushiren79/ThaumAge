using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockCross : Block
{
    public override void BuildBlock(List<Vector3> verts, List<Vector2> uvs, List<int> tris, List<Vector3> vertsCollider, List<int> trisCollider, List<int> trisBothFace)
    {
        base.BuildBlock(verts, uvs, tris, vertsCollider, trisCollider, trisBothFace);

        BlockTypeEnum blockType = blockData.GetBlockType();
        if (blockType != BlockTypeEnum.None)
        {
            BuildFace(blockData, position, verts, uvs, tris, vertsCollider, trisCollider, trisBothFace);
        }
    }

    public override void AddTris(int index, List<int> tris, int indexCollider, List<int> trisCollider, List<int> trisBothFace)
    {
        base.AddTris(index, tris, indexCollider, trisCollider, trisBothFace);

        trisBothFace.Add(index + 0);
        trisBothFace.Add(index + 1);
        trisBothFace.Add(index + 2);

        trisBothFace.Add(index + 0);
        trisBothFace.Add(index + 2);
        trisBothFace.Add(index + 3);

        trisBothFace.Add(index + 4);
        trisBothFace.Add(index + 5);
        trisBothFace.Add(index + 6);

        trisBothFace.Add(index + 4);
        trisBothFace.Add(index + 6);
        trisBothFace.Add(index + 7);
    }

    public override void AddUVs(BlockBean blockData, List<Vector2> uvs)
    {
        base.AddUVs(blockData, uvs);

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
            //随机选一个
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
        base.AddVerts(corner, verts, vertsCollider);

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