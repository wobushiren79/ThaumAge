﻿using UnityEditor;
using UnityEngine;

public class BlockShapeWell : BlockShapeCross
{
    protected float leftOffsetBorder;
    protected float rightOffsetBorder;
    protected float forwardOffsetBorder;
    protected float backOffsetBorder;

    public int[] TrisAddWell = new int[]
    {
            0,1,2, 0,2,3,
            4,5,6, 4,6,7,
            8,9,10, 8,10,11,
            12,13,14, 12,14,15
    };

    public BlockShapeWell(Block block) : base(block)
    {
        trisAdd = TrisAddWell;

        Vector2 uvStart = GetUVStartPosition(block);

        float[] offsetBorder = block.blockInfo.GetOffsetBorder();

        leftOffsetBorder = offsetBorder[0];
        rightOffsetBorder = offsetBorder[1];
        forwardOffsetBorder = offsetBorder[2];
        backOffsetBorder = offsetBorder[3];

        vertsAdd = new Vector3[]
        {
            new Vector3(0,1,1).AddX(leftOffsetBorder),
            new Vector3(0,1,0).AddX(leftOffsetBorder),
            new Vector3(0,0,0).AddX(leftOffsetBorder),
            new Vector3(0,0,1).AddX(leftOffsetBorder),

            new Vector3(1,0,0).AddX(rightOffsetBorder),
            new Vector3(1,1,0).AddX(rightOffsetBorder),
            new Vector3(1,1,1).AddX(rightOffsetBorder),
            new Vector3(1,0,1).AddX(rightOffsetBorder),

            new Vector3(0,0,0).AddZ(forwardOffsetBorder),
            new Vector3(0,1,0).AddZ(forwardOffsetBorder),
            new Vector3(1,1,0).AddZ(forwardOffsetBorder),
            new Vector3(1,0,0).AddZ(forwardOffsetBorder),

            new Vector3(1,1,1).AddZ(backOffsetBorder),
            new Vector3(0,1,1).AddZ(backOffsetBorder),
            new Vector3(0,0,1).AddZ(backOffsetBorder),
            new Vector3(1,0,1).AddZ(backOffsetBorder)
        };

        uvsAdd = new Vector2[]
        {
            new Vector2(uvStart.x ,uvStart.y + uvWidth),
            new Vector2(uvStart.x + uvWidth,uvStart.y + uvWidth),
            new Vector2(uvStart.x + uvWidth ,uvStart.y),
            new Vector2(uvStart.x ,uvStart.y),

            new Vector2(uvStart.x,uvStart.y),
            new Vector2(uvStart.x,uvStart.y + uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y+ uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y),

            new Vector2(uvStart.x,uvStart.y),
            new Vector2(uvStart.x,uvStart.y + uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y+ uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y),

            new Vector2(uvStart.x ,uvStart.y + uvWidth),
            new Vector2(uvStart.x + uvWidth,uvStart.y + uvWidth),
            new Vector2(uvStart.x + uvWidth ,uvStart.y),
            new Vector2(uvStart.x ,uvStart.y)
        };

        colorsAdd = new Color[] 
        {
            Color.white,Color.white,Color.white,Color.white,
            Color.white,Color.white,Color.white,Color.white,
            Color.white,Color.white,Color.white,Color.white,
            Color.white,Color.white,Color.white,Color.white
        };
    }
}