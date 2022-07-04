using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DamageBean
{
    public Dictionary<DamageAdditionEnum, string> dicDamageData = new Dictionary<DamageAdditionEnum, string>();

    /// <summary>
    /// 获取数据
    /// </summary>
    /// <param name="damageAddition"></param>
    /// <returns></returns>
    public string GetData(DamageAdditionEnum damageAddition)
    {
        if (dicDamageData.TryGetValue(DamageAdditionEnum.Damage, out string value))
        {
            return value;
        }
        return null;
    }

    /// <summary>
    /// 获取伤害值
    /// </summary>
    /// <returns></returns>
    public int GetDamage()
    {
        string data = GetData(DamageAdditionEnum.Damage);
        if (data.IsNull())
        {
            return 0;
        }
        return int.Parse(data);
    }
}
