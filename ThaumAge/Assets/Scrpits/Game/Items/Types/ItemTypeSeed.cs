using UnityEditor;
using UnityEngine;

public class ItemTypeSeed : Item
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
                //挖掘
                if (useType == ItemUseTypeEnum.Left)
                {
                    TargetBreak(itemData, targetPosition);
                    return;
                }

                Vector3Int localPosition = targetPosition - chunkForHit.chunk.chunkData.positionForWorld;
                //获取原位置方块
                Block tagetBlock = chunkForHit.chunk.chunkData.GetBlockForLocal(localPosition);

                //如果不能种地
                if (tagetBlock.blockInfo.plant_state == 0)
                    return;

                //种植位置
                Vector3Int upLocalPosition = localPosition + Vector3Int.up;
                //获取上方方块
                Block upBlock = chunkForHit.chunk.chunkData.GetBlockForLocal(upLocalPosition);

                //如果上方有方块 则无法种植
                if (upBlock != null && upBlock.blockType != BlockTypeEnum.None)
                    return;

                //种植的方块
                ItemsInfoBean itemsInfo = GetItemsInfo(itemData.itemId);
                BlockTypeEnum plantBlockType = (BlockTypeEnum)itemsInfo.type_id;
                //初始化meta数据
                BlockMetaCrop blockCropData = new BlockMetaCrop();
                blockCropData.isStartGrow = false;
                blockCropData.growPro = 0;
                string metaData = BlockBaseCrop.ToMetaData(blockCropData);
                //替换为种植
                chunkForHit.chunk.SetBlockForLocal(upLocalPosition, plantBlockType, BlockDirectionEnum.UpForward, metaData);

                //扣除道具
                UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
                userData.AddItems(itemData, -1);
                //刷新UI
                UIHandler.Instance.RefreshUI();
                //播放音效
                PlayItemUseSound(itemData);
            }
        }
    }

    /// <summary>
    /// 播放播种的声音
    /// </summary>
    /// <param name="itemsData"></param>
    public override void PlayItemUseSound(ItemsBean itemsData)
    {
        AudioHandler.Instance.PlaySound(602);
    }
}