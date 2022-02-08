﻿using UnityEditor;
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
                player.playerRay.GetHitPositionAndDirection(hit, out Vector3Int targetPosition, out Vector3Int closePosition, out DirectionEnum direction);
                //如果上手没有物品 或者是左键点击 则挖掘
                if (itemData == null || itemData.itemId == 0 || type == 0)
                {
                    BreakTarget(itemData, targetPosition);
                }
                //如果手上有物品 则使用
                else
                {
                    //获取目标方块
                    WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(targetPosition, out Block targetBlock, out DirectionEnum targetBlockDirection, out Chunk taragetChunk);
                    //如果是液体 则直接替换
                    if (taragetChunk!=null && targetBlock != null&&targetBlock.blockInfo.GetBlockShape() == BlockShapeEnum.Liquid)
                    {                       
                        //获取物品信息
                        ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemData.itemId);
                        //如果是可放置的方块
                        BlockInfoBean blockInfo = BlockHandler.Instance.manager.GetBlockInfo(itemsInfo.type_id);
                        BlockTypeEnum blockType = blockInfo.GetBlockType();
                        //更新方块并 添加更新区块
                        if (blockInfo.rotate_state == 0)
                        {
                            taragetChunk.SetBlockForWorld(targetPosition, blockType, DirectionEnum.UP);
                        }
                        else
                        {
                            taragetChunk.SetBlockForWorld(targetPosition, blockType, direction);
                        }
                        //扣除道具
                        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
                        userData.AddItems(itemData, -1);
                        //刷新UI
                        UIHandler.Instance.RefreshUI();
                        return;
                    }
                    //首先获取靠近方块
                    WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(closePosition, out Block closeBlock, out DirectionEnum closeBlockDirection, out Chunk closeChunk);
                    //如果靠近得方块有区块
                    if (closeChunk)
                    {
                        //如果不是空方块 则不放置
                        if (closeBlock != null && closeBlock.blockType != BlockTypeEnum.None)
                            return;
                        //获取物品信息
                        ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemData.itemId);
                        //如果是可放置的方块
                        BlockInfoBean blockInfo = BlockHandler.Instance.manager.GetBlockInfo(itemsInfo.type_id);

                        BlockTypeEnum changeBlockType = blockInfo.GetBlockType();

                        //更新方块并 添加更新区块
                        if (blockInfo.rotate_state == 0)
                        {
                            closeChunk.SetBlockForWorld(closePosition, changeBlockType, DirectionEnum.UP);
                        }
                        else
                        {
                            closeChunk.SetBlockForWorld(closePosition, changeBlockType, direction);
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