using UnityEditor;
using UnityEngine;

public class BlockBaseHalf : Block
{
    public override string GetUseMetaData(Vector3Int worldPosition, BlockTypeEnum blockType, BlockDirectionEnum blockDirection, string curMeta)
    {
        BlockMetaCubeHalf blockMeta = FromMetaData<BlockMetaCubeHalf>(curMeta);
        if (blockMeta == null)
            blockMeta = new BlockMetaCubeHalf();
        int direction = MathUtil.GetUnitTen((int)blockDirection);
        switch (direction) 
        {
            case 1:
                blockMeta.SetHalfPosition(DirectionEnum.Down);
                break;
            case 2:
                blockMeta.SetHalfPosition(DirectionEnum.UP);
                break;
            case 3:
                blockMeta.SetHalfPosition(DirectionEnum.Right);
                break;
            case 4:
                blockMeta.SetHalfPosition(DirectionEnum.Left);
                break;
            case 5:
                blockMeta.SetHalfPosition(DirectionEnum.Forward);
                break;
            case 6:
                blockMeta.SetHalfPosition(DirectionEnum.Back);
                break;
        }
       return ToMetaData(blockMeta);
    }
}