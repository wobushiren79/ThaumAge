using UnityEditor;
using UnityEngine;

public class BlockBaseItemsTransition : Block
{

    /// <summary>
    /// 增加转换进度
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="blockData"></param>
    /// <param name="addTransitionPro"></param>
    public virtual void AddTransitionPro(BlockMetaItemsTransition blockMetaData, int afterTransitionId,int afterTransitionNum, float addTransitionPro)
    {
        ItemsInfoBean itemsInfoAfterTransition = ItemsHandler.Instance.manager.GetItemsInfoById(afterTransitionId);
        //如果已经超过上限了，则不再增加进度
        if (blockMetaData.itemAfterNum >= itemsInfoAfterTransition.max_number)
        {
            return;
        }
        blockMetaData.transitionPro += addTransitionPro;
        //转换完成
        if (blockMetaData.transitionPro >= 1)
        {
            //烧制完成
            blockMetaData.transitionPro = 0;
            blockMetaData.itemAfterId = afterTransitionId;
            blockMetaData.itemAfterNum += afterTransitionNum;
            blockMetaData.itemBeforeNum--;
        }
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    public virtual void SaveTransitionData(Chunk chunk, Vector3Int localPosition, BlockBean blockData, BlockMetaItemsTransition blockMetaData)
    {
        //数据检测
        if (blockMetaData.itemAfterNum <= 0)
        {
            blockMetaData.itemAfterNum = 0;
            blockMetaData.itemAfterId = 0;
        }
        if (blockMetaData.itemBeforeNum <= 0)
        {
            blockMetaData.itemBeforeNum = 0;
            blockMetaData.itemBeforeId = 0;
        }
        blockData.meta = ToMetaData(blockMetaData);
        chunk.isSaveData = true;
    }
}