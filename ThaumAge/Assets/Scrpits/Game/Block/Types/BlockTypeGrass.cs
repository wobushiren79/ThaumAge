using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockTypeGrass : Block
{
    /// <summary>
    /// 刷新方块
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    /// <param name="direction"></param>
    public override void RefreshBlock(Chunk chunk, Vector3Int localPosition, DirectionEnum direction)
    {
        //获取下方方块
        Vector3Int downLocalPosition = localPosition + Vector3Int.down;
        chunk.chunkData.GetBlockForLocal(downLocalPosition, out Block downBlock, out DirectionEnum downBlockDirection);
        //如果下方方块为NONE或者为液体
        if (downBlock == null || downBlock.blockType == BlockTypeEnum.None || downBlock.blockInfo.GetBlockShape() == BlockShapeEnum.Liquid)
        {
            //移除方块
            chunk.RemoveBlockForLocal(localPosition);
            //创建道具
            List<ItemsBean> listDropItems = blockInfo.GetItemsDrop();
            ItemsHandler.Instance.CreateItemCptDropList(listDropItems, chunk.chunkData.positionForWorld + localPosition, ItemDropStateEnum.DropPick);
        }
    }
}