using UnityEditor;
using UnityEngine;

public class ItemTypeBlock : Item
{
    protected override void UseForPlayer(Player player, ItemsBean itemData, ItemUseTypeEnum useType)
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
                    TargetUseL(itemData, targetPosition);
                }
                //如果手上有物品 则使用
                else
                {
                    TargetUseR(player.gameObject, itemData, targetPosition, closePosition, direction);
                }
            }
        }
    }

    public override void TargetUseR(GameObject user, ItemsBean itemData, Vector3Int targetPosition, Vector3Int closePosition, BlockDirectionEnum direction)
    {
        //获取目标方块
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(targetPosition, out Block targetBlock, out BlockDirectionEnum targetBlockDirection, out Chunk taragetChunk);
        //首先获取靠近方块
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(closePosition, out Block closeBlock, out BlockDirectionEnum closeBlockDirection, out Chunk closeChunk);
        //如果靠近得方块有区块
        if (closeChunk != null)
        {
            //如果不是空方块 则不放置(液体则覆盖放置)
            if (closeBlock != null && closeBlock.blockType != BlockTypeEnum.None && closeBlock.blockInfo.GetBlockShape() != BlockShapeEnum.Liquid)
                return;
            //如果重量为1 说明是草之类太轻的物体 则也不能放置
            if (closeBlock != null && closeBlock.blockInfo.weight == 1)
                return;
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
            UIHandler.Instance.RefreshUI();
            //播放音效
            AudioHandler.Instance.PlaySound(601);
        }
    }
}