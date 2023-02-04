﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class UIViewChestList : BaseUIView
{
    protected BlockBean blockData;
    protected BlockMetaChest blockChestData;
    protected Vector3Int blockWorldPosition;
    public override void Awake()
    {
        base.Awake();
        ui_ItemList.AddCellListener(OnCellForItem);
        this.RegisterEvent<UIViewItemContainer, long>(EventsInfo.UIViewItemContainer_ItemChange, CallBackForItemChange);
    }

    public override void CloseUI()
    {
        ui_ItemList.ClearAllCell();
        base.CloseUI();
    }

    /// <summary>
    /// 增加道具
    /// </summary>
    /// <param name="uiViewItem"></param>
    public bool AddItems(UIViewItem uiViewItem)
    {
        //首先直接在显示的list中搜索空位
        List<GameObject> listCellObj = ui_ItemList.GetAllCellObj();
        for (int i = 0; i < listCellObj.Count; i++)
        {
            GameObject itemObj = listCellObj[i];
            UIViewItemContainer itemContainer = itemObj.GetComponent<UIViewItemContainer>();
            //如果有容器VIEW 并且里面没有东西
            if (itemContainer != null && itemContainer.GetViewItem() == null)
            {
                uiViewItem.ExchangeItemForContainer(itemContainer);
                return true;
            }
        }
        //如果不成功则直接查询整个箱子
        for (int i = 0; i < blockChestData.items.Length; i++)
        {
            ItemsBean itemData = blockChestData.items[i];
            if (itemData == null || itemData.itemId == 0)
            {
                itemData.itemId = uiViewItem.itemId;
                itemData.number = uiViewItem.itemNumber;
                itemData.meta = uiViewItem.meta;
                Destroy(uiViewItem.gameObject);
                return true;
            }
        }
        return false;
    }


    /// <summary>
    /// 初始化数据
    /// </summary>
    public void SetData(Vector3Int worldPosition, int boxSize)
    {
        this.blockWorldPosition = worldPosition;
        //获取对应方块
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(worldPosition, out Block block, out Chunk chunk);
        BlockBaseChest blockChest = block as BlockBaseChest;
        //获取方块数据
        blockChest.GetBlockMetaData(chunk, worldPosition - chunk.chunkData.positionForWorld,out blockData,out  blockChestData);

        if (blockChestData == null) blockChestData = new BlockMetaChest(boxSize);

        ui_ItemList.SetCellCount(blockChestData.items.Length);
        ui_ItemList.RefreshAllCells();
    }

    /// <summary>
    /// 获取数据
    /// </summary>
    /// <returns></returns>
    public string GetDataJson()
    {
        return Block.ToMetaData(blockChestData);
    }

    /// <summary>
    /// 单个数据回调
    /// </summary>
    /// <param name="itemCell"></param>
    public void OnCellForItem(ScrollGridCell itemCell)
    {
        UIViewItemContainer viewItemContainer = itemCell.GetComponent<UIViewItemContainer>();
        ItemsBean itemsData = blockChestData.items[itemCell.index];
        viewItemContainer.SetViewItemByData(UIViewItemContainer.ContainerType.Box, itemsData, itemCell.index);
    }

    /// <summary>
    /// 道具修改回调
    /// </summary>
    /// <param name="itemContainer"></param>
    /// <param name="itemId"></param>
    public void CallBackForItemChange(UIViewItemContainer itemContainer, long itemId)
    {
        if (itemContainer.containerType != UIViewItemContainer.ContainerType.Box)
            return;
        //保存数据
        SaveBoxBlockData();
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    public void SaveBoxBlockData()
    {
        //获取对应方块
        WorldCreateHandler.Instance.manager.GetBlockForWorldPosition(blockWorldPosition, out Block block, out Chunk chunk);
        string dataJson = Block.ToMetaData(blockChestData);
        blockData.meta = dataJson;
        chunk.SetBlockData(blockData);
    }
}