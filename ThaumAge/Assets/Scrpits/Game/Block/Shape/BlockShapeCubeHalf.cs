using UnityEditor;
using UnityEngine;

public class BlockShapeCubeHalf : BlockShapeCube
{
    protected static Vector3[] HandleHalfVerts(Vector3[] verts, Vector3 offsetPosition)
    {
        Vector3[] newVerts = new Vector3[verts.Length];
        for (int i = 0; i < verts.Length; i++)
        {
            Vector3 itemVert = verts[i];
            float newX = offsetPosition.x == 0 ? itemVert.x : (itemVert.x > 0.5f ? itemVert.x - offsetPosition.x : itemVert.x);
            float newY = offsetPosition.y == 0 ? itemVert.y : (itemVert.y > 0.5f ? itemVert.y - offsetPosition.y : itemVert.y);
            float newZ = offsetPosition.z == 0 ? itemVert.z : (itemVert.z > 0.5f ? itemVert.z - offsetPosition.z : itemVert.z);
            newVerts[i] = new Vector3(newX, newY, newZ);
        }
        return newVerts;
    }

    public static Vector3[][] vertsAddHalfUp = new Vector3[][]
    {
           vertsAddLeft,
           vertsAddRight,
           vertsAddUp,
           vertsAddDown,
           vertsAddForward,
           vertsAddBack,
    };

    public static Vector3[][] vertsAddHalfDown = new Vector3[][]
    {
           vertsAddLeft,
           vertsAddRight,
           vertsAddUp,
           vertsAddDown,
           vertsAddForward,
           vertsAddBack,
    };

    public static Vector3[] vertsAddHalfFaceLeft = new Vector3[]
    {
            new Vector3(0.5f,1,1),
            new Vector3(0.5f,1,0),
            new Vector3(0.5f,0,0),
            new Vector3(0.5f,0,1)
    };

    public static Vector3[] vertsAddHalfFaceRight = new Vector3[]
    {
            new Vector3(0.5f,0,0),
            new Vector3(0.5f,1,0),
            new Vector3(0.5f,1,1),
            new Vector3(0.5f,0,1)
    };

    public static Vector3[] vertsAddHalfFaceForward = new Vector3[]
    {
            new Vector3(0,0,0.5f),
            new Vector3(0,1,0.5f),
            new Vector3(1,1,0.5f),
            new Vector3(1,0,0.5f)
    };

    public static Vector3[] vertsAddHalfFaceBack = new Vector3[]
    {
            new Vector3(1,1,0.5f),
            new Vector3(0,1,0.5f),
            new Vector3(0,0,0.5f),
            new Vector3(1,0,0.5f)
    };

    public override void InitData(Block block)
    {
        base.InitData(block);
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
            BlockBean blockData = chunk.GetBlockData(localPosition);
            BlockMetaCubeHalf blockMeta = blockData.GetBlockMeta<BlockMetaCubeHalf>();
            DirectionEnum halfPosition = blockMeta.GetHalfPosition();

            Vector3[] useVertsAddLeft = vertsAddLeft;
            Vector3[] useVertsAddRight = vertsAddRight;
            Vector3[] useVertsAddDown = vertsAddDown;
            Vector3[] useVertsAddUp = vertsAddUp;
            Vector3[] useVertsAddForward = vertsAddForward;
            Vector3[] useVertsAddBack = vertsAddBack;
            switch (halfPosition)
            {
                case DirectionEnum.UP:
                    break;
                case DirectionEnum.Down:
                    break;
                case DirectionEnum.Left:
                    break;
                case DirectionEnum.Right:
                    break;
                case DirectionEnum.Forward:
                    break;
                case DirectionEnum.Back:
                    break;
            }
            //Left
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Left))
            {
                BuildFace(block, chunk, localPosition, direction, DirectionEnum.Left, useVertsAddLeft, uvsAddLeft);
            }

            //Right
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Right))
            {
                BuildFace(block, chunk, localPosition, direction, DirectionEnum.Right, useVertsAddRight, uvsAddRight);
            }

            //Bottom
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Down))
            {
                BuildFace(block, chunk, localPosition, direction, DirectionEnum.Down, useVertsAddDown, uvsAddDown);
            }

            //Top
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.UP))
            {
                BuildFace(block, chunk, localPosition, direction, DirectionEnum.UP, useVertsAddUp, uvsAddUp);
            }

            //Forward
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Forward))
            {
                BuildFace(block, chunk, localPosition, direction, DirectionEnum.Forward, useVertsAddForward, uvsAddForward);
            }

            //Back
            if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Back))
            {
                BuildFace(block, chunk, localPosition, direction, DirectionEnum.Back, useVertsAddBack, uvsAddBack);
            }
        }
    }
}