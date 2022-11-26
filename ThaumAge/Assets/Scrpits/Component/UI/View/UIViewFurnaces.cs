using UnityEditor;
using UnityEngine;

public partial class UIViewFurnaces : BaseUIView
{
    protected ItemsBean itemsFire;
    protected ItemsBean itemsBefore;
    protected ItemsBean itemsAfter;

    protected BlockBean blockData;
    protected BlockMetaFurnaces blockMetaFurnaces;
    protected Vector3Int blockWorldPosition;
    protected Chunk targetBlockChunk;
    protected BlockBaseFurnaces targetBlockFurnaces;

    //线性数据
    protected float lerpFirePowerPro = 0;
    protected float lerpFirePro = 0;
    public override void Awake()
    {
        base.Awake();
        ui_FireItems.SetCallBackForSetViewItem(CallBackForItemsChange);
        ui_BeforeItems.SetCallBackForSetViewItem(CallBackForItemsChange);
        ui_AfterItems.SetCallBackForSetViewItem(CallBackForItemsChange);
    }

    public float timeUpdate = 0;

    public void Update()
    {
        timeUpdate += Time.deltaTime;
        if (timeUpdate >= 0.1f)
        {
            timeUpdate = 0;
            RefreshUI();
        }
        SetFirePower(lerpFirePowerPro);
        SetFirePro(lerpFirePro);
    }

    public override void RefreshUI(bool isOpenInit = false)
    {
        base.RefreshUI(isOpenInit);

        blockMetaFurnaces = Block.FromMetaData<BlockMetaFurnaces>(blockData.meta);

        if (blockMetaFurnaces == null)
            blockMetaFurnaces = new BlockMetaFurnaces();

        itemsFire.itemId = blockMetaFurnaces.itemFireSourceId;
        itemsFire.number = blockMetaFurnaces.itemFireSourceNum;

        itemsBefore.itemId = blockMetaFurnaces.itemBeforeId;
        itemsBefore.number = blockMetaFurnaces.itemBeforeNum;

        itemsAfter.itemId = blockMetaFurnaces.itemAfterId;
        itemsAfter.number = blockMetaFurnaces.itemAfterNum;

        lerpFirePowerPro = blockMetaFurnaces.fireTimeRemain / (float)blockMetaFurnaces.fireTimeMax;
        lerpFirePro = blockMetaFurnaces.transitionPro;

        SetFireItems(itemsFire);
        SetBeforeItems(itemsBefore);
        SetAfterItems(itemsAfter);
    }

    public void SetData(Vector3Int worldPosition)
    {
        //获取相关数据
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition, out Block targetBlock, out targetBlockChunk);
        BlockBean blockData = targetBlockChunk.GetBlockData(worldPosition - targetBlockChunk.chunkData.positionForWorld);
        this.blockData = blockData;
        this.blockWorldPosition = worldPosition;
        targetBlockFurnaces = targetBlock as BlockBaseFurnaces;

        blockMetaFurnaces = Block.FromMetaData<BlockMetaFurnaces>(blockData.meta);

        if (blockMetaFurnaces == null)
            blockMetaFurnaces = new BlockMetaFurnaces();

        itemsFire = new ItemsBean();
        itemsBefore = new ItemsBean();
        itemsAfter = new ItemsBean();

        RefreshUI();
    }


    /// <summary>
    /// 设置烧制能量值
    /// </summary>
    public void SetFirePower(float firePowerPro)
    {
        ui_FirePower.value = Mathf.Lerp(ui_FirePower.value, firePowerPro, Time.deltaTime);
    }

    /// <summary>
    /// 设置烧制进度
    /// </summary>
    public void SetFirePro(float firePro)
    {
        if (ui_FirePro.value > firePro)
        {
            ui_FirePro.value = 0;
        }
        ui_FirePro.value = Mathf.Lerp(ui_FirePro.value, firePro, Time.deltaTime);
    }

    /// <summary>
    /// 设置烧制能量道具
    /// </summary>
    public void SetFireItems(ItemsBean itemsData)
    {
        ui_FireItems.SetViewItemByData(UIViewItemContainer.ContainerType.Furnaces, itemsData);
    }

    /// <summary>
    /// 设置烧制之前的物品
    /// </summary>
    public void SetBeforeItems(ItemsBean itemsData)
    {
        ui_BeforeItems.SetViewItemByData(UIViewItemContainer.ContainerType.Furnaces, itemsData);
    }

    /// <summary>
    /// 设置烧制之后的物品
    /// </summary>
    public void SetAfterItems(ItemsBean itemsData)
    {
        ui_AfterItems.SetViewItemByData(UIViewItemContainer.ContainerType.Furnaces, itemsData);
    }


    /// <summary>
    /// 道具修改回调
    /// </summary>
    public void CallBackForItemsChange(UIViewItemContainer changeView, ItemsBean chagneData)
    {
        if (changeView == ui_FireItems)
        {
            blockMetaFurnaces.itemFireSourceId = (int)chagneData.itemId;
            blockMetaFurnaces.itemFireSourceNum = chagneData.number;
        }
        else if (changeView == ui_BeforeItems)
        {
            blockMetaFurnaces.itemBeforeId = (int)chagneData.itemId;
            blockMetaFurnaces.itemBeforeNum = chagneData.number;
        }
        else if (changeView == ui_AfterItems)
        {
            blockMetaFurnaces.itemAfterId = (int)chagneData.itemId;
            blockMetaFurnaces.itemAfterNum = chagneData.number;
        }

        string metaStr = Block.ToMetaData(blockMetaFurnaces);
        blockData.meta = metaStr;
        Vector3Int localPosition = blockWorldPosition - targetBlockChunk.chunkData.positionForWorld;
        targetBlockFurnaces.StartWork(targetBlockChunk, localPosition);
        targetBlockFurnaces.RefreshObjModel(targetBlockChunk, localPosition);
    }
}