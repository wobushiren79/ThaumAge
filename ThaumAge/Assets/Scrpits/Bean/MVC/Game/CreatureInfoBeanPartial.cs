using UnityEditor;
using UnityEngine;

public partial class CreatureInfoBean
{
    protected DamageBean damageData;

    /// <summary>
    /// 获取伤害数据
    /// </summary>
    /// <returns></returns>
    public DamageBean GetDamageData()
    {
        if (damageData == null)
        {
            damageData = CombatCommon.GetDamageData(damage_data);
        }
        return damageData;
    }
}