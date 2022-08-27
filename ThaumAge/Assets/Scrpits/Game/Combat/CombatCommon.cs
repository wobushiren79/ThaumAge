using UnityEditor;
using UnityEngine;

public class CombatCommon
{


    /// <summary>
    /// 目标检测  长(距离目标的距离) 宽（攻击宽度）高（攻击高度）
    /// </summary>
    /// <param name="user"></param>
    /// <param name="lengthRangeDamage"></param>
    /// <param name="widthRangeDamage"></param>
    /// <param name="heightRangeDamage"></param>
    /// <returns></returns>
    public static Collider[] TargetCheck(GameObject user, float lengthRangeDamage, float widthRangeDamage, float heightRangeDamage, int targetLayer)
    {
        //设置检测范围
        Vector3 centerPosition = user.transform.position + user.transform.forward * (lengthRangeDamage / 2f) + new Vector3(0, 1, 0);
        Vector3 halfEx = new Vector3(lengthRangeDamage, widthRangeDamage, heightRangeDamage);
        Collider[] targetArray = RayUtil.RayToBox(centerPosition, halfEx, user.transform.rotation, targetLayer);

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
    /// <param name="user"></param>
    /// <param name="damage"></param>
    /// <param name="targetArray"></param>
    public static void DamageTarget(GameObject user, DamageBean damageData, Collider[] targetArray)
    {
        if (targetArray.IsNull())
            return;

        CreatureCptBase selfCreature = user.GetComponentInChildren<CreatureCptBase>();
        for (int i = 0; i < targetArray.Length; i++)
        {
            Collider itemCollider = targetArray[i];
            //获取目标生物
            CreatureCptBase creatureCpt = itemCollider.GetComponentInChildren<CreatureCptBase>();
            if (creatureCpt == null)
                continue;
            if (creatureCpt == selfCreature)
                continue;
            creatureCpt.UnderAttack(user, damageData);
        }
    }
    public static void DamageTarget(GameObject user, DamageBean damageData, Collider target)
    {
        DamageTarget(user, damageData, new Collider[] { target });
    }

    /// <summary>
    /// 获取近战伤害范围
    /// </summary>
    /// <param name="length"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public static void GetRangeDamage(string rangeDamage, out float length, out float width, out float height)
    {
        length = 0;
        width = 0;
        height = 0;

        float[] data = rangeDamage.SplitForArrayFloat(',');
        if (data.Length >= 1)
        {
            length = data[0];
        }
        if (data.Length >= 2)
        {
            width = data[1];
        }
        if (data.Length >= 3)
        {
            height = data[2];
        }
    }

}