using UnityEditor;
using UnityEngine;

public class ItemWeapon : Item
{
    public override void Use(GameObject user, ItemsBean itemsData,int type)
    {
        base.Use(user, itemsData, type);
        //获取打中的目标
        Collider[] targetArray = TargetCheck(user, itemsData);
        //伤害打中的目标
        DamageTarget(user, itemsData, targetArray);
    }

    /// <summary>
    /// 目标检测
    /// </summary>
    public virtual Collider[] TargetCheck(GameObject user, ItemsBean itemsData)
    {
        //获取武器伤害
        ItemsInfoBean itemsInfo = GetItemsInfo(itemsData.itemId);
        //获取伤害范围
        itemsInfo.GetRangeDamage(out float lengthRangeDamage, out float widthRangeDamage, out float heightRangeDamage);
        //设置检测范围
        Vector3 centerPosition = user.transform.position + user.transform.forward * (lengthRangeDamage / 2f) + new Vector3(0, 1, 0);
        Vector3 halfEx = new Vector3(lengthRangeDamage, widthRangeDamage, heightRangeDamage);
        Collider[] targetArray = RayUtil.RayToBox(centerPosition, halfEx, user.transform.rotation, 1 << LayerInfo.Creature);

        //GameObject objTest = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //objTest.transform.SetParent(null);
        //objTest.transform.localScale = halfEx;
        //objTest.transform.position = centerPosition;
        //objTest.transform.rotation = user.transform.rotation;
        return targetArray;
    }

    /// <summary>
    /// 伤害目标
    /// </summary>
    public virtual void DamageTarget(GameObject user, ItemsBean itemsData, Collider[] targetArray)
    {
        if (targetArray.IsNull())
            return;
        //获取武器伤害
        ItemsInfoBean itemsInfo = GetItemsInfo(itemsData.itemId);

        CreatureCptBase selfCreature = user.GetComponent<CreatureCptBase>();
        for (int i = 0; i < targetArray.Length; i++)
        {
            Collider itemCollider = targetArray[i];
            //获取目标生物
            CreatureCptBase creatureCpt = itemCollider.GetComponent<CreatureCptBase>();
            if (creatureCpt == null)
                continue;
            if (creatureCpt == selfCreature)
                continue;
            creatureCpt.creatureBattle.UnderAttack(user, itemsInfo.atk);
        }
    }
}