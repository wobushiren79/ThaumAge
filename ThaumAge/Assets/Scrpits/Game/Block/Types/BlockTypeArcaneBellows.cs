using UnityEditor;
using UnityEngine;

public class BlockTypeArcaneBellows : BlockBaseAroundLRFB
{

    public override bool CheckCanLink(Chunk chunk, Vector3Int localPosition, DirectionEnum faceDiection)
    {
        //获取四周的方块 判断是否需要添加
        GetCloseBlockByDirection(chunk, localPosition, faceDiection,
            out Block blockClose, out Chunk blockChunkClose, out Vector3Int localPositionClose);
        bool isCanLink = false;
        //源质冶炼厂
        if (blockClose.blockType == BlockTypeEnum.ElementSmeltery)
        {
            bool isClose = CheckCloseBlockForLRB(blockChunkClose, localPositionClose, chunk, localPosition);
            if (isClose)
                isCanLink = true;
        }
        //如果是多方块结构
        else if (blockClose.blockType == BlockTypeEnum.LinkLargeChild)
        {
            GetCloseBlockByDirection(chunk, localPosition - Vector3Int.up, faceDiection,
                out Block blockClose2, out Chunk blockChunkClose2, out Vector3Int localPositionClose2, 2);
            if(blockChunkClose2 == null || blockClose2 == null)
            {
                return isCanLink;
            }
            //如果是炼狱熔炉
            if (blockClose2.blockType == BlockTypeEnum.InfernalFurnace)
            {
                //炼狱熔炉检测和旁边·2格
                bool isClose = CheckCloseBlockForLRB(blockChunkClose2, localPositionClose2, chunk, localPosition - Vector3Int.up, 2);
                if (isClose)
                    isCanLink = true;
            }
        }
        return isCanLink;
    }

    /// <summary>
    /// 检测目标方块的指定方向是否是自己 LRB
    /// </summary>
    /// <returns></returns>
    public bool CheckCloseBlockForLRB
        (
        Chunk targetChunk, Vector3Int targetLocalPosition,
        Chunk selfChunk, Vector3Int selfLocalPosition,
        int targetDirectionOffset = 1
        )
    {
        bool leftCheck = CheckCloseBlock(targetChunk, targetLocalPosition, DirectionEnum.Left, selfChunk, selfLocalPosition, targetDirectionOffset);
        if (leftCheck)
            return true;
        else
        {
            bool rightCheck = CheckCloseBlock(targetChunk, targetLocalPosition, DirectionEnum.Right, selfChunk, selfLocalPosition, targetDirectionOffset);
            if (rightCheck)
                return true;
            else
            {
                bool backCheck = CheckCloseBlock(targetChunk, targetLocalPosition, DirectionEnum.Back, selfChunk, selfLocalPosition, targetDirectionOffset);
                if (backCheck)
                    return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 检测目标方块的指定方向是否是自己
    /// </summary>
    /// <returns></returns>
    public bool CheckCloseBlock
        (
        Chunk targetChunk, Vector3Int targetLocalPosition, DirectionEnum targetDirection,
        Chunk selfChunk, Vector3Int selfLocalPosition, int targetDirectionOffset = 1
        )
    {
        BlockDirectionEnum blockDirection = targetChunk.chunkData.GetBlockDirection(targetLocalPosition);
        DirectionEnum rotateDirection = blockShape.GetRotateDirection(blockDirection, targetDirection);
        GetCloseBlockByDirection(targetChunk, targetLocalPosition, rotateDirection, out Block closeBlock, out Chunk closeBlockChunk, out Vector3Int closeLocalPosition, targetDirectionOffset);
        if (closeBlockChunk == null)
            return false;
        if (closeBlockChunk == selfChunk && closeLocalPosition == selfLocalPosition)
        {
            return true;
        }
        return false;
    }

    public override void RefreshObjModel(Chunk chunk, Vector3Int localPosition, int refreshType)
    {
        bool isWork = true;
        bool leftCheck = CheckCanLink(chunk, localPosition, DirectionEnum.Left);
        if (!leftCheck)
        {
            bool rightCheck = CheckCanLink(chunk, localPosition, DirectionEnum.Right);
            if (!rightCheck)
            {
                bool forwardCheck = CheckCanLink(chunk, localPosition, DirectionEnum.Forward);
                if (!forwardCheck)
                {
                    bool backCheck = CheckCanLink(chunk, localPosition, DirectionEnum.Back);
                    if (!backCheck)
                    {
                        isWork = false;
                    }
                }
            }
        }
        float scaleSpeed = 0;
        if (isWork)
        {
            scaleSpeed = 1;
        }
        GameObject objItem = GetBlockObj(chunk, localPosition);
        if (objItem == null)
            return;
        MeshRenderer itemMR = objItem.GetComponentInChildren<MeshRenderer>();
        itemMR.material.SetFloat("_ScaleSpeed", scaleSpeed);
    }
}