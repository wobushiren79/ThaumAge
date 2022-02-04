using UnityEditor;
using UnityEngine;

public class ItemTypeHoe : Item
{
    protected override void UseForPlayer(Player player, ItemsBean itemData,int type)
    {
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
                Block tagetBlock = chunkForHit.chunkData.GetBlockForLocal(localPosition);

                //如果不能锄地
                if (tagetBlock.blockInfo.plough_state == 0)
                    return;

                //获取上方方块
                Block upBlock = chunkForHit.chunkData.GetBlockForLocal(localPosition + Vector3Int.up);

                //如果上方有方块 则无法使用锄头
                if (upBlock != null && upBlock.blockType != BlockTypeEnum.None)
                    return;

                Vector3 face = Vector3.Normalize(player.transform.position - hit.point);
                int rotate = Mathf.Abs(face.x) > Mathf.Abs(face.z) ? 0 : 1;

                BlockTypeEnum ploughBlockType = (BlockTypeEnum)tagetBlock.blockInfo.plough_change;
                //替换为耕地方块
                chunkForHit.SetBlockForLocal(localPosition, ploughBlockType, direction, BlockBasePlough.ToMetaData(rotate));

                //播放粒子特效
                BlockCptBreak.PlayBlockCptBreakEffect(ploughBlockType, targetPosition + new Vector3(0.5f, 0.5f, 0.5f));
            }
        }
    }

}