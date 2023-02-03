using UnityEditor;
using UnityEngine;

public class BlockTypeFence : BlockBaseAroundLRFB
{
    /// <summary>
    /// 检测该方向是否能链接
    /// </summary>
    public override bool CheckCanLink(Chunk chunk, Vector3Int localPosition, DirectionEnum faceDiection)
    {
        //获取四周的方块 判断是否需要添加
        GetCloseBlockByDirection(chunk, localPosition, faceDiection,
            out Block blockClose, out Chunk blockChunkClose, out Vector3Int localPositionClose);
        //如果是方块
        if (blockClose.blockInfo.GetBlockShape() == BlockShapeEnum.Cube)
        {
            return true;
        }
        //如果是都这种形状（目前只用于栅栏）
        if (blockClose is BlockTypeFence)
        {
            return true;
        }
        //如果是同一种类型
        if (blockClose.blockType == blockType)
        {
            return true;
        }
        //如果是栅栏门
        if (blockClose is BlockBaseFenceDoor)
        {
            return true;
        }
        return false;
    }
}