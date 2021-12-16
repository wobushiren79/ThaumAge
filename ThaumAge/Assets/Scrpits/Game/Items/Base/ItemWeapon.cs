using UnityEditor;
using UnityEngine;

public class ItemWeapon : Item
{
    public override void Use(GameObject user)
    {
        base.Use(user);
    }

    /// <summary>
    /// 目标检测
    /// </summary>
    public virtual Collider[] TargetCheck()
    {
        return null;
    }
}