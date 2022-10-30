using UnityEditor;
using UnityEngine;

public class BlockShapeCubeAround : BlockShapeCube
{

    public BlockShapeCubeAround(Block block) : base(block)
    {

    }

    /// <summary>
    /// 获取周围方块类型  
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    /// <returns>
    /// 0000周围无方块  
    /// 左右上下
    /// 1111 四周都有
    /// </returns>
    public int GetAroundBlockType(Chunk chunk, Vector3Int localPosition)
    {
        int blockType = 0;
        //获取上下左右4个方向的方块
        block.GetCloseBlockByDirection(chunk, localPosition, DirectionEnum.Left, out Block leftBlock, out Chunk leftChunk, out Vector3Int leftBlockLocalPosition);
        block.GetCloseBlockByDirection(chunk, localPosition, DirectionEnum.Right, out Block rightBlock, out Chunk rightChunk, out Vector3Int rightBlockLocalPosition);
        block.GetCloseBlockByDirection(chunk, localPosition, DirectionEnum.UP, out Block upBlock, out Chunk upChunk, out Vector3Int upBlockLocalPosition);
        block.GetCloseBlockByDirection(chunk, localPosition, DirectionEnum.Down, out Block downBlock, out Chunk downChunk, out Vector3Int downBlockLocalPosition);

        if (leftChunk != null && leftBlock != null && leftBlock.blockType == block.blockType)
        {
            blockType += 1000;
        }
        if (rightChunk != null && rightBlock != null && rightBlock.blockType == block.blockType)
        {
            blockType += 100;
        }
        if (upChunk != null && upBlock != null && upBlock.blockType == block.blockType)
        {
            blockType += 10;
        }
        if (downChunk != null && downBlock != null && downBlock.blockType == block.blockType)
        {
            blockType += 1;
        }
        return blockType;
    }

    /// <summary>
    /// 构建方块的六个面
    /// </summary>
    public override void BuildBlock(Chunk chunk, Vector3Int localPosition)
    {
        //只有在能旋转的时候才去查询旋转方向
        BlockDirectionEnum direction = BlockDirectionEnum.UpForward;

        int blockAroundType = GetAroundBlockType(chunk, localPosition);

        if (block.blockInfo.rotate_state != 0)
        {
            direction = chunk.chunkData.GetBlockDirection(localPosition.x, localPosition.y, localPosition.z);
        }
        //Left
        if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Left))
        {
            BuildFace(chunk, localPosition, direction, DirectionEnum.Left, vertsAddLeft, uvsAddLeft, colorsAddCube, trisAddCube);
        }

        //Right
        if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Right))
        {
            BuildFace(chunk, localPosition, direction, DirectionEnum.Right, vertsAddRight, uvsAddRight, colorsAddCube, trisAddCube);
        }

        //Bottom
        if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Down))
        {
            BuildFace(chunk, localPosition, direction, DirectionEnum.Down, vertsAddDown, uvsAddDown, colorsAddCube, trisAddCube);
        }

        //Top
        if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.UP))
        {
            BuildFace(chunk, localPosition, direction, DirectionEnum.UP, vertsAddUp, uvsAddUp, colorsAddCube, trisAddCube);
        }

        //Forward
        if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Forward))
        {
            BuildFace(chunk, localPosition, direction, DirectionEnum.Forward, vertsAddForward, uvsAddForward, colorsAddCube, trisAddCube);
        }

        //Back
        if (CheckNeedBuildFace(chunk, localPosition, direction, DirectionEnum.Back))
        {
            BuildFace(chunk, localPosition, direction, DirectionEnum.Back, vertsAddBack, uvsAddBack, colorsAddCube, trisAddCube);
        }
    }
}