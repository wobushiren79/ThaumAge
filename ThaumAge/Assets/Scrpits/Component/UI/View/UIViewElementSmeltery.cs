using UnityEditor;
using UnityEngine;

public partial class UIViewElementSmeltery : BaseUIView
{
    protected ItemsBean itemsFire;
    protected ItemsBean itemsBefore;

    protected BlockBean blockData;
    protected BlockMetaElementSmeltery blockMetaElementSmeltery;
    protected Vector3Int blockWorldPosition;
    protected Chunk targetBlockChunk;
    protected BlockTypeElementSmeltery targetBlockElementSmeltery;
    //线性数据
    protected float lerpFirePowerPro = 0;
    protected float lerpFirePro = 0;

    protected float timeForUpdate = 0;
    protected float timeForUpdateMax = 0.5f;
    public override void Awake()
    {
        base.Awake();
        ui_FireItems.SetCallBackForSetViewItem(CallBackForItemsChange);
        ui_BeforeItems.SetCallBackForSetViewItem(CallBackForItemsChange);
    }

    public void Update()
    {
        timeForUpdate += Time.deltaTime;
        if (timeForUpdate > timeForUpdateMax)
        {
            timeForUpdate = 0;
            RefreshUI();
        }
        SetFirePower(lerpFirePowerPro, true);
        SetFirePro(lerpFirePro, true);
    }


    public void SetData(Vector3Int worldPosition)
    {
        //获取相关数据
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition, out Block targetBlock, out targetBlockChunk);
        BlockBean blockData = targetBlockChunk.GetBlockData(worldPosition - targetBlockChunk.chunkData.positionForWorld);
        this.blockData = blockData;
        this.blockWorldPosition = worldPosition;
        targetBlockElementSmeltery = targetBlock as BlockTypeElementSmeltery;

        blockMetaElementSmeltery = Block.FromMetaData<BlockMetaElementSmeltery>(blockData.meta);

        if (blockMetaElementSmeltery == null)
            blockMetaElementSmeltery = new BlockMetaElementSmeltery();

        itemsFire = new ItemsBean();
        itemsBefore = new ItemsBean();

        RefreshUI();

        //初始化的时候设置一次进度
        SetFirePower(lerpFirePowerPro, false);
        SetFirePro(lerpFirePro, false);
    }

    public override void RefreshUI(bool isOpenInit = false)
    {
        base.RefreshUI(isOpenInit);

        blockMetaElementSmeltery = Block.FromMetaData<BlockMetaElementSmeltery>(blockData.meta);

        if (blockMetaElementSmeltery == null)
            blockMetaElementSmeltery = new BlockMetaElementSmeltery();

        itemsFire.itemId = blockMetaElementSmeltery.itemFireSourceId;
        itemsFire.number = blockMetaElementSmeltery.itemFireSourceNum;

        itemsBefore.itemId = blockMetaElementSmeltery.itemBeforeId;
        itemsBefore.number = blockMetaElementSmeltery.itemBeforeNum;

        lerpFirePowerPro = blockMetaElementSmeltery.fireTimeRemain / (float)blockMetaElementSmeltery.fireTimeMax;
        lerpFirePro = blockMetaElementSmeltery.transitionPro;

        SetFireItems(itemsFire);
        SetBeforeItems(itemsBefore);
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
    /// 设置烧制能量值
    /// </summary>
    public void SetFirePower(float firePowerPro, bool isLerp)
    {
        if (isLerp)
        {
            ui_FirePower.value = Mathf.Lerp(ui_FirePower.value, firePowerPro, Time.deltaTime);
        }
        else
        {
            ui_FirePower.value = firePowerPro;
        }
    }

    /// <summary>
    /// 设置烧制进度
    /// </summary>
    public void SetFirePro(float firePro, bool isLerp)
    {
        if (ui_FirePro.value > firePro)
        {
            ui_FirePro.value = 0;
        }

        if (isLerp)
        {
            ui_FirePro.value = Mathf.Lerp(ui_FirePro.value, firePro, Time.deltaTime);
        }
        else
        {
            ui_FirePro.value = firePro;
        }
    }


    /// <summary>
    /// 道具修改回调
    /// </summary>
    public void CallBackForItemsChange(UIViewItemContainer changeView, ItemsBean chagneData)
    {
        if (changeView == ui_FireItems)
        {
            blockMetaElementSmeltery.itemFireSourceId = (int)chagneData.itemId;
            blockMetaElementSmeltery.itemFireSourceNum = chagneData.number;
        }
        else if (changeView == ui_BeforeItems)
        {
            blockMetaElementSmeltery.itemBeforeId = (int)chagneData.itemId;
            blockMetaElementSmeltery.itemBeforeNum = chagneData.number;
        }

        string metaStr = Block.ToMetaData(blockMetaElementSmeltery);
        blockData.meta = metaStr;
        Vector3Int localPosition = blockWorldPosition - targetBlockChunk.chunkData.positionForWorld;
        targetBlockElementSmeltery.StartWork(targetBlockChunk, localPosition);
        targetBlockElementSmeltery.RefreshObjModel(targetBlockChunk, localPosition);
    }
}