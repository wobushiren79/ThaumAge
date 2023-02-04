using UnityEditor;
using UnityEngine;

public class BlockTypeArcaneAlembic : Block
{
    public override bool TargetUseBlock(GameObject user, ItemsBean itemData, Chunk targetChunk, Vector3Int targetLocalPosition)
    {
        if (itemData.itemId == 0)
            return false;
        ItemsInfoBean itemInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemData.itemId);
        //如果时源质罐子
        if (itemInfo.id == 106211)
        {
            BlockMetaWardedJar blockMetaWardedJar = null;
            if (itemData.meta.IsNull())
            {
                if (blockMetaWardedJar == null)
                    blockMetaWardedJar = new BlockMetaWardedJar();
            }
            else
            {

                blockMetaWardedJar = JsonUtil.FromJson<BlockMetaWardedJar>(itemData.meta);
                if (blockMetaWardedJar == null)
                    blockMetaWardedJar = new BlockMetaWardedJar();
            }
            GetBlockMetaData(targetChunk, targetLocalPosition, out BlockBean blockData, out BlockMetaArcaneAlembic blockMetaData);

            if (blockMetaData.elementalData == null || blockMetaData.elementalData.id == 0 || blockMetaData.elementalData.number <= 0)
                return false;
            NumberBean numberData = blockMetaData.elementalData;
            bool checkCanAdd = blockMetaWardedJar.CheckCanAdd((ElementalTypeEnum)numberData.id);
            if (checkCanAdd)
            {
                blockMetaWardedJar.AddElemental((ElementalTypeEnum)numberData.id, (int)numberData.number, out int leftElemental);
                numberData.number = leftElemental;
            }
            itemData.meta = JsonUtil.ToJson(blockMetaWardedJar);
            blockData.SetBlockMeta(blockMetaData);
            targetChunk.SetBlockData(blockData);
            return true;
        }
        else
        {
            return false;
        }
    }

}