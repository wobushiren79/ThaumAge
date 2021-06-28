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
        if (itemsData == null)
        {
            //如果手上没有东西
            item = manager.GetRegisterItem(ItemsTypeEnum.Block);
            item.SetItemData(itemsData);
        }
        else
        {
            //如果受伤有东西
            ItemsInfoBean itemsInfo = manager.GetItemsInfoById(itemsData.itemsId);
            //获取对应得处理类
            item = manager.GetRegisterItem((ItemsTypeEnum)itemsInfo.items_type);
            //设置物品数据
            item.SetItemData(itemsData);
        }
        item.Use();
    }
}