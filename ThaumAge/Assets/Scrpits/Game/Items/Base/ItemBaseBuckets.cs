using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ItemBaseBuckets : Item
{
    /// <summary>
    /// 使用道具（鼠标右键）
    /// </summary>
    public override bool TargetUseR(GameObject user, ItemsBean itemData, Vector3Int targetPosition, Vector3Int closePosition, BlockDirectionEnum direction)
    {
        bool isBlockUseStop = base.TargetUseR(user,  itemData,  targetPosition,  closePosition,  direction);
        if (isBlockUseStop)
            return true;
        ItemMetaBuckets itemMetaBuckets = itemData.GetMetaData<ItemMetaBuckets>();
        if (itemMetaBuckets.itemIdForSomething == 0)
        {
            GetSomething(itemData, targetPosition, closePosition);
            return false;
        }
        SetSomething(itemData, targetPosition, closePosition);
        return false;
    }

    /// <summary>
    /// 设置道具图标
    /// </summary>
    /// <param name="ivTarget"></param>
    /// <param name="itemsInfo"></param>
    public override void SetItemIcon(ItemsBean itemData, ItemsInfoBean itemsInfo, Image ivTarget = null, SpriteRenderer srTarget = null)
    {
        base.SetItemIcon(itemData, itemsInfo, ivTarget, srTarget);
        //没有物品数据 比如合成的时候
        if (itemData == null)
        {
            return;
        }
        ItemMetaBuckets itemMetaBuckets = itemData.GetMetaData<ItemMetaBuckets>();
        //如果没有东西 则不设置
        if (itemMetaBuckets.itemIdForSomething == 0)
        {

        }
        //如果有东西
        else
        {
            GameObject objIvSomething = null;
            if (ivTarget != null)
            {
                objIvSomething = ItemsHandler.Instance.Instantiate(ivTarget.gameObject, ivTarget.gameObject);
            }
            if (srTarget != null)
            {
                objIvSomething = ItemsHandler.Instance.Instantiate(srTarget.gameObject, srTarget.gameObject);
            }
            if (objIvSomething == null)
            {
                return;
            }
            objIvSomething.ShowObj(true);

            Image ivSomething = objIvSomething.GetComponent<Image>();
            SpriteRenderer srSomething = objIvSomething.GetComponent<SpriteRenderer>();

            //设置图标
            ItemsInfoBean itemsInfoForSomething = ItemsHandler.Instance.manager.GetItemsInfoById(itemMetaBuckets.itemIdForSomething);
            string iconKeySomething = "";
            BlockInfoBean blockInfoForSometiong = BlockHandler.Instance.manager.GetBlockInfo(itemsInfoForSomething.type_id);
            if (blockInfoForSometiong.GetBlockMaterialType() == BlockMaterialEnum.Water)
            {
                iconKeySomething = "icon_item_buckets_water";
            }
            else if (blockInfoForSometiong.GetBlockMaterialType() == BlockMaterialEnum.Magma)
            {
                iconKeySomething = "icon_item_buckets_magma";
            }
            IconHandler.Instance.manager.GetItemsSpriteByName(iconKeySomething, (spIcon) =>
            {
                if (spIcon == null)
                {
                    IconHandler.Instance.GetUnKnowSprite((spIcon) =>
                    {
                        if (ivSomething != null)
                        {
                            ivSomething.sprite = spIcon;
                        }
                        if (srSomething != null)
                        {
                            srSomething.sprite = spIcon;
                            srSomething.sortingOrder = 1;
                        }
                    });
                }
                else
                {
                    if (ivSomething != null)
                    {
                        ivSomething.sprite = spIcon;
                    }
                    if (srSomething != null)
                    {
                        srSomething.sprite = spIcon;
                        srSomething.sortingOrder = 1;
                    }
                }
            });
        }
    }

    /// <summary>
    /// 检测是否能装
    /// </summary>
    /// <returns></returns>
    public virtual bool CheckCanGet(ItemsBean itemData, Block targetBlock)
    {
        BlockShapeEnum blockShape = targetBlock.blockInfo.GetBlockShape();
        switch (blockShape)
        {
            case BlockShapeEnum.Liquid:
            case BlockShapeEnum.LiquidCross:
            case BlockShapeEnum.LiquidCrossOblique:
                return true;
        }
        return false;
    }


    /// <summary>
    /// 装东西
    /// </summary>
    public virtual void GetSomething(ItemsBean itemData, Vector3Int targetPosition, Vector3Int closePosition)
    {
        //获取目标方块
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(targetPosition, out Block targetBlock, out BlockDirectionEnum targetBlockDirection, out Chunk targetChunk);
        if (targetChunk == null)
            return;
        //首先获取靠近方块
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(closePosition, out Block closeBlock, out BlockDirectionEnum closeBlockDirection, out Chunk closeChunk);
        if (closeChunk == null)
            return;
        if (closeBlock != null && closeBlock.blockType == BlockTypeEnum.Water || closeBlock.blockType == BlockTypeEnum.Magma)
        {
            //检测是否能装
            if (!CheckCanGet(itemData, closeBlock))
                return;

            Vector3Int closePositionLocal = closePosition - closeChunk.chunkData.positionForWorld;

            BlockBaseLiquid closeLiquidBlock = closeBlock as BlockBaseLiquid;
            closeLiquidBlock.GetBlockMetaData(closeChunk, closePositionLocal, out BlockBean blockCloseData, out BlockMetaLiquid blockMetaLiquid);
            int liquidVolume = blockMetaLiquid.AddVolume(-1);
            blockCloseData.SetBlockMeta(blockMetaLiquid);
            if (liquidVolume == 0)
            {
                closeChunk.SetBlockForLocal(closePositionLocal, BlockTypeEnum.None);
            }
            else
            {
                closeChunk.SetBlockForLocal(closePositionLocal, closeBlock.blockType, closeBlockDirection, blockCloseData.meta);
            }

            ItemMetaBuckets itemMetaBuckets = itemData.GetMetaData<ItemMetaBuckets>();
            ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoByBlockId((int)closeBlock.blockInfo.id);
            //扣除道具
            itemMetaBuckets.itemIdForSomething = (int)itemsInfo.id;
            itemData.SetMetaData(itemMetaBuckets);

            ///播放音效
            AudioHandler.Instance.PlaySound(704);

            UIHandler.Instance.RefreshUI();
        }
    }

    /// <summary>
    /// 放东西
    /// </summary>
    public virtual void SetSomething(ItemsBean itemData, Vector3Int targetPosition, Vector3Int closePosition)
    {
        //获取目标方块
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(targetPosition, out Block targetBlock, out BlockDirectionEnum targetBlockDirection, out Chunk targetChunk);
        if (targetChunk == null)
            return;
        if (targetBlock != null)
        {
            //首先看看能否把水桶里的水放进方块里面
            bool isSetSuccess = targetBlock.SetItems(targetChunk, targetBlock, targetBlockDirection, targetPosition, itemData);
            if (isSetSuccess)
            {
                AudioHandler.Instance.PlaySound(705);
                UIHandler.Instance.RefreshUI();
                return;
            }
        }
        //首先获取靠近方块
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(closePosition, out Block closeBlock, out BlockDirectionEnum closeBlockDirection, out Chunk closeChunk);
        if (closeChunk == null)
            return;
        if (closeBlock == null || closeBlock.blockType == BlockTypeEnum.None)
        {
            ItemMetaBuckets itemMetaBuckets = itemData.GetMetaData<ItemMetaBuckets>();
            //获取物品信息
            ItemsInfoBean itemsInfoSomething = ItemsHandler.Instance.manager.GetItemsInfoById(itemMetaBuckets.itemIdForSomething);
            //获取方块信息
            Block useBlockForSomething = BlockHandler.Instance.manager.GetRegisterBlock(itemsInfoSomething.type_id);
            BlockInfoBean blockInfoForSomething = useBlockForSomething.blockInfo;

            BlockTypeEnum changeBlockType = blockInfoForSomething.GetBlockType();
            //获取meta数据
            string metaData = useBlockForSomething.ItemUseMetaData(closePosition, changeBlockType, BlockDirectionEnum.UpForward, itemData.meta);

            //使用方块
            useBlockForSomething.ItemUse(this, itemData,
                targetPosition, BlockDirectionEnum.UpForward, closeBlock, closeChunk,
                closePosition, closeBlockDirection, closeBlock, closeChunk,
                 BlockDirectionEnum.UpForward, metaData);

            //扣除道具
            itemMetaBuckets.itemIdForSomething = 0;
            itemData.SetMetaData(itemMetaBuckets);

            UIHandler.Instance.RefreshUI();
            AudioHandler.Instance.PlaySound(705);
        }
    }
}