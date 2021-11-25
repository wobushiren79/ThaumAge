using UnityEditor;
using UnityEngine;

public class ItemSeed : Item
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
                Block tagetBlock = chunkForHit.chunkData.GetBlockForLocal(localPosition);

                //如果不能种地
                if (tagetBlock.blockInfo.plant_state == 0)
                    return;

                //种植位置
                Vector3Int upLocalPosition = localPosition + Vector3Int.up;
                //获取上方方块
                Block upBlock = chunkForHit.chunkData.GetBlockForLocal(upLocalPosition);

                //如果上方有方块 则无法种植
                if (upBlock != null && upBlock.blockType != BlockTypeEnum.None)
                    return;

                //种植的方块
                BlockTypeEnum plantBlockType = (BlockTypeEnum)itemsInfo.type_id;
                Block plantBlock = BlockHandler.Instance.manager.GetRegisterBlock(plantBlockType);
                //初始化meta数据
                string metaData= BlockPlantExtension.ToMetaData(0,false);
                //替换为种植
                chunkForHit.SetBlockForLocal(upLocalPosition, plantBlockType, DirectionEnum.UP, metaData);
                //更新区块
                WorldCreateHandler.Instance.HandleForUpdateChunk(chunkForHit, upLocalPosition, upBlock, plantBlock, direction, true);
            }
        }
    }
}