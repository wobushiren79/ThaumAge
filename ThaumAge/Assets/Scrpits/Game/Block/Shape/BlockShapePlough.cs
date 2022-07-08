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
    protected override bool CheckNeedBuildFaceDef(Block closeBlock, Chunk closeBlockChunk, Vector3Int closeLocalPosition, DirectionEnum closeDirection)
    {
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