using UnityEditor;
using UnityEngine;

public class ItemWeapon : Item
{
    public override void Use(GameObject user, ItemsBean itemsData)
    {
        base.Use(user, itemsData);
        //获取打中的目标
        Collider[] targetArray = TargetCheck();
        //伤害打中的目标
        DamageTarget(itemsData, targetArray);
    }

    /// <summary>
    /// 目标检测
    /// </summary>
    public virtual Collider[] TargetCheck()
    {
        return null;
    }

    /// <summary>
    /// 伤害目标
    /// </summary>
    public virtual void DamageTarget(ItemsBean itemsData, Collider[] targetArray)
    {
        for (int i = 0; i < targetArray.Length; i++)
        {
            Collider itemCollider = targetArray[i];
            //获取目标生物
            CreatureCptBase creatureCpt = itemCollider.GetComponent<CreatureCptBase>();
            //获取武器伤害
            ItemsInfoBean itemsInfo = GetItemsInfo(itemsData.itemId);
            creatureCpt.UnderAttack(itemsInfo.atk);
        }
    }
}