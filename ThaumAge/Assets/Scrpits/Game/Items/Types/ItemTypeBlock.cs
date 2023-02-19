using UnityEditor;
using UnityEngine;

public class ItemTypeBlock : Item
{
    public override void UseForPlayer(Player player, ItemsBean itemData, ItemUseTypeEnum useType)
    {
        //检测玩家前方是否有方块
        if (player.playerRay.RayToChunkBlock(out RaycastHit hit, out Vector3Int targetBlockPosition))
        {
            ChunkComponent chunkForHit = hit.collider.GetComponentInParent<ChunkComponent>();
            if (chunkForHit != null)
            {
                //获取位置和方向
                player.playerRay.GetHitPositionAndDirection(hit, out Vector3Int targetPosition, out Vector3Int closePosition, out BlockDirectionEnum direction);
                //如果上手没有物品 或者是左键点击 则挖掘
                if (itemData == null || itemData.itemId == 0 || useType == ItemUseTypeEnum.Left)
                {
                    TargetUseL(player.gameObject, itemData, targetPosition);
                }
                //如果手上有物品 则使用
                else
                {
                    TargetUseR(player.gameObject, itemData, targetPosition, closePosition, direction);
                }
            }
        }
    }

    public override bool TargetUseR(GameObject user, ItemsBean itemData, Vector3Int targetPosition, Vector3Int closePosition, BlockDirectionEnum direction)
    {
        bool isBlockUseStop = base.TargetUseR(user, itemData, targetPosition, closePosition, direction);
        if (isBlockUseStop)
            return true;

        //获取目标方块
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(targetPosition, out Block targetBlock, out BlockDirectionEnum targetBlockDirection, out Chunk taragetChunk);
        //首先获取靠近方块
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(closePosition, out Block closeBlock, out BlockDirectionEnum closeBlockDirection, out Chunk closeChunk);
        //如果靠近得方块有区块
        if (taragetChunk != null && closeChunk != null)
        {
            bool canUse = TargetUseForCheckCanUse(targetPosition, closePosition, targetBlock, closeBlock);
            if (!canUse)
                return false;
            //获取物品信息
            ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemData.itemId);
            //获取方块信息
            Block useBlock = BlockHandler.Instance.manager.GetRegisterBlock(itemsInfo.type_id);
            BlockInfoBean blockInfo = useBlock.blockInfo;

            BlockTypeEnum changeBlockType = blockInfo.GetBlockType();

            //获取meta数据
            string metaData = useBlock.ItemUseMetaData(closePosition, changeBlockType, direction, itemData.meta);
            //使用方块
            useBlock.ItemUse(this, itemData,
                targetPosition, targetBlockDirection, targetBlock, taragetChunk,
                closePosition, closeBlockDirection, closeBlock, closeChunk,
                direction, metaData);

            //扣除道具
            UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
            userData.AddItems(itemData, -1);
            //刷新UI
            EventHandler.Instance.TriggerEvent(EventsInfo.ItemsBean_MetaChange, itemData);
            //播放音效
            AudioHandler.Instance.PlaySound(601);
        }
        return false;
    }


    /// <summary>
    /// 检测是否能放置
    /// </summary>
    /// <param name="closeBlock"></param>
    /// <returns></returns>
    public virtual bool TargetUseForCheckCanUse(Vector3Int targetPosition, Vector3Int closePosition, Block targetBlock, Block closeBlock)
    {
        //如果不是空方块 则不放置(液体则覆盖放置)
        if (closeBlock != null && closeBlock.blockType != BlockTypeEnum.None && closeBlock.blockInfo.GetBlockShape() != BlockShapeEnum.Liquid)
            return false;
        //如果重量为1 说明是草之类太轻的物体 则也不能放置
        if (closeBlock != null && closeBlock.blockInfo.weight == 1)
            return false;
        return true;
    }
}