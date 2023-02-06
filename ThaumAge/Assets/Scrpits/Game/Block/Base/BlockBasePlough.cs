using UnityEditor;
using UnityEngine;

public class BlockBasePlough : Block
{

    public override void RefreshBlock(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction, int refreshType, int updateChunkType)
    {
        base.RefreshBlock(chunk, localPosition, direction, refreshType, updateChunkType);
        GetCloseBlockByDirection(chunk, localPosition, DirectionEnum.UP,
            out Block blockClose, out Chunk blockChunkClose, out Vector3Int closeLocalPositionClose);
        if (blockClose != null && blockClose.blockType != BlockTypeEnum.None)
        {
            if (blockClose.blockInfo.GetBlockShape() != BlockShapeEnum.CropCross
                && blockClose.blockInfo.GetBlockShape() != BlockShapeEnum.CropCrossOblique
                && blockClose.blockInfo.GetBlockShape() != BlockShapeEnum.CropWell)
            {
                //还原成普通的地面
                chunk.SetBlockForLocal(localPosition, (BlockTypeEnum)blockInfo.remark_int, BlockDirectionEnum.UpForward);
            }
        }
    }
}