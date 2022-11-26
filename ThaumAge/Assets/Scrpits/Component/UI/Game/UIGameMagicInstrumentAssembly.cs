using UnityEditor;
using UnityEngine;

public partial class UIGameMagicInstrumentAssembly : UIGameCommonNormal
{
    protected BlockTypeMagicInstrumentAssemblyTable blockTypeMagicInstrumentAssemblyTable;
    protected BlockBean blockData;
    protected Vector3Int blockWorldPosition;

    public override void Awake()
    {
        base.Awake();
        ui_Cap.SetCallBackForSetViewItem(CallBackForItemChange);
        ui_Rod.SetCallBackForSetViewItem(CallBackForItemChange);
        ui_Wand.SetCallBackForSetViewItem(CallBackForItemChange);
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="worldPosition"></param>
    public void SetData(Vector3Int worldPosition)
    {
        this.blockWorldPosition = worldPosition;
        //获取对应方块
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition, out Block block, out Chunk chunk);
        blockTypeMagicInstrumentAssemblyTable = block as BlockTypeMagicInstrumentAssemblyTable;
        //获取方块数据
        blockData = chunk.GetBlockData(worldPosition - chunk.chunkData.positionForWorld);
        if (blockData == null)
            return;
        BlockMetaMagicInstrumentAssembly blockMetaMagicInstrument = blockData.GetBlockMeta<BlockMetaMagicInstrumentAssembly>();
        if (blockMetaMagicInstrument == null)
            return;
        if (blockMetaMagicInstrument.capData != null)
            ui_Cap.SetViewItemByData(blockMetaMagicInstrument.capData);
        if (blockMetaMagicInstrument.rodData != null)
            ui_Rod.SetViewItemByData(blockMetaMagicInstrument.rodData);
        if (blockMetaMagicInstrument.wandData != null)
            ui_Wand.SetViewItemByData(blockMetaMagicInstrument.wandData);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        ui_Shortcuts.OpenUI();
        ui_ViewBackPack.OpenUI();
    }

    public override void CloseUI()
    {
        ui_Shortcuts.CloseUI();
        ui_ViewBackPack.CloseUI();

        ui_Wand.ClearViewItem(true, false);
        ui_Cap.ClearViewItem(true, false);
        ui_Rod.ClearViewItem(true, false);
        base.CloseUI();
    }

    public override void RefreshUI(bool isOpenInit = false)
    {
        base.RefreshUI(isOpenInit);
        ui_CapHint.text = TextHandler.Instance.GetTextById(1042);
        ui_RodHint.text = TextHandler.Instance.GetTextById(1043);
        if (isOpenInit)
            return;
        ui_Shortcuts.RefreshUI();
        ui_ViewBackPack.RefreshUI();
    }

    public void CallBackForItemChange(UIViewItemContainer viewContainer, ItemsBean itemData)
    {
        if (viewContainer == ui_Cap || viewContainer == ui_Rod || viewContainer == ui_Wand)
        {
            HandleAssemblyForWand();
        }
    }

    /// <summary>
    /// 处理-法杖合成
    /// </summary>
    public void HandleAssemblyForWand()
    {
        UIViewItem itemWand = ui_Wand.GetViewItem();
        UIViewItem itemCap = ui_Cap.GetViewItem();
        UIViewItem itemRod = ui_Rod.GetViewItem();
        if (itemWand != null)
        {
            //如果法杖栏已经有法杖了 则不组合
            return;
        }
        if (itemCap == null || itemRod == null)
        {
            SaveBlockData();
            //如果杖端和杖柄其中一个没有 那也不合成
            return;
        }
        ItemsBean itemsWand = new ItemsBean();
        itemsWand.itemId = 4100001;
        itemsWand.number = 1;
        ItemMetaWand itemMetaWand = new ItemMetaWand();
        itemMetaWand.capId = (int)itemCap.itemId;
        itemMetaWand.rodId = (int)itemRod.itemId;
        itemsWand.SetMetaData(itemMetaWand);

        ui_Wand.SetViewItemByData(itemsWand);
        ui_Cap.ClearViewItem(true, false);
        ui_Rod.ClearViewItem(true, false);
        SaveBlockData();

        AudioHandler.Instance.PlaySound(201);
    }

    public void SaveBlockData()
    {
        //获取对应方块
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(blockWorldPosition, out Block block, out BlockDirectionEnum blockDirection, out Chunk chunk);
        //获取方块数据
        Vector3Int blockLocalPosition = blockWorldPosition - chunk.chunkData.positionForWorld;
        blockData = chunk.GetBlockData(blockLocalPosition);
        if (blockData == null)
        {
            blockData = new BlockBean(blockLocalPosition, block.blockType, blockDirection);
        }
        BlockMetaMagicInstrumentAssembly blockMetaMagicInstrument = blockData.GetBlockMeta<BlockMetaMagicInstrumentAssembly>();
        if (blockMetaMagicInstrument == null)
        {
            blockMetaMagicInstrument = new BlockMetaMagicInstrumentAssembly();
        }
        //先清除所有数据
        blockMetaMagicInstrument.ClearData();

        UIViewItem itemWand = ui_Wand.GetViewItem();
        UIViewItem itemCap = ui_Cap.GetViewItem();
        UIViewItem itemRod = ui_Rod.GetViewItem();
        if (itemWand != null)
        {
            blockMetaMagicInstrument.wandData = ui_Wand.itemsData;
        }
        if (itemCap != null)
        {
            blockMetaMagicInstrument.capData = ui_Cap.itemsData;
        }
        if (itemRod != null)
        {
            blockMetaMagicInstrument.rodData = ui_Rod.itemsData;
        }
        blockData.SetBlockMeta(blockMetaMagicInstrument);
        chunk.SetBlockData(blockData, true);
    }
}