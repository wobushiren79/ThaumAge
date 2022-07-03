using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockBasePlant : Block
{
    /// <summary>
    /// 刷新方块
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    /// <param name="direction"></param>
    public override void RefreshBlock(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
    {
        //获取下方方块
        Vector3Int downLocalPosition = localPosition + Vector3Int.down;
        chunk.chunkData.GetBlockForLocal(downLocalPosition, out Block downBlock, out BlockDirectionEnum downBlockDirection);
        //如果下方方块为NONE或者为液体
        if (downBlock == null || downBlock.blockType == BlockTypeEnum.None || downBlock.blockInfo.GetBlockShape() == BlockShapeEnum.Liquid)
        {
            //移除方块
            chunk.RemoveBlockForLocal(localPosition);
            //创建道具
            List<ItemsBean> listDropItems = ItemsHandler.Instance.GetItemsDrop(blockInfo.items_drop);
            ItemsHandler.Instance.CreateItemCptDropList(listDropItems, ItemDropStateEnum.DropPick, chunk.chunkData.positionForWorld + localPosition);
        }
    }
}