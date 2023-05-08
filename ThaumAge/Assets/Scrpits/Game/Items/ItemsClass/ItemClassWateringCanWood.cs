using UnityEditor;
using UnityEngine;

public class ItemClassWateringCanWood : ItemBaseTool
{
    public override bool TargetUseR(GameObject user, ItemsBean itemData, Vector3Int targetPosition, Vector3Int closePosition, BlockDirectionEnum direction)
    {
        bool isBlockUseStop = base.TargetUseR(user, itemData, targetPosition, closePosition, direction);
        if (isBlockUseStop)
            return true;

        //获取目标方块
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(targetPosition, out Block targetBlock, out BlockDirectionEnum targetBlockDirection, out Chunk targetChunk);

        if (targetBlock == null)
            return false;
        if (targetBlock.blockInfo.GetBlockShape() == BlockShapeEnum.Plough
            || targetBlock is BlockBaseCrop)
        {

        }
        else
        {
            return false;
        }
        ItemMetaTool itemsDetailsTool = itemData.GetMetaData<ItemMetaTool>();
        //如果没有耐久了 则不执行
        if (itemsDetailsTool.curDurability <= 0)
        {
            return false;
        }
        //扣除耐久
        itemsDetailsTool.AddLife(-1);
        //保存数据 道具数据
        itemData.SetMetaData(itemsDetailsTool);
        //回调
        EventHandler.Instance.TriggerEvent(EventsInfo.ItemsBean_MetaChange, itemData);

        Vector3Int ploughLocalPosition = targetPosition - targetChunk.chunkData.positionForWorld;
        BlockBasePlough blockPlough = null;
        //修改耕地的状态
        if (targetBlock.blockInfo.GetBlockShape() == BlockShapeEnum.Plough)
        {
            blockPlough = targetBlock as BlockBasePlough;
        }
        else if (targetBlock is BlockBaseCrop)
        {
            //如果是植物 则获取他下方的一个方块 
            targetBlock.GetCloseBlockByDirection(targetChunk, ploughLocalPosition, DirectionEnum.Down, out Block blockDown, out Chunk blockChunkDown, out Vector3Int localPositionDown);
            ploughLocalPosition = localPositionDown;
            if (blockDown != null && blockDown is BlockBasePlough)
            {
                blockPlough = blockDown as BlockBasePlough;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
        //改变耕地状态
        blockPlough.ChangeWaterState(targetChunk, ploughLocalPosition,1);

        //播放音效
        PlayItemSoundUse(itemData, ItemUseTypeEnum.Right);
        //播放浇水粒子特效
        EffectBean effectData = new EffectBean();
        effectData.effectName = EffectInfo.Effect_Water_1;
        effectData.effectType = EffectTypeEnum.Visual;
        effectData.effectPosition = ploughLocalPosition + new Vector3(0.5f, 0.5f, 0.5f);
        effectData.timeForShow = 2;

        EffectHandler.Instance.ShowEffect(effectData);
        return false;
    }
}