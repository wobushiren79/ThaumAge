using UnityEditor;
using UnityEngine;

public class BlockShapeCubeHalf : BlockShapeCube
{
    protected static float offsetCubeHalf = 0.5f;
    protected static Vector3[] HandleHalfVerts(Vector3[] verts, Vector3 offsetPosition)
    {
        Vector3[] newVerts = new Vector3[verts.Length];
        for (int i = 0; i < verts.Length; i++)
        {
            Vector3 itemVert = verts[i];
            float newX = offsetPosition.x == 0 
                ? itemVert.x :(offsetPosition.x > 0 
                ? (itemVert.x < 0.5f ? itemVert.x + offsetPosition.x : itemVert.x) : (itemVert.x > 0.5f ? itemVert.x + offsetPosition.x : itemVert.x));
            float newY = offsetPosition.y == 0
                ? itemVert.y : (offsetPosition.y > 0
                ? (itemVert.y < 0.5f ? itemVert.y + offsetPosition.y : itemVert.y) : (itemVert.y > 0.5f ? itemVert.y + offsetPosition.y : itemVert.y));
            float newZ = offsetPosition.z == 0
                ? itemVert.z : (offsetPosition.z > 0
                ? (itemVert.z < 0.5f ? itemVert.z + offsetPosition.z : itemVert.z) : (itemVert.z > 0.5f ? itemVert.z + offsetPosition.z : itemVert.z));
            newVerts[i] = new Vector3(newX, newY, newZ);
        }
        return newVerts;
    }

    public static Vector3[][] vertsAddHalfUp = new Vector3[][]
    {
           HandleHalfVerts(vertsAddLeft,new Vector3(0,offsetCubeHalf,0)),
           HandleHalfVerts(vertsAddRight,new Vector3(0,offsetCubeHalf,0)),
           HandleHalfVerts(vertsAddUp,new Vector3(0,offsetCubeHalf,0)),
           HandleHalfVerts(vertsAddDown,new Vector3(0,offsetCubeHalf,0)),
           HandleHalfVerts(vertsAddForward,new Vector3(0,offsetCubeHalf,0)),
           HandleHalfVerts(vertsAddBack,new Vector3(0,offsetCubeHalf,0))
    };

    public static Vector3[][] vertsAddHalfDown = new Vector3[][]
    {
           HandleHalfVerts(vertsAddLeft,new Vector3(0,-offsetCubeHalf,0)),
           HandleHalfVerts(vertsAddRight,new Vector3(0,-offsetCubeHalf,0)),
           HandleHalfVerts(vertsAddUp,new Vector3(0,-offsetCubeHalf,0)),
           HandleHalfVerts(vertsAddDown,new Vector3(0,-offsetCubeHalf,0)),
           HandleHalfVerts(vertsAddForward,new Vector3(0,-offsetCubeHalf,0)),
           HandleHalfVerts(vertsAddBack,new Vector3(0,-offsetCubeHalf,0))
    };

    public static Vector3[][] vertsAddHalfLeft = new Vector3[][]
    {
           HandleHalfVerts(vertsAddLeft,new Vector3(-offsetCubeHalf,0,0)),
           HandleHalfVerts(vertsAddRight,new Vector3(-offsetCubeHalf,0,0)),
           HandleHalfVerts(vertsAddUp,new Vector3(-offsetCubeHalf,0,0)),
           HandleHalfVerts(vertsAddDown,new Vector3(-offsetCubeHalf,0,0)),
           HandleHalfVerts(vertsAddForward,new Vector3(-offsetCubeHalf,0,0)),
           HandleHalfVerts(vertsAddBack,new Vector3(-offsetCubeHalf,0,0))
    };

    public static Vector3[][] vertsAddHalfRight = new Vector3[][]
    {
           HandleHalfVerts(vertsAddLeft,new Vector3(offsetCubeHalf,0,0)),
           HandleHalfVerts(vertsAddRight,new Vector3(offsetCubeHalf,0,0)),
           HandleHalfVerts(vertsAddUp,new Vector3(offsetCubeHalf,0,0)),
           HandleHalfVerts(vertsAddDown,new Vector3(offsetCubeHalf,0,0)),
           HandleHalfVerts(vertsAddForward,new Vector3(offsetCubeHalf,0,0)),
           HandleHalfVerts(vertsAddBack,new Vector3(offsetCubeHalf,0,0))
    };

    public static Vector3[][] vertsAddHalfForward = new Vector3[][]
    {
           HandleHalfVerts(vertsAddLeft,new Vector3(0,0,-offsetCubeHalf)),
           HandleHalfVerts(vertsAddRight,new Vector3(0,0,-offsetCubeHalf)),
           HandleHalfVerts(vertsAddUp,new Vector3(0,0,-offsetCubeHalf)),
           HandleHalfVerts(vertsAddDown,new Vector3(0,0,-offsetCubeHalf)),
           HandleHalfVerts(vertsAddForward,new Vector3(0,0,-offsetCubeHalf)),
           HandleHalfVerts(vertsAddBack,new Vector3(0,0,-offsetCubeHalf))
    };

    public static Vector3[][] vertsAddHalfBack = new Vector3[][]
    {
           HandleHalfVerts(vertsAddLeft,new Vector3(0,0,offsetCubeHalf)),
           HandleHalfVerts(vertsAddRight,new Vector3(0,0,offsetCubeHalf)),
           HandleHalfVerts(vertsAddUp,new Vector3(0,0,offsetCubeHalf)),
           HandleHalfVerts(vertsAddDown,new Vector3(0,0,offsetCubeHalf)),
           HandleHalfVerts(vertsAddForward,new Vector3(0,0,offsetCubeHalf)),
           HandleHalfVerts(vertsAddBack,new Vector3(0,0,offsetCubeHalf))
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

            Vector3[][] arrayData = null;
            switch (halfPosition)
            {
                case DirectionEnum.UP:
                    arrayData = vertsAddHalfUp;
                    break;
                case DirectionEnum.Down:
                    arrayData = vertsAddHalfDown;
                    break;
                case DirectionEnum.Left:
                    arrayData = vertsAddHalfLeft;
                    break;
                case DirectionEnum.Right:
                    arrayData = vertsAddHalfRight;
                    break;
                case DirectionEnum.Forward:
                    arrayData = vertsAddHalfForward;
                    break;
                case DirectionEnum.Back:
                    arrayData = vertsAddHalfBack;
                    break;
            }

            Vector3[] useVertsAddLeft = arrayData[0];
            Vector3[] useVertsAddRight = arrayData[1];
            Vector3[] useVertsAddUp = arrayData[2];
            Vector3[] useVertsAddDown = arrayData[3];
            Vector3[] useVertsAddForward = arrayData[4];
            Vector3[] useVertsAddBack = arrayData[5];

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

    public override bool CheckNeedBuildFace(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, DirectionEnum closeDirection)
    {
        return true;
    }
}