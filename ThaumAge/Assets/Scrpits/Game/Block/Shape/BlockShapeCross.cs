using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockShapeCross : BlockShape
{
    public static Vector3[] VertsAddCross = new Vector3[]
    {
            new Vector3(0.5f,0f,0f),
            new Vector3(0.5f,1f,0f),
            new Vector3(0.5f,1f,1f),
            new Vector3(0.5f,0f,1f),

            new Vector3(0f,0f,0.5f),
            new Vector3(0f,1f,0.5f),
            new Vector3(1f,1f,0.5f),
            new Vector3(1f,0f,0.5f)
    };
    public static int[] TrisAddCross = new int[]
    {
            0,1,2, 0,2,3, 4,5,6, 4,6,7
    };

    public Vector3[] vertsColliderAddBuffer;
    public int[] trisColliderAddBuffer;

    public BlockShapeCross(Block block) : base(block)
    {
        vertsAdd = VertsAddCross;
        trisAdd = TrisAddCross;
        colorsAdd = new Color[8];

        InitVertsColliderAdd();
        InitTrisColliderAdd();
    }

    /// <summary>
    /// 初始化碰撞
    /// </summary>
    public virtual void InitVertsColliderAdd()
    {
        if (block.blockInfo.remark_string.IsNull())
        {
            vertsColliderAddBuffer = VertsColliderAddCube;
        }
        else
        {
            //用备注信息来设置高
            float heightRate = float.Parse(block.blockInfo.remark_string);
            vertsColliderAddBuffer = VertsColliderAddCube.MultiplyY(heightRate);
        }
    }

    /// <summary>
    /// 初始化碰撞
    /// </summary>
    public virtual void InitTrisColliderAdd()
    {
        trisColliderAddBuffer = TrisColliderAddCube;
    }

    public override void BuildFace(Chunk chunk, Vector3Int localPosition, Vector3[] vertsAdd, Vector2[] uvsAdd, Color[] colorsAdd, Vector3[] vertsColliderAdd, Vector3[] vertsTriggerAdd, int[] trisAdd, int[] trisColliderAdd, int[] trisTriggerAdd)
    {
        base.BuildFace(chunk, localPosition, vertsAdd, uvsAdd, colorsAdd, vertsColliderAddBuffer, vertsColliderAddBuffer, trisAdd, trisColliderAddBuffer, trisColliderAddBuffer);
    }

    public override void InitData()
    {
        base.InitData();

        Vector2 uvStartPosition = GetUVStartPosition(block);
        uvsAdd = new Vector2[]
        {
            new Vector2(uvStartPosition.x,uvStartPosition.y),
            new Vector2(uvStartPosition.x,uvStartPosition.y + uvWidth),
            new Vector2(uvStartPosition.x + uvWidth,uvStartPosition.y + uvWidth),
            new Vector2(uvStartPosition.x + uvWidth,uvStartPosition.y),

            new Vector2(uvStartPosition.x,uvStartPosition.y),
            new Vector2(uvStartPosition.x,uvStartPosition.y + uvWidth),
            new Vector2(uvStartPosition.x + uvWidth,uvStartPosition.y + uvWidth),
            new Vector2(uvStartPosition.x + uvWidth,uvStartPosition.y)
        };
    }

    public static Vector2 GetUVStartPosition(Block block)
    {
        Vector2Int[] arrayUVData = block.blockInfo.GetUVPosition();
        Vector2 uvStartPosition;
        if (arrayUVData.IsNull())
        {
            uvStartPosition = Vector2.zero;
        }
        else if (arrayUVData.Length == 1)
        {
            //只有一种面
            uvStartPosition = new Vector2(uvWidth * arrayUVData[0].y, uvWidth * arrayUVData[0].x);
        }
        else
        {
            //随机选一个
            uvStartPosition = new Vector2(uvWidth * arrayUVData[0].y, uvWidth * arrayUVData[0].x);
        }
        return uvStartPosition;
    }

    /// <summary>
    /// 重新选中的方块预览 方块
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    public override Mesh GetSelectMeshData(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertsColliderAddBuffer;
        mesh.triangles = trisColliderAddBuffer;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        return mesh;
    }
}