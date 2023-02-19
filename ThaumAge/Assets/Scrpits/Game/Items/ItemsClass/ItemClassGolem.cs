using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ItemClassGolem : ItemTypeCreature
{

    public override void CreateCreature(ItemsBean itemData, Vector3Int targetPosition)
    {
        ItemsInfoBean itemsInfo = GetItemsInfo(itemData.itemId);
        CreatureHandler.Instance.CreateCreature(itemsInfo.type_id, targetPosition + Vector3Int.up);
    }

    /// <summary>
    /// 设置图标
    /// </summary>
    public override void SetItemIcon(ItemsBean itemData, ItemsInfoBean itemsInfo, Image ivTarget = null, SpriteRenderer srTarget = null)
    {
        ItemMetaGolem itemMetaGolem = itemData.GetMetaData<ItemMetaGolem>();
        if (itemMetaGolem.material == 0)
        {
            itemMetaGolem.material = 10001;
        }
        var materialData = GolemPressInfoCfg.GetItemData(itemMetaGolem.material);
        IconHandler.Instance.manager.GetItemsSpriteByName($"icon_item_golem_{materialData.id}", (sprite) =>
        {
            Color colorIcon = GetItemIconColor(itemData, itemsInfo);
            if (ivTarget != null)
            {
                ivTarget.color = colorIcon;
                ivTarget.sprite = sprite;
            }
            if (srTarget != null)
            {
                srTarget.color = colorIcon;
                srTarget.sprite = sprite;
            }
        });
    }
}