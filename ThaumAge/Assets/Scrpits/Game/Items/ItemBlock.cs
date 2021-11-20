using UnityEditor;
using UnityEngine;

public class ItemBlock : Item
{
    public override void Use()
    {
        base.Use();
        Player player = GameHandler.Instance.manager.player;

        //检测玩家前方是否有方块
        if (player.playerRay.RayToChunkBlock(out RaycastHit hit, out Vector3Int targetBlockPosition))
        {
            Chunk chunkForHit = hit.collider.GetComponentInParent<Chunk>();
            if (chunkForHit)
            {               
                //获取位置和方向
                player.playerRay.GetHitPositionAndDirection(hit, out Vector3Int targetPosition, out Vector3Int closePosition, out DirectionEnum direction);
                //如果上手没有物品 则挖掘
                if (itemsData == null || itemsData.itemId == 0)
                {
                    BreakTarget(targetPosition);
                }
                //如果手上有物品 则使用
                else
                {
                    //首先获取靠近方块
                    WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(closePosition, out Block block, out DirectionEnum blockDirection, out Chunk addChunk);
                    //如果靠近得方块有区块
                    if (addChunk)
                    {
                        //获取物品信息
                        ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemsData.itemId);
                        ItemsTypeEnum itemsType = itemsInfo.GetItemsType();
                        //如果是可放置的方块
                        BlockInfoBean blockInfo = BlockHandler.Instance.manager.GetBlockInfo(itemsInfo.type_id);
                        //更新方块并 添加更新区块
                        if (blockInfo.rotate_state == 0)
                        {
                            addChunk.SetBlockForWorld(closePosition, blockInfo.GetBlockType(), DirectionEnum.UP);
                        }
                        else
                        {
                            addChunk.SetBlockForWorld(closePosition, blockInfo.GetBlockType(), direction);
                        }
                        //更新区块
                        //WorldCreateHandler.Instance.HandleForUpdateChunk(true, null);
                        Block newBlock = BlockHandler.Instance.manager.GetRegisterBlock(blockInfo.GetBlockType());
                        WorldCreateHandler.Instance.HandleForUpdateChunkTest(addChunk, closePosition, newBlock);
                    }
                }
            }
        }
    }
}