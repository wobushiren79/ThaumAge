using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class BlockShapeFace : BlockShape
{

    public BlockShapeFace() : base()
    {
        vertsAdd = new Vector3[]
        {
            new Vector3(0f,0.001f,0f),
            new Vector3(0f,0.001f,1f),
            new Vector3(1f,0.001f,1f),
            new Vector3(1f,0.001f,0f)
        };

        trisAdd = new int[]
        {
            0,1,2, 0,2,3
        };

        colorsAdd = new Color[]
        {
            Color.white,Color.white,Color.white,Color.white
        };

        vertsColliderAdd = new Vector3[]
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

        trisColliderAdd = new int[]
        {
            0,1,2, 0,2,3, 4,6,5, 4,7,6
        };
    }

    public override void InitData(Block block)
    {
        base.InitData(block);
        Vector2 uvStartPosition = GetUVStartPosition(block);

        uvsAdd = new Vector2[]
        {
            new Vector2(uvStartPosition.x,uvStartPosition.y),
            new Vector2(uvStartPosition.x,uvStartPosition.y + uvWidth),
            new Vector2(uvStartPosition.x + uvWidth,uvStartPosition.y + uvWidth),
            new Vector2(uvStartPosition.x + uvWidth,uvStartPosition.y)
        };
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