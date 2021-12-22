using UnityEditor;
using UnityEngine;

public class ItemWeapon : Item
{
    public override void Use(GameObject user,ItemsBean itemsData)
    {
        base.Use(user, itemsData);
        //获取打中的目标
        Collider[] targetArray = TargetCheck();
    }

    /// <summary>
    /// 目标检测
    /// </summary>
    public virtual Collider[] TargetCheck()
    {
        return null;
    }
}