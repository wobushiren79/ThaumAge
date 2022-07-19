using UnityEditor;
using UnityEngine;

public class BlockShapeCubeTransparent : BlockShapeCube
{
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
}