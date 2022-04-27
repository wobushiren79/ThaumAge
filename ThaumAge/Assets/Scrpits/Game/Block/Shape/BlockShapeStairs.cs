using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BlockShapeStairs : BlockShapeCube
{
    public static Vector3[] vertsAddLeftStairs = new Vector3[]
    {
            new Vector3(0,0,0),
            new Vector3(0,0,1),
            new Vector3(0,1,1),
            new Vector3(0,1,0.5f),
            new Vector3(0,0.5f,0.5f),
            new Vector3(0,0.5f,0)
    };
    public static Vector3[] vertsAddRightStairs = new Vector3[]
    {
            new Vector3(1,1,1),
            new Vector3(1,0,1),
            new Vector3(1,0,0),
            new Vector3(1,0.5f,0),
            new Vector3(1,0.5f,0.5f),
            new Vector3(1,1,0.5f)
    };
    public static Vector3[] vertsAddDownStairs = new Vector3[]
    {
            new Vector3(1,0,1),
            new Vector3(0,0,1),
            new Vector3(0,0,0),
            new Vector3(1,0,0)
    };
    public static Vector3[] vertsAddUpStairs = new Vector3[]
    {
            new Vector3(0,1,0.5f),
            new Vector3(0,1,1),
            new Vector3(1,1,1),
            new Vector3(1,1,0.5f)
    };
    public static Vector3[] vertsAddForwardStairs = new Vector3[]
    {
            new Vector3(0,0,0),
            new Vector3(0,0.5f,0),
            new Vector3(1,0.5f,0),
            new Vector3(1,0,0)
    };
    public static Vector3[] vertsAddBackStairs = new Vector3[]
    {
            new Vector3(1,1,1),
            new Vector3(0,1,1),
            new Vector3(0,0,1),
            new Vector3(1,0,1)
    };

    public static int[] trisAddStairs1 = new int[]
    {
        0,1,5, 1,4,5, 1,3,4, 1,2,3
    };

    public static int[] trisAddStairsCommon = new int[]
    {
        0,1,4,0,4,5,
        1,2,3,1,3,4
    };

    public static Color[] colorsAdd1 = new Color[]
    {
        Color.white,  Color.white,  Color.white,  Color.white,  Color.white,  Color.white
    };

    public static Color[] colorsAddCommon = new Color[]
    {
            Color.white,
            Color.white,
            Color.white,
            Color.white,
            Color.white,
            Color.white
    };

    public BlockShapeStairs() : base()
    {
        //添加固有的面
        vertsAdd = new Vector3[]
        {
            new Vector3(0f,0.5f,0f),
            new Vector3(0f,0.5f,0.5f),
            new Vector3(0f,1f,0.5f),
            new Vector3(1f,1f,0.5f),
            new Vector3(1f,0.5f,0.5f),
            new Vector3(1f,0.5f,0f)
        };
        colorsAdd = new Color[]
        {
            Color.white,
            Color.white,
            Color.white,
            Color.white,
            Color.white,
            Color.white,
        };
    }

    public override void InitData(Block block)
    {
        base.InitData(block);
        Vector2 uvStart = GetUVStartPosition(block, DirectionEnum.Left);

        uvsAddLeft = new Vector2[]
        {
            new Vector2(uvStart.x,uvStart.y),
            new Vector2(uvStart.x,uvStart.y + uvWidth),
            new Vector2(uvStart.x + uvWidth,uvStart.y + uvWidth),
            new Vector2(uvStart.x + uvWidth,uvStart.y + uvWidth/2),
            new Vector2(uvStart.x + uvWidth/2,uvStart.y + uvWidth/2),
            new Vector2(uvStart.x + uvWidth/2,uvStart.y)
        };

        uvStart = GetUVStartPosition(block, DirectionEnum.Right);
        uvsAddRight = new Vector2[]
        {
            new Vector2(uvStart.x + uvWidth,uvStart.y + uvWidth),
            new Vector2(uvStart.x,uvStart.y + uvWidth),
            new Vector2(uvStart.x,uvStart.y),
            new Vector2(uvStart.x + uvWidth/2,uvStart.y),
            new Vector2(uvStart.x + uvWidth/2,uvStart.y + uvWidth/2),
            new Vector2(uvStart.x + uvWidth,uvStart.y + uvWidth/2)
        };

        uvStart = GetUVStartPosition(block, DirectionEnum.Down);
        uvsAddDown = new Vector2[]
        {
            new Vector2(uvStart.x + uvWidth,uvStart.y + uvWidth),
            new Vector2(uvStart.x,uvStart.y + uvWidth),
            new Vector2(uvStart.x,uvStart.y),
            new Vector2(uvStart.x + uvWidth,uvStart.y)
        };

        uvStart = GetUVStartPosition(block, DirectionEnum.UP);
        uvsAddUp = new Vector2[]
        {
            new Vector2(uvStart.x,uvStart.y),
            new Vector2(uvStart.x,uvStart.y + uvWidth),
            new Vector2(uvStart.x + uvWidth,uvStart.y + uvWidth),
            new Vector2(uvStart.x + uvWidth,uvStart.y)
        };

        uvStart = GetUVStartPosition(block, DirectionEnum.Forward);
        uvsAddForward = new Vector2[]
        {
            new Vector2(uvStart.x,uvStart.y),
            new Vector2(uvStart.x,uvStart.y + uvWidth),
            new Vector2(uvStart.x + uvWidth,uvStart.y + uvWidth),
            new Vector2(uvStart.x+ uvWidth,uvStart.y)
        };

        uvStart = GetUVStartPosition(block, DirectionEnum.Back);
        uvsAddBack = new Vector2[]
        {
            new Vector2(uvStart.x + uvWidth,uvStart.y + uvWidth),
            new Vector2(uvStart.x,uvStart.y + uvWidth),
            new Vector2(uvStart.x,uvStart.y),
            new Vector2(uvStart.x+ uvWidth,uvStart.y)
        };

        uvsAdd = new Vector2[]
        {
            new Vector2(uvStart.x,uvStart.y),
            new Vector2(uvStart.x,uvStart.y + uvWidth/2),
            new Vector2(uvStart.x,uvStart.y + uvWidth),
            new Vector2(uvStart.x + uvWidth,uvStart.y + uvWidth),
            new Vector2(uvStart.x + uvWidth,uvStart.y + uvWidth/2),
            new Vector2(uvStart.x + uvWidth,uvStart.y )
        };

        colorsAdd = new Color[]
        {
            Color.white,
            Color.white,
            Color.white,
            Color.white
        };
    }

    public override void BuildBlock(Chunk chunk, Vector3Int localPosition)
    {
        if (block.blockType != BlockTypeEnum.None)
        {
            //只有在能旋转的时候才去查询旋转方向
            BlockDirectionEnum direction = BlockDirectionEnum.UpForward;
            if (block.blockInfo.rotate_state != 0)
            {
                direction = chunk.chunkData.GetBlockDirection(localPosition.x, localPosition.y, localPosition.z);
            }
            //Left
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Left))
            {
                BuildFace(chunk, localPosition, direction, DirectionEnum.Left, vertsAddLeftStairs, uvsAddLeft, colorsAdd1, trisAddStairs1);
            }

            //Right
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Right))
            {
                BuildFace(chunk, localPosition, direction, DirectionEnum.Right, vertsAddRightStairs, uvsAddRight, colorsAdd1, trisAddStairs1);
            }

            //Bottom
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Down))
            {
                BuildFace(chunk, localPosition, direction, DirectionEnum.Down, vertsAddDownStairs, uvsAddDown, colorsAdd, trisAdd);
            }

            //Top
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.UP))
            {
                BuildFace(chunk, localPosition, direction, DirectionEnum.UP, vertsAddUpStairs, uvsAddUp, colorsAdd, trisAdd);
            }

            //Forward
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Forward))
            {
                BuildFace(chunk, localPosition, direction, DirectionEnum.Forward, vertsAddForwardStairs, uvsAddForward, colorsAdd, trisAdd);
            }

            //Back
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Back))
            {
                BuildFace(chunk, localPosition, direction, DirectionEnum.Back, vertsAddBack, uvsAddBack, colorsAdd, trisAdd);
            }

            BuildFace(chunk, localPosition, direction, DirectionEnum.Left, vertsAdd, uvsAdd, colorsAddCommon, trisAddStairsCommon);
        }
    }

    public override Mesh GetCompleteMeshData()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertsAddLeftStairs
            .Concat(vertsAddRightStairs)
            .Concat(vertsAddUpStairs)
            .Concat(vertsAddDownStairs)
            .Concat(vertsAddForwardStairs)
            .Concat(vertsAddBackStairs)
            .Concat(vertsAdd)
            .ToArray();
        mesh.triangles = trisAddStairs1
            .Concat(trisAddStairs1)
            .Concat(trisAdd)
            .Concat(trisAdd)
            .Concat(trisAdd)
            .Concat(trisAdd)
            .Concat(trisAddStairsCommon)
            .ToArray();
        mesh.uv = uvsAddLeft
             .Concat(uvsAddRight)
             .Concat(uvsAddUp)
             .Concat(uvsAddDown)
             .Concat(uvsAddForward)
             .Concat(uvsAddBack)
             .Concat(uvsAdd)
             .ToArray();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        return mesh;
    }
}