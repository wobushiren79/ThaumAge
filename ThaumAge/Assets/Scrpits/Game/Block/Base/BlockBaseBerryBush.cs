using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockBaseBerryBush : Block
{

    public override bool TargetUseBlock(GameObject user, ItemsBean itemData, Chunk targetChunk, Vector3Int targetLocalPosition)
    {
        Vector3Int targetWorldPosition = targetChunk.chunkData.positionForWorld + targetLocalPosition;
        List<ItemsBean> listDropItems = GetDropItems();

        //创建掉落物
        ItemsHandler.Instance.CreateItemCptDropList(listDropItems, ItemDropStateEnum.DropPick, targetWorldPosition + Vector3.one * 0.5f);

        //设置被采集
        targetChunk.SetBlockForLocal(targetLocalPosition, (BlockTypeEnum) blockInfo.remark_int);

        return false;
    }

}