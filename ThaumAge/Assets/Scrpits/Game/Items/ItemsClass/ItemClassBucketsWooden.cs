using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ItemClassBucketsWooden : ItemBaseBuckets
{

   public override bool CheckCanGet(ItemsBean itemData, Block targetBlock)
    {
        bool baseCheck = base.CheckCanGet(itemData,targetBlock);
        if (baseCheck)
        {
            //如果是岩浆 则烧掉这个桶
            if(targetBlock.blockInfo.GetBlockMaterialType() == BlockMaterialEnum.Magma)
            {
                itemData.itemId = 0;
                itemData.meta = null;
                itemData.number = 0;

                UIHandler.Instance.RefreshUI();
                return false;
            }
        }
        return baseCheck;
    }
}