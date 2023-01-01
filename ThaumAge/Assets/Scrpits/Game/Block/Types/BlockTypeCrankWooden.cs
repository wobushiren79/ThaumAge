using UnityEditor;
using UnityEngine;
using DG.Tweening;

public class BlockTypeCrankWooden : Block
{
    public override bool TargetUseBlock(GameObject user, ItemsBean itemData, Chunk targetChunk, Vector3Int targetWorldPosition)
    {
        Vector3Int blockLocalPosition = targetWorldPosition - targetChunk.chunkData.positionForWorld;
        GameObject objTarget = targetChunk.GetBlockObjForLocal(blockLocalPosition);
        if (objTarget == null)
            return true;

        GetCloseBlockByDirection(targetChunk, blockLocalPosition, DirectionEnum.Down, out Block downBlock, out Chunk downChunk, out Vector3Int downLocalPosition);
        if (downChunk == null || downBlock == null)
            return true;
        BlockTypeEnum downBlockType = downBlock.blockType;
        switch (downBlockType)
        {
            case BlockTypeEnum.GrinderSimple:
                HandleForGrinderSimple(objTarget, downBlock, downChunk, downLocalPosition);
                break;
        }
        return true;
    }

    /// <summary>
    /// 处理 研磨机
    /// </summary>
    /// <param name="objTarget"></param>
    public void HandleForGrinderSimple(GameObject objTarget, Block grinderBlock,  Chunk grinderChunk,  Vector3Int grinderLocalPosition)
    {
        Transform tfModel = objTarget.transform.Find("Model");
        tfModel.DOComplete();
        tfModel.DOLocalRotate(tfModel.localEulerAngles + new Vector3(0, 45, 0), 0.5f);

        BlockBaseGrinder blockBaseGrinder = grinderBlock as BlockBaseGrinder;
        blockBaseGrinder.AddTransitionPro(grinderBlock, grinderChunk, grinderLocalPosition,1/8f);
    }
}