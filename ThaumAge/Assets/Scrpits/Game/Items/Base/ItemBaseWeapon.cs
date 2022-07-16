using UnityEditor;
using UnityEngine;

public class ItemBaseWeapon : ItemBaseTool
{
    public override void Use(GameObject user, ItemsBean itemsData, ItemUseTypeEnum useType)
    {
        base.Use(user, itemsData, useType);
        //获取武器伤害
        ItemsInfoBean itemsInfo = GetItemsInfo(itemsData.itemId);
        //获取伤害范围
        CombatCommon.GetRangeDamage(itemsInfo.range_damage, out float lengthRangeDamage, out float widthRangeDamage, out float heightRangeDamage);
        //获取打中的目标
        Collider[] targetArray = CombatCommon.TargetCheck(user, lengthRangeDamage, widthRangeDamage, heightRangeDamage, 1 << LayerInfo.Creature);
        //伤害打中的目标
        DamageBean damageData = itemsInfo.GetDamageData();
        CombatCommon.DamageTarget(user, damageData, targetArray);
    }
}