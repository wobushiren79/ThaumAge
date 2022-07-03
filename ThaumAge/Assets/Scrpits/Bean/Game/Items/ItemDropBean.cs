using UnityEditor;
using UnityEngine;

public class ItemDropBean
{
    public ItemsBean itemData;//掉落道具数据

    public Vector3 dropPosition;//掉落的地点

    public Vector3 dropDirection = Vector3.zero;//掉落方向

    public ItemDropStateEnum itemDrapState = ItemDropStateEnum.DropNoPick;//掉落状态 0：掉落不可拾取 1：掉落可拾取 2：拾取中

    public void InitData(ItemsBean itemData, ItemDropStateEnum itemDrapState, Vector3 dropPosition, Vector3 dropDirection)
    {
        this.itemData = itemData;
        this.itemDrapState = itemDrapState;
        this.dropPosition = dropPosition;
        this.dropDirection = dropDirection;
    }
    public void InitData(long itemsId, Vector3 dropPosition, Vector3 dropDirection, int itemsNumber = 1, string meta = null, ItemDropStateEnum itemDropState = ItemDropStateEnum.DropPick)
    {
        ItemsBean itemData = new ItemsBean();
        itemData.itemId = itemsId;
        itemData.number = itemsNumber;
        itemData.meta = meta;
        InitData(itemData, itemDropState, dropPosition, dropDirection);
    }

    public void InitData(BlockTypeEnum blockType, Vector3 dropPosition, Vector3 dropDirection, int itemsNumber = 1, string meta = null, ItemDropStateEnum itemDropState = ItemDropStateEnum.DropPick)
    {
        ItemsInfoBean itemsInfo = ItemsHandler.Instance.manager.GetItemsInfoByBlockType(blockType);
        InitData(itemsInfo.id, dropPosition, dropDirection, itemsNumber, meta, itemDropState);
    }


    #region 构造函数
    public ItemDropBean(ItemsBean itemData, ItemDropStateEnum itemDrapState, Vector3 dropPosition, Vector3 dropDirection)
    {
        InitData(itemData, itemDrapState, dropPosition, dropDirection);
    }

    public ItemDropBean(long itemsId, Vector3 dropPosition, Vector3 dropDirection, int itemsNumber = 1, string meta = null, ItemDropStateEnum itemDropState = ItemDropStateEnum.DropPick)
    {
        InitData(itemsId, dropPosition, dropDirection, itemsNumber, meta, itemDropState);
    }
    public ItemDropBean(long itemsId, Vector3 dropPosition, int itemsNumber = 1, string meta = null, ItemDropStateEnum itemDropState = ItemDropStateEnum.DropPick)
    {
        InitData(itemsId, dropPosition, Vector3.zero, itemsNumber, meta, itemDropState);
    }

    public ItemDropBean(BlockTypeEnum blockType, Vector3 dropPosition, Vector3 dropDirection, int itemsNumber = 1, string meta = null, ItemDropStateEnum ItemCptDropState = ItemDropStateEnum.DropPick)
    {
        InitData(blockType, dropPosition, dropDirection, itemsNumber, meta, ItemCptDropState);
    }

    public ItemDropBean(BlockTypeEnum blockType, Vector3 dropPosition,int itemsNumber = 1, string meta = null, ItemDropStateEnum ItemCptDropState = ItemDropStateEnum.DropPick)
    {
        InitData(blockType, dropPosition, Vector3.zero, itemsNumber, meta, ItemCptDropState);
    }
    #endregion
}