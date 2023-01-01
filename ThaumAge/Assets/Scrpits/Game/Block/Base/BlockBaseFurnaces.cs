using UnityEditor;
using UnityEngine;

public class BlockBaseFurnaces : Block
{
    public override void InitBlock(Chunk chunk, Vector3Int localPosition, int state)
    {
        base.InitBlock(chunk, localPosition, state);
        //刷新的时候注册事件 
        if (state == 0 || state == 1)
            StartWork(chunk, localPosition);
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
        BlockMetaFurnaces blockMetaData = FromMetaData<BlockMetaFurnaces>(blockData.meta);
        if (blockMetaData == null)
            blockMetaData = new BlockMetaFurnaces();

        GameObject objFurnaces = chunk.GetBlockObjForLocal(localPosition);
        //设置烧纸之前的物品
        Transform tfItemBefore = objFurnaces.transform.Find("ItemBefore");
        Transform tfItemAfter = objFurnaces.transform.Find("ItemAfter");
        Transform tfItemFire = objFurnaces.transform.Find("Fire");

        SpriteRenderer srItemBefore = tfItemBefore.GetComponent<SpriteRenderer>();
        SpriteRenderer srItemAfter = tfItemAfter.GetComponent<SpriteRenderer>();

        //如果没有烧制的物品
        if (blockMetaData.itemBeforeId == 0)
        {
            srItemBefore.ShowObj(false);
        }
        else
        {
            srItemBefore.ShowObj(false);
            ItemsHandler.Instance.manager.GetItemsIconById(blockMetaData.itemBeforeId, (iconSp) =>
            {

                srItemBefore.ShowObj(true);
                srItemBefore.sprite = iconSp;
            });
        }

        //如果没有烧制之后的物品
        if (blockMetaData.itemAfterId == 0)
        {
            srItemAfter.ShowObj(false);
        }
        else
        {
            srItemAfter.ShowObj(false);
            ItemsHandler.Instance.manager.GetItemsIconById(blockMetaData.itemAfterId, (iconSp) =>
            {

                srItemAfter.ShowObj(true);
                srItemAfter.sprite = iconSp;
            });
        }

        //设置火焰
        if (blockMetaData.transitionPro <= 0)
        {
            tfItemFire.ShowObj(false);
        }
        else
        {
            tfItemFire.ShowObj(true);
        }
    }

    /// <summary>
    /// 每秒刷新
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="localPosition"></param>
    public override void EventBlockUpdateForSec(Chunk chunk, Vector3Int localPosition)
    {
        base.EventBlockUpdateForSec(chunk, localPosition);
        //获取数据
        BlockBean blockData = chunk.GetBlockData(localPosition);
        BlockMetaFurnaces blockMetaData = FromMetaData<BlockMetaFurnaces>(blockData.meta);
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
                int fireAddRemain = blockMetaData.fireTimeRemain + elementalWood * 10 + elementalFire * 10;
                //限制最大100能量
                if (fireAddRemain < blockMetaData.fireTimeMax)
                {
                    blockMetaData.fireTimeRemain = fireAddRemain;
                    blockMetaData.itemFireSourceNum--;
                }
            }
        }

        //如果没有烧制的物品 或者剩下的烧制时间为0 则结束
        if (blockMetaData.itemBeforeId == 0 || blockMetaData.fireTimeRemain == 0)
        {
            blockMetaData.transitionPro = 0;
            chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
            //保存数据
            SaveFurnacesData(chunk, localPosition, blockData, blockMetaData);
            return;
        }
        //查询烧制之前的物品能否烧制物品
        ItemsInfoBean itemsInfoBefore = ItemsHandler.Instance.manager.GetItemsInfoById(blockMetaData.itemBeforeId);
        if (itemsInfoBefore.fire_items.IsNull())
        {
            blockMetaData.transitionPro = 0;
            //如果是不能烧制的物品 则结束刷新
            chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
            //保存数据
            SaveFurnacesData(chunk, localPosition, blockData, blockMetaData);
            return;
        }

        //获取烧制的结果
        itemsInfoBefore.GetFireItems(out int[] fireItemsId, out int[] fireItemsNum, out int[] fireTime);
        int itemFireTime = fireTime[0];
        //检测是否正在烧制物品
        if (blockMetaData.transitionPro < 1)
        {
            blockMetaData.transitionPro += 1f / itemFireTime;
        }
        else if (blockMetaData.transitionPro >= 1)
        {            
            //烧制完成
            blockMetaData.transitionPro = 0;
            blockMetaData.itemAfterId = fireItemsId[0];
            blockMetaData.itemAfterNum++;
            blockMetaData.itemBeforeNum--;
            chunk.isSaveData = true;
        }
        else
        {
            ItemsInfoBean itemsInfoAfter = ItemsHandler.Instance.manager.GetItemsInfoById(blockMetaData.itemAfterId);
            //如果已经有烧制的物品 并且该物品不等于当前物品烧制后的物品 则也不进行烧制     //如果已经达到物品上限 也不烧制了
            if (blockMetaData.itemAfterId != 0 
                && (blockMetaData.itemAfterId != fireItemsId[0] || itemsInfoAfter.max_number <= blockMetaData.itemAfterNum))
            {
                blockMetaData.transitionPro = 0;
                chunk.UnRegisterEventUpdate(localPosition, TimeUpdateEventTypeEnum.Sec);
                //保存数据
                SaveFurnacesData(chunk, localPosition, blockData, blockMetaData);
                return;
            }
            blockMetaData.transitionPro = 1f / itemFireTime;
        }
        blockMetaData.fireTimeRemain--;
        //保存数据
        SaveFurnacesData(chunk, localPosition, blockData, blockMetaData);
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    protected void SaveFurnacesData(Chunk chunk, Vector3Int localPosition, BlockBean blockData, BlockMetaFurnaces blockMetaData)
    {
        //数据检测
        if (blockMetaData.itemFireSourceNum <= 0)
        {
            blockMetaData.itemFireSourceNum = 0;
            blockMetaData.itemFireSourceId = 0;
        }
        if (blockMetaData.itemAfterNum <= 0)
        {
            blockMetaData.itemAfterNum = 0;
            blockMetaData.itemAfterId = 0;
        }
        if (blockMetaData.itemBeforeNum <= 0)
        {
            blockMetaData.itemBeforeNum = 0;
            blockMetaData.itemBeforeId = 0;
        }
        blockData.meta = ToMetaData(blockMetaData);
        RefreshObjModel(chunk, localPosition);
        chunk.isSaveData = true;
    }

}