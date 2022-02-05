using UnityEditor;
using UnityEngine;

public class ItemBaseTool : Item
{
    public override ItemsDetailsBean GetItemsDetailsBean(long itemId)
    {
        ItemsInfoBean itemsInfo = GetItemsInfo(itemId);
        ItemsDetailsToolBean itemsDetailsTool = new ItemsDetailsToolBean();
        itemsDetailsTool.life = itemsInfo.life;
        itemsDetailsTool.lifeMax = itemsInfo.life;
        return itemsDetailsTool;
    }
}