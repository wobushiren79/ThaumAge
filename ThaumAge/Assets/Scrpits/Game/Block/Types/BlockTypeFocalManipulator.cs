using UnityEditor;
using UnityEngine;

public class BlockTypeFocalManipulator : Block
{
    public override void CreateBlockModelSuccess(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum blockDirection, GameObject obj)
    {
        base.CreateBlockModelSuccess(chunk, localPosition, blockDirection, obj);

        //获取方块数据
        BlockBean blockData = chunk.GetBlockData(localPosition);
        BlockMetaFocalManipulator blockMetaData = blockData.GetBlockMeta<BlockMetaFocalManipulator>();
        if (blockMetaData == null)
            blockMetaData = new BlockMetaFocalManipulator();
        if (blockMetaData.itemMagicCore != null && blockMetaData.itemMagicCore.itemId != 0)
        {
            SetMagicCore(localPosition + chunk.chunkData.positionForWorld, blockMetaData.itemMagicCore);
        }
    }

    public override void Interactive(GameObject user, Vector3Int worldPosition, BlockDirectionEnum direction)
    {
        base.Interactive(user, worldPosition, direction);
        //打开UI
        UIGameFocalManipulator uiGameFocalManipulator = UIHandler.Instance.OpenUIAndCloseOther<UIGameFocalManipulator>();
        uiGameFocalManipulator.SetData(worldPosition);

        AudioHandler.Instance.PlaySound(1);
    }


    public override void EventBlockUpdateForSec(Chunk chunk, Vector3Int localPosition)
    {
        base.EventBlockUpdateForSec(chunk, localPosition);
        BlockBean blockData = chunk.GetBlockData(localPosition);
        if (blockData == null)
        {
            chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
            return;
        }
        BlockMetaFocalManipulator blockMetaData = blockData.GetBlockMeta<BlockMetaFocalManipulator>();
        if (blockMetaData == null)
        {
            chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
            return;
        }
        blockMetaData.workPro += 0.1f;
        if (blockMetaData.workPro >= 1)
        {
            blockMetaData.workPro = 0;
            chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
            AudioHandler.Instance.PlaySound(1101);

            //展示粒子效果
            GameObject objEffect = SetWorkEffect(localPosition + chunk.chunkData.positionForWorld, false);

            EffectBean effectData = new EffectBean();
            effectData.effectName = EffectInfo.Effect_Smoke_3;
            effectData.effectPosition = objEffect.transform.position;
            effectData.timeForShow = 5;
            EffectHandler.Instance.ShowEffect(effectData);
        }

        blockData.SetBlockMeta(blockMetaData);
        chunk.SetBlockData(blockData);

        //事件通知更新
        EventHandler.Instance.TriggerEvent(EventsInfo.BlockTypeFocalManipulator_UpdateWork, localPosition + chunk.chunkData.positionForWorld);
    }


    /// <summary>
    /// 开始工作
    /// </summary>
    public void StartWork(Chunk targetChunk, Vector3Int worldPosition)
    {
        Vector3Int blockLocalPosition = worldPosition - targetChunk.chunkData.positionForWorld;
        targetChunk.RegisterEventUpdate(blockLocalPosition, TimeUpdateEventTypeEnum.Sec);
        EventBlockUpdateForSec(targetChunk, worldPosition - targetChunk.chunkData.positionForWorld);
        AudioHandler.Instance.PlaySound(1103);

        //展示粒子效果
        SetWorkEffect(worldPosition, true);
    }

    /// <summary>
    /// 展示工作粒子效果
    /// </summary>
    public GameObject SetWorkEffect(Vector3Int worldPosition,bool isShow)
    {
        GameObject objBlock = GetBlockObj(worldPosition);
        Transform tfEffect = objBlock.transform.Find("Effect");
        tfEffect.ShowObj(isShow);
        return tfEffect.gameObject;
    }

    /// <summary>
    /// 设置魔法核心
    /// </summary>
    public void SetMagicCore(Vector3Int worldPosition, ItemsBean itemsData)
    {
        GameObject objBlock = GetBlockObj(worldPosition);
        Transform tfMagicCore = objBlock.transform.Find("MagicCore");

        if (itemsData == null || itemsData.itemId == 0)
        {
            tfMagicCore.ShowObj(false);
        }
        else
        {
            tfMagicCore.ShowObj(true);
            ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoById(itemsData.itemId);
            ItemCptShow itemCpt = tfMagicCore.GetComponent<ItemCptShow>();
            itemCpt.SetItem(itemsData, itemsInfo, 1);
        }
    }
}