using UnityEditor;
using UnityEngine;

public class BlockTypeSwitchesWooden : Block
{

    public override void Interactive(GameObject user, Vector3Int worldPosition, BlockDirectionEnum blockDirection)
    {
        base.Interactive(user, worldPosition, blockDirection);
        //只有player才能打开
        if (user == null || user.GetComponent<Player>() == null)
            return;
        //获取周围的方块 并触发互动
        GetRoundBlock(worldPosition, out Block upBlock, out Block downBlock, out Block leftBlock, out Block rightBlock, out Block forwardBlock, out Block backBlock);

        GameObject objBlock = GetBlockObj(worldPosition);

        if (upBlock != null) upBlock.Interactive(objBlock, worldPosition + Vector3Int.up, blockDirection);
        if (downBlock != null) downBlock.Interactive(objBlock, worldPosition + Vector3Int.down, blockDirection);
        if (leftBlock != null) leftBlock.Interactive(objBlock, worldPosition + Vector3Int.left, blockDirection);
        if (rightBlock != null) rightBlock.Interactive(objBlock, worldPosition + Vector3Int.right, blockDirection);
        if (forwardBlock != null) forwardBlock.Interactive(objBlock, worldPosition + Vector3Int.forward, blockDirection);
        if (backBlock != null) backBlock.Interactive(objBlock, worldPosition + Vector3Int.back, blockDirection);


        //获取紧挨方块周围的方块并 互动
        DirectionEnum closeDirection = GetDirection(blockDirection);
        Vector3Int closePosition = Vector3Int.zero;
        //如果是能互动的方块 则不能连携到下一个方块
        switch (closeDirection)
        {
            case DirectionEnum.UP:
                if (downBlock.blockInfo.interactive_state == 1)
                    return;
                closePosition = Vector3Int.down;
                break;
            case DirectionEnum.Down:
                if (upBlock.blockInfo.interactive_state == 1)
                    return;
                closePosition = Vector3Int.up;
                break;
            case DirectionEnum.Left:
                if (rightBlock.blockInfo.interactive_state == 1)
                    return;
                closePosition = Vector3Int.right;
                break;
            case DirectionEnum.Right:
                if (leftBlock.blockInfo.interactive_state == 1)
                    return;
                closePosition = Vector3Int.left;
                break;
            case DirectionEnum.Forward:
                if (backBlock.blockInfo.interactive_state == 1)
                    return;
                closePosition = Vector3Int.back;
                break;
            case DirectionEnum.Back:
                if (forwardBlock.blockInfo.interactive_state == 1)
                    return;
                closePosition = Vector3Int.forward;
                break;
        }
        closePosition += worldPosition;

        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(closePosition, out Block closeBlock, out Chunk closeChunk);
        if (closeBlock != null)
        {
            GetRoundBlock(closePosition, out Block upCloseBlock, out Block downCloseBlock, out Block leftCloseBlock, out Block rightCloseBlock, out Block forwardCloseBlock, out Block backCloseBlock);

            if (upCloseBlock != null) upCloseBlock.Interactive(objBlock, closePosition + Vector3Int.up, blockDirection);
            if (downCloseBlock != null) downCloseBlock.Interactive(objBlock, closePosition + Vector3Int.down, blockDirection);
            if (leftCloseBlock != null) leftCloseBlock.Interactive(objBlock, closePosition + Vector3Int.left, blockDirection);
            if (rightCloseBlock != null) rightCloseBlock.Interactive(objBlock, closePosition + Vector3Int.right, blockDirection);
            if (forwardCloseBlock != null) forwardCloseBlock.Interactive(objBlock, closePosition + Vector3Int.forward, blockDirection);
            if (backCloseBlock != null) backCloseBlock.Interactive(objBlock, closePosition + Vector3Int.back, blockDirection);
        }
    }



}