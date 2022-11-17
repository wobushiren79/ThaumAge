using UnityEditor;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class BlockTypeCrucible : Block
{

    /// <summary>
    /// 对着空手使用
    /// </summary>
    /// <param name="targetChunk"></param>
    /// <param name="targetWorldPosition"></param>
    public override void TargetUseBlock(GameObject user, ItemsBean itemData, Chunk targetChunk, Vector3Int targetWorldPosition)
    {
        base.TargetUseBlock(user, itemData, targetChunk, targetWorldPosition);

        //保存坩埚数据
        Vector3Int blockLocalPosition = targetWorldPosition - targetChunk.chunkData.positionForWorld;
        BlockDirectionEnum blockDirection = targetChunk.chunkData.GetBlockDirection(blockLocalPosition.x, blockLocalPosition.y, blockLocalPosition.z);

        GeteCrucibleData(targetChunk, blockDirection, targetWorldPosition, out BlockBean blockData, out BlockMetaCrucible blockMetaData);
        SaveCrucibleData(targetChunk,  targetWorldPosition, blockData, blockMetaData, 0);

        EventHandler.Instance.TriggerEvent(EventsInfo.BlockTypeCrucible_UpdateElemental, targetWorldPosition);
    }

    /// <summary>
    /// 放置道具
    /// </summary>
    /// <param name="blockWorldPosition"></param>
    /// <param name="itemData"></param>
    /// <returns></returns>
    public override bool SetItems(Chunk targetChunk, Block targetBlock, BlockDirectionEnum targetBlockDirection, Vector3Int blockWorldPosition, ItemsBean itemData)
    {
        if (itemData != null)
        {
            //如果是水桶 并且里面装的是水
            ItemMetaBuckets itemMetaBuckets = itemData.GetMetaData<ItemMetaBuckets>();
            if (itemMetaBuckets != null && itemMetaBuckets.itemIdForSomething == 109001)
            {
                //设置水桶空了
                itemMetaBuckets.itemIdForSomething = 0;
                itemData.SetMetaData(itemMetaBuckets);
                //保存坩埚数据
                GeteCrucibleData(targetChunk, targetBlockDirection, blockWorldPosition, out BlockBean blockData, out BlockMetaCrucible blockMetaData);
                SaveCrucibleData(targetChunk, blockWorldPosition, blockData, blockMetaData, 5);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 开始合成
    /// </summary>
    /// <returns></returns>
    public void StartSynthesis(Vector3Int blockWorldPosition, ItemsBean itemData,out int numberSynthesis)
    {
        numberSynthesis = 0;
        GeteCrucibleData(blockWorldPosition, out BlockBean blockData, out BlockMetaCrucible blockMetaData);
        List<ItemsSynthesisBean> listSynthesis = ItemsSynthesisCfg.GetItemsSynthesisForCrucible();
        for (int i = 0; i < listSynthesis.Count; i++)
        {
            ItemsSynthesisBean itemSynthesisData = listSynthesis[i];
            itemSynthesisData.CheckSynthesisForCrucible(blockMetaData.listElemental, itemData,out numberSynthesis);
            if (numberSynthesis > 0)
            {
                //减少水
                blockMetaData.waterLevel--;
                if (blockMetaData.waterLevel < 0)
                    blockMetaData.waterLevel = 0;

                //减少元素 * numberSynthesis
                Dictionary<ElementalTypeEnum, int> dicElemental = itemSynthesisData.GetElemental();
                blockMetaData.SubElemental(dicElemental, numberSynthesis);
                EventHandler.Instance.TriggerEvent(EventsInfo.BlockTypeCrucible_UpdateElemental, blockWorldPosition);

                //生成道具 * numberSynthesis
                itemSynthesisData.GetSynthesisResult(out long itemIdSynthesisResult, out int itemNumberSynthesisResult);
                ItemsBean itemSynthesisResult = new ItemsBean(itemIdSynthesisResult, itemNumberSynthesisResult * numberSynthesis);

                ItemDropBean itemDropData = new ItemDropBean(itemSynthesisResult, ItemDropStateEnum.DropPick, blockWorldPosition + new Vector3(0.5f, 1.1f, 0.5f), Vector3.up * 1.5f);
                ItemsHandler.Instance.CreateItemCptDrop(itemDropData,(drop)=> 
                {
                    drop.canInteractiveBlock = false;
                });

                blockData.SetBlockMeta(blockMetaData);

                Chunk targetChunk = WorldCreateHandler.Instance.manager.GetChunkForWorldPosition(blockWorldPosition);

                SaveCrucibleData(targetChunk, blockWorldPosition, blockData, blockMetaData);
                //播放特效
                PlaySynthesisEffect(blockWorldPosition + new Vector3(0.5f, 1f, 0.5f));

                AudioHandler.Instance.PlaySound(1103);
                return;
            }
        }
        return;
    }

    /// <summary>
    /// 增加元素
    /// </summary>
    /// <param name="listElemental"></param>
    /// <returns></returns>
    public bool AddElemental(Vector3Int blockWorldPosition, List<NumberBean> listElemental)
    {
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(blockWorldPosition, out Block targetBlock, out BlockDirectionEnum targetBlockDirection, out Chunk targetChunk);
        GeteCrucibleData(targetChunk, targetBlockDirection, blockWorldPosition, out BlockBean blockData, out BlockMetaCrucible blockMetaData);
        //如果没有水 则不添加
        if (blockMetaData.waterLevel == 0)
        {
            return false;
        }
        bool isBoiling = CheckIsBoiling(targetChunk, blockWorldPosition);
        //如果没有烧开
        if (!isBoiling)
        {
            return false;
        }
        if (targetChunk != null && targetBlock != null)
        {
            SaveCrucibleData(targetChunk, blockWorldPosition, blockData, blockMetaData, addListElemental: listElemental);
            EventHandler.Instance.TriggerEvent(EventsInfo.BlockTypeCrucible_UpdateElemental, blockWorldPosition);

            float waterPlaneY = GetWaterLevelY(blockMetaData.waterLevel);
            PlayAddElementalEffect(blockWorldPosition + new Vector3(0.5f, 0.5f + waterPlaneY, 0.5f));
            AudioHandler.Instance.PlaySound(703);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 设置坩埚数据
    /// </summary>
    public void SaveCrucibleData(Chunk targetChunk, Vector3Int blockWorldPosition,  BlockBean blockData,  BlockMetaCrucible blockMetaData,
        int waterLevel = -1, bool isCleanElemental = false, List<NumberBean> addListElemental = null)
    {
        //是否设置水量
        if (waterLevel != -1)
            blockMetaData.waterLevel = waterLevel;
        //是否清除元素
        if (isCleanElemental && !blockMetaData.listElemental.IsNull())
        {
            blockMetaData.listElemental.Clear();
        }
        if (!addListElemental.IsNull())
        {
            blockMetaData.AddElemental(addListElemental);
        }
        //如果没有水了。也要清除元素
        if (blockMetaData.waterLevel == 0 && !blockMetaData.listElemental.IsNull())
        {
            blockMetaData.listElemental.Clear();
        }
        blockData.SetBlockMeta(blockMetaData);
        targetChunk.isSaveData = true;

        bool isBoiling = CheckIsBoiling(targetChunk, blockWorldPosition);
        SetWaterShow(blockWorldPosition, blockMetaData.waterLevel, isBoiling);

    }

    /// <summary>
    /// 获取坩埚数据
    /// </summary>
    public void GeteCrucibleData(Chunk targetChunk, BlockDirectionEnum targetBlockDirection, Vector3Int blockWorldPosition,
        out BlockBean blockData, out BlockMetaCrucible blockMetaData)
    {
        Vector3Int blockLocalPosition = blockWorldPosition - targetChunk.chunkData.positionForWorld;
        blockData = targetChunk.GetBlockData(blockLocalPosition);
        if (blockData == null)
        {
            blockData = new BlockBean(blockLocalPosition, blockType, targetBlockDirection);
        }
        blockMetaData = blockData.GetBlockMeta<BlockMetaCrucible>();
        if (blockMetaData == null)
            blockMetaData = new BlockMetaCrucible();
    }
    public void GeteCrucibleData(Vector3Int blockWorldPosition, out BlockBean blockData, out BlockMetaCrucible blockMetaData)
    {
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(blockWorldPosition, out Block targetBlock, out BlockDirectionEnum blockDirection, out Chunk targetChunk);
        GeteCrucibleData(targetChunk, blockDirection, blockWorldPosition, out blockData, out blockMetaData);
    }


    public override void RefreshBlock(Chunk chunk, Vector3Int localPosition, BlockDirectionEnum direction)
    {
        base.RefreshBlock(chunk, localPosition, direction);
        chunk.RegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
    }

    public override void BuildBlock(Chunk chunk, Vector3Int localPosition)
    {
        base.BuildBlock(chunk, localPosition);
        chunk.RegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
    }

    public override void EventBlockUpdateForSec(Chunk chunk, Vector3Int localPosition)
    {
        base.EventBlockUpdateForSec(chunk, localPosition);
        InitWater(chunk, localPosition);
        chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
    }

    /// <summary>
    /// 检测是否煮沸
    /// </summary>
    /// <returns></returns>
    public bool CheckIsBoiling(Chunk targetChunk, Vector3Int blockWorldPosition)
    {
        Vector3Int blockLocalPosition = blockWorldPosition - targetChunk.chunkData.positionForWorld;
        GetCloseBlockByDirection(targetChunk, blockLocalPosition, DirectionEnum.Down, out Block downBlock, out Chunk downChunk, out Vector3Int downLocalPosition);
        if (downChunk != null && downBlock != null)
        {
            ItemsInfoBean itemInfoBlock = ItemsHandler.Instance.manager.GetItemsInfoByBlockId(downBlock.blockInfo.id);
            if (itemInfoBlock.GetElemental(ElementalTypeEnum.Fire) >= 5)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 初始化水的状态
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    public void InitWater(Chunk chunk, Vector3Int localPosition)
    {
        Vector3Int targetWorldPosition = localPosition + chunk.chunkData.positionForWorld;
        BlockDirectionEnum targetBlockDirection = chunk.chunkData.GetBlockDirection(localPosition);
        //获取坩埚数据
        GeteCrucibleData(chunk, targetBlockDirection, localPosition + chunk.chunkData.positionForWorld, out BlockBean blockData, out BlockMetaCrucible blockMetaData);
        //获取是否烧开
        bool isBoiling = CheckIsBoiling(chunk, targetWorldPosition);
        //设置水位
        SetWaterShow(targetWorldPosition, blockMetaData.waterLevel, isBoiling);
    }

    /// <summary>
    /// 设置水面
    /// </summary>
    /// <param name="level">0没有水 5满水</param>
    /// <param name="isBoiling">是否沸腾</param>
    public void SetWaterShow(Vector3Int blockWorldPosition, int level, bool isBoiling)
    {
        GameObject objBlock = GetBlockObj(blockWorldPosition);
        if (objBlock == null)
        {
            return;
        }
        Transform tfWaterPlane = objBlock.transform.Find("WaterPlane");
        Transform tfBoiling = objBlock.transform.Find("WaterPlane/Effect_WaterBoiling_1");
        float waterPlaneY = GetWaterLevelY(level);
        if (level == 0)
        {
            tfWaterPlane.ShowObj(false);
            tfBoiling.ShowObj(false);
            tfWaterPlane.DOKill();
            return;
        }
        tfWaterPlane.DOLocalMoveY(waterPlaneY, 1);
        tfWaterPlane.ShowObj(true);
        if (isBoiling)
        {
            tfBoiling.ShowObj(true);
        }
        else
        {
            tfBoiling.ShowObj(false);
        }
    }
    /// <summary>
    /// 获取不同等级水位高度
    /// </summary>
    protected float GetWaterLevelY(int level)
    {
        switch (level)
        {
            case 0:
                return 0 ;
            case 1:
                return 0.05f;
            case 2:
                return 0.1f;
            case 3:
                return 0.2f;
            case 4:
                return 0.3f;
            case 5:
                return 0.4f;
            default:
                return 0;
        }
    }

    public void PlayAddElementalEffect(Vector3 position)
    {
        EffectBean effectData = new EffectBean();
        effectData.effectName = EffectInfo.Effect_WaterBoiling_2;
        effectData.effectPosition = position;
        effectData.timeForShow = 3;
        EffectHandler.Instance.ShowEffect(effectData, (effect)=> 
        { 
        
        
        });
    }

    public void PlaySynthesisEffect(Vector3 position)
    {
        EffectBean effectData = new EffectBean();
        effectData.effectName = EffectInfo.Effect_Smoke_3;
        effectData.effectPosition = position;
        effectData.timeForShow = 5;
        EffectHandler.Instance.ShowEffect(effectData, (effect) =>
        {


        });
    }
}