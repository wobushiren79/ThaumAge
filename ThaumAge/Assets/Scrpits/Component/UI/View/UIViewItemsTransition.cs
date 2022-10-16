using UnityEditor;
using UnityEngine;

public partial class UIViewItemsTransition : BaseUIView
{
    protected ItemsBean itemsBefore;
    protected ItemsBean itemsAfter;

    protected BlockBean blockData;
    protected BlockMetaItemsTransition blockMetaData;
    protected Chunk targetBlockChunk;
    protected Block targetBlock;

    public float timeUpdate = 0;

    protected Vector3Int blockWorldPosition;

    public override void Awake()
    {
        base.Awake();
        ui_BeforeItems.SetCallBackForSetViewItem(CallBackForItemsChange);
        ui_AfterItems.SetCallBackForSetViewItem(CallBackForItemsChange);
    }

    public void Update()
    {
        timeUpdate += Time.deltaTime;
        if (timeUpdate >= 0.1f)
        {
            timeUpdate = 0;
            RefreshUI();
        }
    }

    public void SetData(Vector3Int blockWorldPosition)
    {
        this.blockWorldPosition = blockWorldPosition;
        //获取相关数据
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(blockWorldPosition, out targetBlock, out targetBlockChunk);
        BlockBean blockData = targetBlockChunk.GetBlockData(blockWorldPosition - targetBlockChunk.chunkData.positionForWorld);
        this.blockData = blockData;
        if (this.blockData == null)
            this.blockData = new BlockBean();

        itemsBefore = new ItemsBean();
        itemsAfter = new ItemsBean();

        RefreshUI();
    }



    public override void RefreshUI()
    {
        base.RefreshUI();

        blockMetaData = Block.FromMetaData<BlockMetaItemsTransition>(blockData.meta);

        if (blockMetaData == null)
            blockMetaData = new BlockMetaItemsTransition();

        itemsBefore.itemId = blockMetaData.itemBeforeId;
        itemsBefore.number = blockMetaData.itemBeforeNum;

        itemsAfter.itemId = blockMetaData.itemAfterId;
        itemsAfter.number = blockMetaData.itemAfterNum;

        SetBeforeItems(itemsBefore);
        SetAfterItems(itemsAfter);
        SetTransitionPro(blockMetaData.transitionPro);
    }

    /// <summary>
    /// 设置进度
    /// </summary>
    public void SetTransitionPro(float firePro)
    {
        if (ui_TransitionPro.value > firePro)
        {
            ui_TransitionPro.value = 0;
        }
        ui_TransitionPro.value = firePro;
    }

    /// <summary>
    /// 设置之前的物品
    /// </summary>
    public void SetBeforeItems(ItemsBean itemsData)
    {
        ui_BeforeItems.SetData(UIViewItemContainer.ContainerType.ItemsTransition, itemsData);
    }

    /// <summary>
    /// 设置之后的物品
    /// </summary>
    public void SetAfterItems(ItemsBean itemsData)
    {
        ui_AfterItems.SetData(UIViewItemContainer.ContainerType.ItemsTransition, itemsData);
    }

    /// <summary>
    /// 道具修改回调
    /// </summary>
    public void CallBackForItemsChange(UIViewItemContainer changeView, ItemsBean chagneData)
    {
        if (changeView == ui_BeforeItems)
        {
            if (blockMetaData.itemBeforeId != chagneData.itemId)
            {
                blockMetaData.transitionPro = 0;
            }
            blockMetaData.itemBeforeId = (int)chagneData.itemId;
            blockMetaData.itemBeforeNum = chagneData.number;

        }
        else if (changeView == ui_AfterItems)
        {
            blockMetaData.itemAfterId = (int)chagneData.itemId;
            blockMetaData.itemAfterNum = chagneData.number;
        }

        string metaStr = Block.ToMetaData(blockMetaData);
        blockData.meta = metaStr;
        targetBlockChunk.isSaveData = true;
    }
}