using UnityEditor;
using UnityEngine;

public class BlockShapePlough : BlockShapeCubeCuboid
{
    protected Color[] colorAddWater;
    public override void InitData(Block block)
    {
        base.InitData(block);
        colorAddWater = new Color[]{
            Color.gray,
            Color.gray,
            Color.gray,
            Color.gray
        };
    }

    public override void BaseAddVertsUVsColors(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, DirectionEnum face, Vector3[] vertsAdd, Vector2[] uvsAdd, Color[] colorsAdd)
    {
        BlockBean blockData = chunk.GetBlockData(localPosition);
        if (blockData != null && face == DirectionEnum.UP)
        {
            BlockMetaPlough blockMetaPlough = Block.FromMetaData<BlockMetaPlough>(blockData.meta);
            //判断是否浇水
            if (blockMetaPlough != null && blockMetaPlough.waterState == 1)
            {
                base.BaseAddVertsUVsColors(chunk, localPosition, direction, face, vertsAdd, uvsAdd, colorAddWater);
            }
            else
            {
                base.BaseAddVertsUVsColors(chunk, localPosition, direction, face, vertsAdd, uvsAdd, colorsAdd);
            }
            return;
        }
        base.BaseAddVertsUVsColors(chunk, localPosition, direction, face, vertsAdd, uvsAdd, colorsAdd);
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
            case BlockShapeEnum.Plough:
                return false;
            default:
                return true;
        }
    }
}