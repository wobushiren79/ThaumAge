using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlockTypeCraftingTableArcane : Block
{
    /// <summary>
    /// 互动
    /// </summary>
    public override void Interactive(GameObject user, Vector3Int worldPosition, BlockDirectionEnum blockDirection)
    {
        base.Interactive(user, worldPosition, blockDirection);
        //只有player才能打开
        if (user == null || user.GetComponent<Player>() == null)
            return;
        UIGameUserDetails uiGameUserDetails = UIHandler.Instance.OpenUIAndCloseOther<UIGameUserDetails>();
        uiGameUserDetails.ui_ViewSynthesis.SetData(ItemsSynthesisTypeEnum.Arcane, worldPosition);
        uiGameUserDetails.SetSelectType(1);
    }

    /// <summary>
    /// 获取周围魔力加成
    /// </summary>
    public int GetAroundMagicTotal(Vector3Int worldPosition)
    {
        int magicTotal = 100;
        GetRoundBlock(worldPosition, callBackItem: (targetChunk, targetBlock, targetLocalPosition) =>
         {
             magicTotal += GetSingleBlockMagic(targetBlock);
         });
        return magicTotal;
    }

    /// <summary>
    /// 获取单个方块的魔力加成
    /// </summary>
    /// <param name="targetBlock"></param>
    /// <returns></returns>
    public int GetSingleBlockMagic(Block targetBlock)
    {
        int magic = 0;
        if (targetBlock == null)
        {
            return magic;
        }
        var itemInfo = ItemsHandler.Instance.manager.GetItemsInfoByBlockType(targetBlock.blockType);
        if (itemInfo != null)
        {
            int magicElemental = itemInfo.GetElemental(ElementalTypeEnum.Magic);
            magic += magicElemental * 10;
        }
        return magic;
    }
}