using UnityEditor;
using UnityEngine;

public class BlockShapeCubeTransparent : BlockShapeCube
{

    //public override void BuildBlock(Chunk chunk, Vector3Int localPosition)
    //{
    //    if (block.blockType != BlockTypeEnum.None)
    //    {
    //        //只有在能旋转的时候才去查询旋转方向
    //        BlockDirectionEnum direction = BlockDirectionEnum.UpForward;
    //        if (block.blockInfo.rotate_state != 0)
    //        {
    //            direction = chunk.chunkData.GetBlockDirection(localPosition.x, localPosition.y, localPosition.z);
    //        }

    //        //Left
    //        if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Left))
    //        {
    //            BuildFace(chunk, localPosition, direction, DirectionEnum.Left, vertsAddLeft, uvsAddLeft, colorsAdd, trisAdd);
    //        }

    //        //Right
    //        if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Right))
    //        {
    //            BuildFace(chunk, localPosition, direction, DirectionEnum.Right, vertsAddRight, uvsAddRight, colorsAdd, trisAdd);
    //        }

    //        //Bottom
    //        if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Down))
    //        {
    //            BuildFace(chunk, localPosition, direction, DirectionEnum.Down, vertsAddDown, uvsAddDown, colorsAdd, trisAdd);
    //        }

    //        //Top
    //        if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.UP))
    //        {
    //            BuildFace(chunk, localPosition, direction, DirectionEnum.UP, vertsAddUp, uvsAddUp, colorsAdd, trisAdd);
    //        }

    //        //Forward
    //        if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Forward))
    //        {
    //            BuildFace(chunk, localPosition, direction, DirectionEnum.Forward, vertsAddForward, uvsAddForward, colorsAdd, trisAdd);
    //        }

    //        //Back
    //        if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Back))
    //        {
    //            BuildFace(chunk, localPosition, direction, DirectionEnum.Back, vertsAddBack, uvsAddBack, colorsAdd, trisAdd);
    //        }
    //    }
    //}

    /// <summary>
    /// 检测是否生成面
    /// </summary>
    /// <returns></returns>
    protected override bool CheckNeedBuildFaceDef(Block closeBlock, Chunk closeBlockChunk, Vector3Int closeLocalPosition, DirectionEnum closeDirection)
    {
        BlockShapeEnum blockShape = closeBlock.blockInfo.GetBlockShape();
        switch (blockShape)
        {
            case BlockShapeEnum.Cube:
            case BlockShapeEnum.CubeTransparent:
                return false;
            default:
                return true;
        }
    }

    //public void GetUVsAdd(Chunk chunk, Vector3Int localPosition,
    //    out Vector2[] uvsAddLeft, out Vector2[] uvsAddRight, out Vector2[] uvsAddUp,
    //    out Vector2[] uvsAddDown, out Vector2[] uvsAddForward, out Vector2[] uvsAddBack)
    //{

    //    //获取上下左右6个面的方块
    //    block.GetCloseBlockByDirection(chunk, localPosition, DirectionEnum.Left,
    //        out Block leftBlock, out Chunk leftBlockChunk, out Vector3Int leftLocalPosition);
    //    block.GetCloseBlockByDirection(chunk, localPosition, DirectionEnum.Right,
    //        out Block rightBlock, out Chunk rightBlockChunk, out Vector3Int rightLocalPosition);
    //    block.GetCloseBlockByDirection(chunk, localPosition, DirectionEnum.UP,
    //        out Block upBlock, out Chunk upBlockChunk, out Vector3Int upLocalPosition);
    //    block.GetCloseBlockByDirection(chunk, localPosition, DirectionEnum.Down,
    //        out Block downBlock, out Chunk downBlockChunk, out Vector3Int downLocalPosition);
    //    block.GetCloseBlockByDirection(chunk, localPosition, DirectionEnum.Forward,
    //        out Block forwardBlock, out Chunk forwardBlockChunk, out Vector3Int forwardLocalPosition);
    //    block.GetCloseBlockByDirection(chunk, localPosition, DirectionEnum.Back,
    //        out Block backBlock, out Chunk backBlockChunk, out Vector3Int backLocalPosition);


    //}

    //public Vector2[] uvsAddLRUD;

    //public Vector2[] uvsAddLRD;
    //public Vector2[] uvsAddLUD;
    //public Vector2[] uvsAddLRU;
    //public Vector2[] uvsAddRUD;

    //public Vector2[] uvsAddLU;
    //public Vector2[] uvsAddRU;
    //public Vector2[] uvsAddRD;
    //public Vector2[] uvsAddLD;

    //public Vector2[] uvsAddUD;
    //public Vector2[] uvsAddLR;

    //public Vector2[] uvsAddL;
    //public Vector2[] uvsAddR;
    //public Vector2[] uvsAddU;
    //public Vector2[] uvsAddD;

    //public Vector2[] uvsAddNone;
    //protected Vector2[] GetUVsAddDir(Block leftBlockDir,Block rightBlockDir,Block upBlockDir,Block downBlockDir)
    //{
    //    bool isTransparentLeft = leftBlockDir.blockInfo.GetBlockShape() == BlockShapeEnum.CubeTransparent ? true : false;
    //    bool isTransparentRight = rightBlockDir.blockInfo.GetBlockShape() == BlockShapeEnum.CubeTransparent ? true : false;
    //    bool isTransparentUp = upBlockDir.blockInfo.GetBlockShape() == BlockShapeEnum.CubeTransparent ? true : false;
    //    bool isTransparentDown = downBlockDir.blockInfo.GetBlockShape() == BlockShapeEnum.CubeTransparent ? true : false;
        
    //    if (!isTransparentLeft && !isTransparentRight && !isTransparentUp && !isTransparentDown)
    //    {
    //        return uvsAddLRUD;
    //    }
    //    else if (!isTransparentLeft && !isTransparentRight && !isTransparentUp && !isTransparentDown)
    //    {
    //        return uvsAddLRUD;
    //    }
    //    else if (!isTransparentLeft && !isTransparentRight && !isTransparentUp && !isTransparentDown)
    //    {
    //        return uvsAddLRUD;
    //    }
    //    else if (!isTransparentLeft && !isTransparentRight && !isTransparentUp && !isTransparentDown)
    //    {
    //        return uvsAddLRUD;
    //    }
    //    else if (!isTransparentLeft && !isTransparentRight && !isTransparentUp && !isTransparentDown)
    //    {
    //        return uvsAddLRUD;
    //    }
    //    else if (!isTransparentLeft && !isTransparentRight && !isTransparentUp && !isTransparentDown)
    //    {
    //        return uvsAddLRUD;
    //    }
    //    else if (!isTransparentLeft && !isTransparentRight && !isTransparentUp && !isTransparentDown)
    //    {
    //        return uvsAddLRUD;
    //    }
    //    else if (!isTransparentLeft && !isTransparentRight && !isTransparentUp && !isTransparentDown)
    //    {
    //        return uvsAddLRUD;
    //    }
    //    else if (!isTransparentLeft && !isTransparentRight && !isTransparentUp && !isTransparentDown)
    //    {
    //        return uvsAddLRUD;
    //    }
    //    else if (!isTransparentLeft && !isTransparentRight && !isTransparentUp && !isTransparentDown)
    //    {
    //        return uvsAddLRUD;
    //    }
    //    else if (!isTransparentLeft && !isTransparentRight && !isTransparentUp && !isTransparentDown)
    //    {
    //        return uvsAddLRUD;
    //    }
    //    else if (!isTransparentLeft && !isTransparentRight && !isTransparentUp && !isTransparentDown)
    //    {
    //        return uvsAddLRUD;
    //    }
    //    else if (!isTransparentLeft && !isTransparentRight && !isTransparentUp && !isTransparentDown)
    //    {
    //        return uvsAddLRUD;
    //    }
    //    else if (!isTransparentLeft && !isTransparentRight && !isTransparentUp && !isTransparentDown)
    //    {
    //        return uvsAddLRUD;
    //    }
    //    else if (!isTransparentLeft && !isTransparentRight && !isTransparentUp && !isTransparentDown)
    //    {
    //        return uvsAddLRUD;
    //    }
    //    else
    //    {
    //        return uvsAddNone;
    //    }
    //}
}