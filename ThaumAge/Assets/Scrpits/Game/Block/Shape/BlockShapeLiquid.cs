using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockShapeLiquid : BlockShapeCube
{

    public override void InitData(Block block)
    {
        base.InitData(block);
        uvsAdd = new Vector2[]
        {
            new Vector2(0,0),
            new Vector2(0,1),
            new Vector2(1,1),
            new Vector2(1,0),
        };
    }

    public override void BaseAddVertsUVsColors(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, DirectionEnum face, Vector3[] vertsAdd, Vector2[] uvsAdd, Color[] colorsAdd)
    {
        BlockBean blockData = chunk.GetBlockData(localPosition);
        //水是满的
        if (blockData == null)
        {
            base.BaseAddVertsUVsColors(chunk, localPosition, direction, face, vertsAdd, uvsAdd, colorsAdd);
        }
        else
        {
            //获取水的数据
            BlockMetaLiquid blockMetaLiquid = blockData.GetBlockMeta<BlockMetaLiquid>();
            //水是满的
            if(blockMetaLiquid.volume == BlockBaseLiquid.maxLiquidVolume)
            {
                base.BaseAddVertsUVsColors(chunk, localPosition, direction, face, vertsAdd, uvsAdd, colorsAdd);
            }
            else
            {

            }
        }
    }

    /// <summary>
    /// 检测是否需要构建面
    /// </summary>
    protected override bool CheckNeedBuildFaceDef(Block closeBlock, Chunk closeBlockChunk, Vector3Int closeLocalPosition,DirectionEnum closeDirection)
    {
        BlockShapeEnum blockShape = closeBlock.blockInfo.GetBlockShape();
        switch (blockShape)
        {
            case BlockShapeEnum.Cube:
                if(closeDirection == DirectionEnum.UP)
                {
                    return true;
                }
                return false;
            case BlockShapeEnum.Liquid:
                if (closeBlock.blockType == block.blockType)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            default:
                if (closeBlock.blockType == BlockTypeEnum.FlowerWater)
                {
                    return false;
                }
                return true;
        }
    }
}