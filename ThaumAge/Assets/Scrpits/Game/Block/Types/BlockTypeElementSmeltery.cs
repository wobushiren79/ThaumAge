using UnityEditor;
using UnityEngine;

public class BlockTypeElementSmeltery : Block
{
    /// <summary>
    /// 打开UI
    /// </summary>
    public override void Interactive(GameObject user, Vector3Int worldPosition, BlockDirectionEnum direction)
    {
        base.Interactive(user, worldPosition, direction);

        //打开箱子UI
        UIGameElementSmeltery uiGameElementSmeltery = UIHandler.Instance.OpenUIAndCloseOther<UIGameElementSmeltery>();
        //设置数据
        uiGameElementSmeltery.SetData(worldPosition);

        AudioHandler.Instance.PlaySound(1);
    }


    public override void InitBlock(Chunk chunk, Vector3Int localPosition, int state)
    {
        base.InitBlock(chunk, localPosition, state);
        if (state == 0 || state == 1)
        {
            StartWork(chunk, localPosition);
        }
    }

    /// <summary>
    /// 开始工作
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    public void StartWork(Chunk chunk, Vector3Int localPosition)
    {
        chunk.RegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
    }

    /// <summary>
    /// 刷新方块模型
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    public override void RefreshObjModel(Chunk chunk, Vector3Int localPosition)
    {
        BlockBean blockData = chunk.GetBlockData(localPosition);
        BlockMetaElementSmeltery blockMetaData = FromMetaData<BlockMetaElementSmeltery>(blockData.meta);
        if (blockMetaData == null)
            blockMetaData = new BlockMetaElementSmeltery();

        //GameObject objFurnaces = chunk.GetBlockObjForLocal(localPosition);
        ////设置烧纸之前的物品
        //Transform tfItemFire = objFurnaces.transform.Find("Fire");

        ////设置火焰
        //if (blockMetaData.transitionPro <= 0)
        //{
        //    tfItemFire.ShowObj(false);
        //}
        //else
        //{
        //    tfItemFire.ShowObj(true);
        //}
    }

    /// <summary>
    /// 每秒刷新
    /// </summary>
    public override void EventBlockUpdateForSec(Chunk chunk, Vector3Int localPosition)
    {
        base.EventBlockUpdateForSec(chunk, localPosition);
        //获取数据
        BlockBean blockData = chunk.GetBlockData(localPosition);
        BlockMetaElementSmeltery blockMetaData = FromMetaData<BlockMetaElementSmeltery>(blockData.meta);

        bool isDataChange = false;
        //如果没有数据
        if (blockMetaData == null)
        {
            chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
            return;
        }
        //首先添加烧制能量材料
        if (blockMetaData.itemFireSourceId != 0)
        {
            ItemsInfoBean itemsInfoFire = ItemsHandler.Instance.manager.GetItemsInfoById(blockMetaData.itemFireSourceId);
            //拥有相应元素 能够烧制 一个元素能烧制10秒
            int elementalWood = itemsInfoFire.GetElemental(ElementalTypeEnum.Wood);
            int elementalFire = itemsInfoFire.GetElemental(ElementalTypeEnum.Fire);
            if (elementalWood != 0 || elementalFire != 0)
            {
                int addFireAddRemain = elementalWood * 10 + elementalFire * 10;

                if (blockMetaData.fireTimeRemain + addFireAddRemain <= blockMetaData.fireTimeMax)
                {
                    blockMetaData.AddFireTimeRemain(addFireAddRemain);
                    blockMetaData.itemFireSourceNum--;
                    isDataChange = true;
                }
            }
        }
        //如果有烧纸的物品则开始烧纸
        float elementalPro = blockMetaData.GetElementalPro();
        //如果容器没满 并且还有可炼制的物品
        if (elementalPro != 1 && blockMetaData.itemBeforeId != 0 && blockMetaData.itemBeforeNum != 0)
        {
            //获取烧制的时间
            int itemFireTime = 5;
            //检测是否正在烧制物品
            if (blockMetaData.transitionPro < 1)
            {
                blockMetaData.transitionPro += 1f / itemFireTime;
            }
            else if (blockMetaData.transitionPro >= 1)
            {
                ItemsInfoBean itemsInfoBefore = ItemsHandler.Instance.manager.GetItemsInfoById(blockMetaData.itemBeforeId);
                itemsInfoBefore.GetAllElemental();
                //烧制完成
                blockMetaData.transitionPro = 0;
                blockMetaData.itemBeforeNum--;
                //增加元素
                var allElemental = itemsInfoBefore.GetAllElemental();
                foreach (var itemElemental in allElemental)
                {
                    ElementalTypeEnum elementalType = itemElemental.Key;
                    int count = itemElemental.Value;
                    for (int i = 0; i < count; i++)
                    {
                        bool isAdd = blockMetaData.AddElemental(elementalType);
                        if (!isAdd)
                            break;
                    }
                }
            }
            else
            {
                blockMetaData.transitionPro = 1f / itemFireTime;
            }
            blockMetaData.AddFireTimeRemain(-1);
            isDataChange = true;
        }

        //检测上方3个位置是否有ArcaneAlembic 奥术蒸馏器
        bool isUp1Flow = false;
        bool isUp2Flow = false;
        bool isUp3Flow = false;

        GetCloseBlockByDirection(chunk, localPosition, DirectionEnum.UP, out Block up1Block, out Chunk up1Chunk, out Vector3Int up1LocalPosition);
        isUp1Flow = HandleForFlowElemental(blockMetaData, up1Block, up1Chunk, up1LocalPosition);
        if (!isUp1Flow)
        {
            GetCloseBlockByDirection(chunk, up1LocalPosition, DirectionEnum.UP, out Block up2Block, out Chunk up2Chunk, out Vector3Int up2LocalPosition);
            isUp2Flow = HandleForFlowElemental(blockMetaData, up2Block, up2Chunk, up2LocalPosition);
            if (!isUp2Flow)
            {
                GetCloseBlockByDirection(chunk, up2LocalPosition, DirectionEnum.UP, out Block up3Block, out Chunk up3Chunk, out Vector3Int up3LocalPosition);
                isUp3Flow = HandleForFlowElemental(blockMetaData, up3Block, up3Chunk, up3LocalPosition);
                if (!isUp3Flow)
                {

                }
            }
        }
        if (isUp1Flow || isUp2Flow || isUp3Flow)
        {
            isDataChange = true;
        }
        //保存数据
        if (isDataChange)
        {
            SaveData(chunk, localPosition, blockData, blockMetaData);
        }
    }

    /// <summary>
    /// 处理元素流动
    /// </summary>
    protected bool HandleForFlowElemental(BlockMetaElementSmeltery blockMetaData, Block targetBlock, Chunk targetChunk, Vector3Int targetLocalPosition)
    {
        if (targetBlock != null && targetBlock.blockType == BlockTypeEnum.ArcaneAlembic)
        {
            BlockTypeArcaneAlembic blockTypeArcane = targetBlock as BlockTypeArcaneAlembic;
            blockTypeArcane.GetBlockMetaData(targetChunk, targetLocalPosition,
                out BlockBean blockData, out BlockMetaArcaneAlembic blockMetaArcaneAlembic);

            bool isSubElemental = blockMetaData.SubElemental(out ElementalTypeEnum subElemental);
            if (isSubElemental)
            {
                bool isAddElemental = blockMetaArcaneAlembic.AddElemental(subElemental, 1);
                if(isAddElemental)
                {
                    blockData.SetBlockMeta(blockMetaArcaneAlembic);
                    targetChunk.SetBlockData(blockData);
                    return true;
                }  
            }
        }
        return false;
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    protected void SaveData(Chunk chunk, Vector3Int localPosition, BlockBean blockData, BlockMetaElementSmeltery blockMetaData)
    {
        //数据检测
        if (blockMetaData.itemFireSourceNum <= 0)
        {
            blockMetaData.itemFireSourceNum = 0;
            blockMetaData.itemFireSourceId = 0;
        }
        if (blockMetaData.itemBeforeNum <= 0)
        {
            blockMetaData.itemBeforeNum = 0;
            blockMetaData.itemBeforeId = 0;
        }
        RefreshObjModel(chunk, localPosition);
        blockData.SetBlockMeta(blockMetaData);
        chunk.SetBlockData(blockData);

        //暂时不通知 如果有很多熔炉 每个都更新很耗资源 直接在UI里去检测比较好
        //EventHandler.Instance.TriggerEvent(EventsInfo.BlockTypeElementSmeltery_Update, localPosition + chunk.chunkData.positionForWorld);
    }
}