using UnityEditor;
using UnityEngine;

public class ItemTypeEmpty : Item
{
    public override void Use(GameObject user, ItemsBean itemsData, ItemUseTypeEnum itemUseType)
    {
        base.Use(user, itemsData, itemUseType);

    }
}