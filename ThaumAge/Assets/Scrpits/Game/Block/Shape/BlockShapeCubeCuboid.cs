using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BlockShapeCubeCuboid : BlockShapeCube
{
    //长方形方块所用数据
    public float leftOffsetBorder;
    public float rightOffsetBorder;
    public float downOffsetBorder;
    public float upOffsetBorder;
    public float forwardOffsetBorder;
    public float backOffsetBorder;

    public Vector3[] vertsAddLeftOffset;
    public Vector3[] vertsAddRightOffset;
    public Vector3[] vertsAddDownOffset;
    public Vector3[] vertsAddUpOffset;
    public Vector3[] vertsAddForwardOffset;
    public Vector3[] vertsAddBackOffset;

    public override void InitData(Block block)
    {
        base.InitData(block);

        float[] offsetBorder = block.blockInfo.GetOffsetBorder();

        leftOffsetBorder = offsetBorder[0];
        rightOffsetBorder = offsetBorder[1];
        upOffsetBorder = offsetBorder[2];
        downOffsetBorder = offsetBorder[3];
        forwardOffsetBorder = offsetBorder[4];
        backOffsetBorder = offsetBorder[5];

        vertsAddLeftOffset = new Vector3[vertsAddLeft.Length];
        vertsAddRightOffset = new Vector3[vertsAddRight.Length];
        vertsAddDownOffset = new Vector3[vertsAddDown.Length];
        vertsAddUpOffset = new Vector3[vertsAddUp.Length];
        vertsAddForwardOffset = new Vector3[vertsAddForward.Length];
        vertsAddBackOffset = new Vector3[vertsAddBack.Length];

        for (int i = 0; i < vertsAddLeft.Length; i++)
        {
            vertsAddLeftOffset[i] = vertsAddLeft[i].AddX(leftOffsetBorder);
        }
        for (int i = 0; i < vertsAddRight.Length; i++)
        {
            vertsAddRightOffset[i] = vertsAddRight[i].AddX(rightOffsetBorder);
        }
        for (int i = 0; i < vertsAddUp.Length; i++)
        {
            vertsAddUpOffset[i] = vertsAddUp[i].AddY(upOffsetBorder);
        }
        for (int i = 0; i < vertsAddDown.Length; i++)
        {
            vertsAddDownOffset[i] = vertsAddDown[i].AddY(downOffsetBorder);
        }
        for (int i = 0; i < vertsAddForward.Length; i++)
        {
            vertsAddForwardOffset[i] = vertsAddForward[i].AddZ(forwardOffsetBorder);
        }
        for (int i = 0; i < vertsAddBack.Length; i++)
        {
            vertsAddBackOffset[i] = vertsAddBack[i].AddZ(backOffsetBorder);
        }
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
            //Left
            if (leftOffsetBorder == 0 ? CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Left) : true)
            {
                BuildFace(chunk, localPosition, direction, DirectionEnum.Left, vertsAddLeftOffset, uvsAddLeft, colorsAdd, trisAdd);
            }
            //Right
            if (rightOffsetBorder == 0 ? CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Right) : true)
            {
                BuildFace(chunk, localPosition, direction, DirectionEnum.Right, vertsAddRightOffset, uvsAddRight, colorsAdd, trisAdd);
            }
            //Bottom
            if (downOffsetBorder == 0 ? CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Down) : true)
            {
                BuildFace(chunk, localPosition, direction, DirectionEnum.Down, vertsAddDownOffset, uvsAddDown, colorsAdd, trisAdd);
            }
            //Top
            if (upOffsetBorder == 0 ? CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.UP) : true)
            {
                BuildFace(chunk, localPosition, direction, DirectionEnum.UP, vertsAddUpOffset, uvsAddUp, colorsAdd, trisAdd);
            }
            //Front
            if (forwardOffsetBorder == 0 ? CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Forward) : true)
            {
                BuildFace(chunk, localPosition, direction, DirectionEnum.Forward, vertsAddForwardOffset, uvsAddForward, colorsAdd, trisAdd);
            }
            //Back
            if (backOffsetBorder == 0 ? CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Back) : true)
            {
                BuildFace(chunk, localPosition, direction, DirectionEnum.Back, vertsAddBackOffset, uvsAddBack, colorsAdd, trisAdd);
            }
        }
    }


    /// <summary>
    /// 检测是否需要构建面
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    /// <param name="direction"></param>
    /// <param name="closeDirection"></param>
    /// <returns></returns>
    public override bool CheckNeedBuildFace(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, DirectionEnum closeDirection)
    {
        if (localPosition.y == 0) return false;
        GetCloseRotateBlockByDirection(chunk, localPosition, direction, closeDirection, out Block closeBlock, out Chunk closeBlockChunk, out Vector3Int closeLocalPosition);
        if (closeBlock == null || closeBlock.blockType == BlockTypeEnum.None)
        {
            if (closeBlockChunk != null && closeBlockChunk.isInit)
            {
                //只是空气方块
                return true;
            }
            else
            {
                //还没有生成chunk
                return false;
            }
        }
        BlockShapeEnum blockShape = closeBlock.blockInfo.GetBlockShape();
        switch (blockShape)
        {
            case BlockShapeEnum.Cube:
            case BlockShapeEnum.CubeCuboid:
                return false;
            default:
                return true;
        }
    }

    public override Mesh GetCompleteMeshData()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertsAddLeftOffset
            .Concat(vertsAddRightOffset)
            .Concat(vertsAddUpOffset)
            .Concat(vertsAddDownOffset)
            .Concat(vertsAddForwardOffset)
            .Concat(vertsAddBackOffset)
            .ToArray();
        mesh.triangles = trisAdd
            .Concat(trisAdd)
            .Concat(trisAdd)
            .Concat(trisAdd)
            .Concat(trisAdd)
            .Concat(trisAdd)
            .ToArray();
        mesh.uv = uvsAddLeft
             .Concat(uvsAddRight)
             .Concat(uvsAddUp)
             .Concat(uvsAddDown)
             .Concat(uvsAddForward)
             .Concat(uvsAddBack)
             .ToArray();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        return mesh;
    }
}