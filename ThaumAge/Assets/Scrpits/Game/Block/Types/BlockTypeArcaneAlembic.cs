using UnityEditor;
using UnityEngine;

public class BlockTypeArcaneAlembic : Block
{
    public override bool TargetUseBlock(GameObject user, ItemsBean itemData, Chunk targetChunk, Vector3Int targetWorldPosition)
    {
        if (itemData.itemId == 0)
            return true;
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
            GetBlockMetaData(targetChunk, targetWorldPosition - targetChunk.chunkData.positionForWorld, out BlockBean blockData, out BlockMetaArcaneAlembic blockMetaData);

            if (blockMetaData.listElemental.IsNull())
                return true;
            NumberBean numberData = blockMetaData.listElemental[0];
            bool checkCanAdd = blockMetaWardedJar.CheckCanAdd((ElementalTypeEnum)numberData.id);
            if (checkCanAdd)
            {
                blockMetaWardedJar.AddElemental((ElementalTypeEnum)numberData.id, (int)numberData.number, out int leftElemental);
                numberData.number = leftElemental;
            }
            itemData.meta = JsonUtil.ToJson(blockMetaWardedJar);
            blockData.SetBlockMeta(blockMetaData);
            targetChunk.SetBlockData(blockData);
        }
        return true;
    }

}