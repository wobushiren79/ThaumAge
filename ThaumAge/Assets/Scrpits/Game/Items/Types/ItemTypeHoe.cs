using UnityEditor;
using UnityEngine;

public class ItemTypeHoe : ItemBaseTool
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
                if (useType == ItemUseTypeEnum.Left)
                {
                    TargetBreak(itemData, targetPosition);
                }
                else
                {

                    Vector3Int localPosition = targetPosition - chunkForHit.chunk.chunkData.positionForWorld;
                    //获取原位置方块
                    Block tagetBlock = chunkForHit.chunk.chunkData.GetBlockForLocal(localPosition);

                    //如果不能锄地
                    if (tagetBlock.blockInfo.plough_state == 0)
                        return;

                    //获取上方方块
                    Block upBlock = chunkForHit.chunk.chunkData.GetBlockForLocal(localPosition + Vector3Int.up);

                    //如果上方有方块 则无法使用锄头
                    if (upBlock != null && upBlock.blockType != BlockTypeEnum.None)
                        return;
                    //扣除道具耐久
                    if (this is ItemBaseTool itemTool)
                    {
                        ItemsMetaTool itemsDetailsTool = itemData.GetMetaData<ItemsMetaTool>();
                        //如果没有耐久 不能锄地
                        if (itemsDetailsTool.curDurability <= 0)
                        {
                            return;
                        }
                        itemsDetailsTool.AddLife(-1);
                        //保存数据
                        itemData.SetMetaData(itemsDetailsTool);
                        //回调
                        EventHandler.Instance.TriggerEvent(EventsInfo.ItemsBean_MetaChange, itemData);
                    }

                    BlockTypeEnum ploughBlockType = (BlockTypeEnum)tagetBlock.blockInfo.remark_int;
                    //替换为耕地方块
                    chunkForHit.chunk.SetBlockForLocal(localPosition, ploughBlockType, direction);

                    //播放粒子特效
                    BlockCptBreak.PlayBlockCptBreakEffect(ploughBlockType, targetPosition + new Vector3(0.5f, 0.5f, 0.5f));

                    //播放音效
                    PlayItemUseSound(itemData);
                }
            }
        }
    }

}