using UnityEditor;
using UnityEngine;

public class ItemTypeCreature : Item
{
    protected override void UseForPlayer(Player player, ItemsBean itemData, ItemUseTypeEnum useType)
    {
        //检测玩家前方是否有方块
        if (player.playerRay.RayToChunkBlock(out RaycastHit hit, out Vector3Int targetBlockPosition))
        {
            Chunk chunkForHit = hit.collider.GetComponentInParent<Chunk>();
            if (chunkForHit != null)
            {
                //获取位置和方向
                player.playerRay.GetHitPositionAndDirection(hit, out Vector3Int targetPosition, out Vector3Int closePosition, out BlockDirectionEnum direction);

                Vector3Int localPosition = targetPosition - chunkForHit.chunkData.positionForWorld;

                //放置位置
                Vector3Int upLocalPosition = localPosition + Vector3Int.up;

                //获取上方方块
                Block upBlock = chunkForHit.chunkData.GetBlockForLocal(upLocalPosition);

                //如果上方有方块 则无法放置
                if (upBlock != null && upBlock.blockType != BlockTypeEnum.None)
                    return;

                ItemsInfoBean itemsInfo = GetItemsInfo(itemData.itemId);
                CreatureHandler.Instance.CreateCreature(itemsInfo.type_id, targetPosition + Vector3Int.up);
            }
        }
    }
}