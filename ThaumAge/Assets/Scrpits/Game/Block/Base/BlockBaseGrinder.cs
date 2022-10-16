using UnityEditor;
using UnityEngine;

public class BlockBaseGrinder : BlockBaseItemsTransition
{
    /// <summary>
    /// 增加转换进度
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="blockData"></param>
    /// <param name="addTransitionPro"></param>
    public void AddTransitionPro(Block grinderBlock, Chunk grinderChunk, Vector3Int grinderLocalPosition, float addTransitionPro)
    {
        if (grinderChunk == null || grinderBlock == null)
            return;
        BlockBean blockData = grinderChunk.GetBlockData(grinderLocalPosition);
        if (blockData == null)
        {
            blockData = new BlockBean(grinderLocalPosition, grinderBlock.blockType);
        }
        BlockMetaItemsTransition blockMetaData;
        if (blockData.meta.IsNull())
        {
            blockMetaData = new BlockMetaItemsTransition();
        }
        else
        {
            blockMetaData = blockData.GetBlockMeta<BlockMetaItemsTransition>();
        }
        //如果没有可以转换的物体
        if (blockMetaData.itemBeforeId == 0|| blockMetaData.itemBeforeNum == 0)
            return;
        ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(blockMetaData.itemBeforeId);
        itemsInfo.GetGrindItems(out int afterTransitionId,out int afterTransitionNum);
        //如果没有转换后的物体
        if (afterTransitionId == 0)
        {
            return;
        }

        AddTransitionPro(blockMetaData, afterTransitionId, afterTransitionNum, addTransitionPro);
        SaveTransitionData(grinderChunk, grinderLocalPosition, blockData, blockMetaData);
    }
}