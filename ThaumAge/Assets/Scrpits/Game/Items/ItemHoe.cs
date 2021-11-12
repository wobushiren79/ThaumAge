using UnityEditor;
using UnityEngine;

public class ItemHoe : Item
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

                Vector3Int localPosition = targetPosition - chunkForHit.chunkData.positionForWorld;
                //获取原位置方块
                chunkForHit.GetBlockForLocal(localPosition, out Block tagetBlock);

                //获取上方方块
                chunkForHit.GetBlockForLocal(localPosition + Vector3Int.up, out Block upBlock);

                //如果上方有方块 则无法使用锄头
                if (upBlock != null && upBlock.blockType != BlockTypeEnum.None)
                    return;


            }
        }
    }

}