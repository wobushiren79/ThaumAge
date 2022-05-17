using UnityEditor;
using UnityEngine;

public class BlockShapeCubeLeaves : BlockShapeCube
{

    /// <summary>
    /// 检测是否需要构建面
    /// </summary>
    protected override bool CheckNeedBuildFaceDef(Block closeBlock, Chunk closeBlockChunk, Vector3Int closeLocalPosition)
    {
        BlockShapeEnum blockShape = closeBlock.blockInfo.GetBlockShape();
        switch (blockShape)
        {
            case BlockShapeEnum.Cube:
            case BlockShapeEnum.CubeLeaves:
                return false;
            default:
                return true;
        }
    }
}