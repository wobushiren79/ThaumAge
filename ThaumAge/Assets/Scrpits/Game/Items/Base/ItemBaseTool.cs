using UnityEditor;
using UnityEngine;

public class ItemBaseTool : Item
{
    public override ItemBaseMeta GetInitMetaData<T>(long itemId)
    {
        ItemsInfoBean itemsInfo = GetItemsInfo(itemId);
        ItemMetaTool itemsDetailsTool = new ItemMetaTool();
        int maxDurability = itemsInfo.GetAttributeData().GetAttributeValue( AttributeTypeEnum.Durability);
        itemsDetailsTool.curDurability = maxDurability;
        itemsDetailsTool.durability = maxDurability;
        return itemsDetailsTool;
    }
}