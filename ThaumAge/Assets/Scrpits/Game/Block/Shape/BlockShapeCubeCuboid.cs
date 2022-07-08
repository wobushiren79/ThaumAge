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

        vertsAddLeftOffset = vertsAddLeft.AddX(leftOffsetBorder);
        vertsAddRightOffset = vertsAddLeft.AddX(rightOffsetBorder);
        vertsAddDownOffset = vertsAddDown.AddY(downOffsetBorder);
        vertsAddUpOffset = vertsAddUp.AddY(upOffsetBorder);
        vertsAddForwardOffset = vertsAddForward.AddZ(forwardOffsetBorder);
        vertsAddBackOffset = vertsAddBack.AddZ(backOffsetBorder);
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
    /// <returns></returns>
    protected override bool CheckNeedBuildFaceDef(Block closeBlock, Chunk closeBlockChunk, Vector3Int closeLocalPosition, DirectionEnum closeDirection)
    {
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

    public override Mesh GetCompleteMeshData(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
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
            .Concat(trisAdd.Add(4))
            .Concat(trisAdd.Add(8))
            .Concat(trisAdd.Add(12))
            .Concat(trisAdd.Add(16))
            .Concat(trisAdd.Add(20))
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