using UnityEditor;
using UnityEngine;

public class ItemTypeBlock : Item
{
    protected override void UseForPlayer(Player player, ItemsBean itemData, int type)
    {
        //检测玩家前方是否有方块
        if (player.playerRay.RayToChunkBlock(out RaycastHit hit, out Vector3Int targetBlockPosition))
        {
            Chunk chunkForHit = hit.collider.GetComponentInParent<Chunk>();
            if (chunkForHit)
            {
                //获取位置和方向
                player.playerRay.GetHitPositionAndDirection(hit, out Vector3Int targetPosition, out Vector3Int closePosition, out BlockDirectionEnum direction);
                //如果上手没有物品 或者是左键点击 则挖掘
                if (itemData == null || itemData.itemId == 0 || type == 0)
                {
                    BreakTarget(itemData, targetPosition);
                }
                //如果手上有物品 则使用
                else
                {
                    //获取目标方块
                    WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(targetPosition, out Block targetBlock, out BlockDirectionEnum targetBlockDirection, out Chunk taragetChunk);
                    //首先获取靠近方块
                    WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(closePosition, out Block closeBlock, out BlockDirectionEnum closeBlockDirection, out Chunk closeChunk);
                    //如果靠近得方块有区块
                    if (closeChunk)
                    {
                        //如果不是空方块 则不放置(液体则覆盖放置)
                        if (closeBlock != null && closeBlock.blockType != BlockTypeEnum.None && closeBlock.blockInfo.GetBlockShape() != BlockShapeEnum.Liquid)
                            return;
                        //如果重量为1 说明是草之类太轻的物体 则也不能放置
                        if (closeBlock != null && closeBlock.blockInfo.weight == 1) 
                            return;
                        //获取物品信息
                        ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemData.itemId);
                        //如果是可放置的方块
                        BlockInfoBean blockInfo = BlockHandler.Instance.manager.GetBlockInfo(itemsInfo.type_id);

                        BlockTypeEnum changeBlockType = blockInfo.GetBlockType();

                        //更新方块并 添加更新区块
                        if (blockInfo.rotate_state == 0)
                        {
                            closeChunk.SetBlockForWorld(closePosition, changeBlockType, BlockDirectionEnum.UpForward, itemData.meta);
                        }
                        else if (blockInfo.rotate_state == 1)
                        {
                            closeChunk.SetBlockForWorld(closePosition, changeBlockType, direction, itemData.meta);
                        }
                        else if (blockInfo.rotate_state == 2)
                        {
                            //只能朝上
                            if((int)direction > 20)
                            {
                                direction = (BlockDirectionEnum)((int)direction%10 + 10);
                            }
                            closeChunk.SetBlockForWorld(closePosition, changeBlockType, direction, itemData.meta);
                        }
                        //扣除道具
                        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
                        userData.AddItems(itemData, -1);
                        //刷新UI
                        UIHandler.Instance.RefreshUI();
                    }
                }
            }
        }
    }
}