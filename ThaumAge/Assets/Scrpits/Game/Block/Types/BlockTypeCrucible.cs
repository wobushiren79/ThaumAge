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
        SaveCrucibleData(targetChunk, blockDirection, targetWorldPosition, 0, true);
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
                SaveCrucibleData(targetChunk, targetBlockDirection, blockWorldPosition, 5);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 开始合成
    /// </summary>
    /// <returns></returns>
    public bool StartSynthesis(Vector3Int blockWorldPosition, ItemsBean itemData)
    {
        GeteCrucibleData(blockWorldPosition, out BlockBean blockData, out BlockMetaCrucible blockMetaData);
        List<ItemsSynthesisBean> listSynthesis = ItemsHandler.Instance.manager.GetItemsSynthesisForCrucible();
        for (int i = 0; i < listSynthesis.Count; i++)
        {
            ItemsSynthesisBean itemSynthesisData = listSynthesis[i];
            bool canSynthesis = itemSynthesisData.CheckSynthesisForCrucible(blockMetaData.listElemental, (int)itemData.itemId);
            if (canSynthesis)
            {
                //减少水
                blockMetaData.waterLevel--;
                if (blockMetaData.waterLevel < 0)
                    blockMetaData.waterLevel = 0;
                //减少元素
                Dictionary<ElementalTypeEnum, int> dicElemental = itemSynthesisData.GetElemental();
                blockMetaData.SubElemental(dicElemental);
                EventHandler.Instance.TriggerEvent(EventsInfo.BlockTypeCrucible_UpdateElemental, blockWorldPosition);
                //生成道具
                ItemDropBean itemDropData = new ItemDropBean(itemData, ItemDropStateEnum.DropPick, blockWorldPosition + new Vector3(0.5f, 1.1f, 0.5f), Vector3.up * 1.5f);
                ItemsHandler.Instance.CreateItemCptDrop(itemDropData,(drop)=> 
                {
                    drop.canInteractiveBlock = false;
                });


                blockData.SetBlockMeta(blockMetaData);
                Chunk targetChunk = WorldCreateHandler.Instance.manager.GetChunkForWorldPosition(blockWorldPosition);
                targetChunk.isSaveData = true;
                return true;
            }
        }
        return false;
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
            SaveCrucibleData(targetChunk, targetBlockDirection, blockWorldPosition, listElemental: listElemental);
            EventHandler.Instance.TriggerEvent(EventsInfo.BlockTypeCrucible_UpdateElemental, blockWorldPosition);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 设置坩埚数据
    /// </summary>
    public void SaveCrucibleData(Chunk targetChunk, BlockDirectionEnum targetBlockDirection, Vector3Int blockWorldPosition,
        int waterLevel = -1, bool isCleanElemental = false, List<NumberBean> listElemental = null)
    {
        GeteCrucibleData(targetChunk, targetBlockDirection, blockWorldPosition, out BlockBean blockData, out BlockMetaCrucible blockMetaData);
        //是否设置水量
        if (waterLevel != -1)
            blockMetaData.waterLevel = waterLevel;
        blockData.meta = blockMetaData.ToJson();
        //是否清除元素
        if (isCleanElemental && !blockMetaData.listElemental.IsNull())
        {
            blockMetaData.listElemental.Clear();
        }
        if (!listElemental.IsNull())
        {
            blockMetaData.AddElemental(listElemental);
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
        float waterPlaneY = 0;
        switch (level)
        {
            case 0:
                tfWaterPlane.ShowObj(false);
                tfBoiling.ShowObj(false);
                tfWaterPlane.DOKill();
                return;
            case 1:
                waterPlaneY = 0.05f;
                break;
            case 2:
                waterPlaneY = 0.1f;
                break;
            case 3:
                waterPlaneY = 0.2f;
                break;
            case 4:
                waterPlaneY = 0.3f;
                break;
            case 5:
                waterPlaneY = 0.4f;
                break;
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
}