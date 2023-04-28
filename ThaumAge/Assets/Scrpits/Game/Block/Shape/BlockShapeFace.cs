using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class BlockShapeFace : BlockShape
{
    public static Vector3[] VertsAddFace = new Vector3[]
    {
            new Vector3(0f,0.001f,0f),
            new Vector3(0f,0.001f,1f),
            new Vector3(1f,0.001f,1f),
            new Vector3(1f,0.001f,0f)
    };

    public static int[] TrisAddFace = new int[]
    {
            0,1,2, 0,2,3
    };

    public static Color[] ColorAddFace = new Color[]
    {
            Color.white,Color.white,Color.white,Color.white
    };

    public static Vector3[] VertsColliderAddFace = new Vector3[]
    {
            new Vector3(0f,0.001f,0f),
            new Vector3(0f,0.001f,1f),
            new Vector3(1f,0.001f,1f),
            new Vector3(1f,0.001f,0f),

            new Vector3(0f,0.001f,0f),
            new Vector3(0f,0.001f,1f),
            new Vector3(1f,0.001f,1f),
            new Vector3(1f,0.001f,0f)
    };

    public static int[] TrisColliderAddFace = new int[]
    {
            0,1,2, 0,2,3, 4,6,5, 4,7,6
    };
    public BlockShapeFace(Block block) : base(block)
    {
        vertsAdd = VertsAddFace;

        trisAdd = TrisAddFace;

        colorsAdd = ColorAddFace;

        Vector2 uvStartPosition = GetUVStartPosition(block);

        uvsAdd = new Vector2[]
        {
            new Vector2(uvStartPosition.x,uvStartPosition.y),
            new Vector2(uvStartPosition.x,uvStartPosition.y + uvWidth),
            new Vector2(uvStartPosition.x + uvWidth,uvStartPosition.y + uvWidth),
            new Vector2(uvStartPosition.x + uvWidth,uvStartPosition.y)
        };
    }

    /// <summary>
    /// 构建面
    /// </summary>
    public override void BuildFace(
        Chunk chunk, Vector3Int localPosition, 
        Vector3[] vertsAdd, Vector2[] uvsAdd, Color[] colorsAdd, Vector3[] vertsColliderAdd, Vector3[] vertsTriggerAdd, 
        int[] trisAdd, int[] trisColliderAdd, int[] trisTriggerAdd)
    {
        base.BuildFace(chunk, localPosition, vertsAdd, uvsAdd, colorsAdd, VertsColliderAddFace, vertsTriggerAdd, trisAdd, TrisColliderAddFace, trisTriggerAdd);
    }

    public virtual Vector2 GetUVStartPosition(Block block)
    {
        Vector2Int[] arrayUVData = block.blockInfo.GetUVPosition();
        Vector2 uvStartPosition;
        if (arrayUVData.IsNull())
        {
            uvStartPosition = Vector2.zero;
        }
        else
        {
            //只有一种面
            uvStartPosition = new Vector2(uvWidth * arrayUVData[0].y, uvWidth * arrayUVData[0].x);
        }
        return uvStartPosition;
    }
}