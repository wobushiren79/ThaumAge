using UnityEditor;
using UnityEngine;

public class ItemBaseTool : Item
{
    public override ItemsMetaTool GetInitMetaData(long itemId)
    {
        ItemsInfoBean itemsInfo = GetItemsInfo(itemId);
        ItemsMetaTool itemsDetailsTool = new ItemsMetaTool();
        int maxDurability = itemsInfo.GetAttributeData().GetAttributeValue( AttributeTypeEnum.Durability);
        itemsDetailsTool.curDurability = maxDurability;
        itemsDetailsTool.durability = maxDurability;
        return itemsDetailsTool;
    }
}