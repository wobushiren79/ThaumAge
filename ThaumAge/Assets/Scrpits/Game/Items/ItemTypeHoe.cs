using UnityEditor;
using UnityEngine;

public class ItemTypeHoe : ItemBaseTool
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
                player.playerRay.GetHitPositionAndDirection(hit, out Vector3Int targetPosition, out Vector3Int closePosition, out BlockDirectionEnum direction);

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
                //扣除道具耐久
                if (this is ItemBaseTool itemTool)
                {
                    ItemsDetailsToolBean itemsDetailsTool = itemData.GetMetaData<ItemsDetailsToolBean>();
                    //如果没有耐久 不能锄地
                    if (itemsDetailsTool.life <=0)
                    {
                        return;
                    }
                    itemsDetailsTool.AddLife(-1);
                    //保存数据
                    itemData.SetMetaData(itemsDetailsTool);
                    //回调
                    EventHandler.Instance.TriggerEvent(EventsInfo.ItemsBean_MetaChange, itemData);
                }

                BlockTypeEnum ploughBlockType = (BlockTypeEnum)tagetBlock.blockInfo.plough_change;
                //替换为耕地方块
                chunkForHit.SetBlockForLocal(localPosition, ploughBlockType, direction);

                //播放粒子特效
                BlockCptBreak.PlayBlockCptBreakEffect(ploughBlockType, targetPosition + new Vector3(0.5f, 0.5f, 0.5f));


            }
        }
    }

}