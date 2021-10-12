using UnityEditor;
using UnityEngine;

public class ItemsHandler : BaseHandler<ItemsHandler, ItemsManager>
{
    /// <summary>
    /// 使用物品
    /// </summary>
    /// <param name="itemsData"></param>
    public void UseItem(ItemsBean itemsData)
    {
        Item item;
        if (itemsData == null || itemsData.itemId == 0)
        {
            //如果手上没有东西
            item = manager.GetRegisterItem(ItemsTypeEnum.Block);
            item.SetItemData(itemsData);
        }
        else
        {
            //如果手上有东西
            ItemsInfoBean itemsInfo = manager.GetItemsInfoById(itemsData.itemId);
            //获取对应得处理类
            item = manager.GetRegisterItem((ItemsTypeEnum)itemsInfo.items_type);
            //设置物品数据
            item.SetItemData(itemsData);
        }
        item.Use();
    }


    /// <summary>
    ///  创建掉落道具实例
    /// </summary>
    public void CreateItemDrop(long itemId, int itemsNumber, Vector3 position)
    {
        CreateItemDrop(new ItemsBean(itemId, itemsNumber), position);
    }
    /// <summary>
    ///  创建掉落道具实例
    /// </summary>
    public void CreateItemDrop(BlockTypeEnum blockType, int itemsNumber, Vector3 position)
    {
        ItemsInfoBean itemsInfo = manager.GetItemsInfoByBlockType(blockType);
        CreateItemDrop(itemsInfo.id, itemsNumber, position);
    }
    /// <summary>
    ///  创建掉落道具实例
    /// </summary>
    public void CreateItemDrop(ItemsBean itemData, Vector3 position)
    {
        manager.GetItemsObjById(-1, (objModel) =>
        {
            GameObject objCommon = Instantiate(gameObject, objModel);
            ItemDrop itemDrop = objCommon.GetComponent<ItemDrop>();
            itemDrop.SetData(itemData, position);
        });
    }
}
