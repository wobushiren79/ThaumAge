using UnityEditor;
using UnityEngine;
using System;

[Serializable]
public class CharacterStatusBean
{
    //生命值
    public int health = 100;
    public int maxHealth = 100;

    //耐力值
    public float stamina = 10f;
    public int maxStamina = 10;

    //魔力值
    public int magic = 0;
    public int maxMagic = 0;

    //饥饿值
    public float saturation = 10;
    public int maxSaturation = 10;

    //空气
    public float air = 10;
    public int maxAir = 10;

    /// <summary>
    /// 耐力修改
    /// </summary>
    /// <param name="changeData">修改的值</param>
    public bool StaminaChange(float changeData)
    {
        if (changeData > 0 && stamina >= maxStamina)
        {
            return false;
        }
        if (changeData < 0 && stamina <= 0)
        {
            return false;
        }
        stamina += changeData;
        if (stamina > maxStamina)
            stamina = maxStamina;
        if (stamina < 0)
            stamina = 0;
        return true;
    }

    /// <summary>
    /// 减少饥饿值
    /// </summary>
    /// <returns></returns>
    public float SaturationChange(float changeData)
    {
        this.saturation += changeData;
        if (saturation > maxSaturation)
        {
            saturation = maxSaturation;
        }
        if (saturation < 0)
        {
            saturation = 0;
        }
        return saturation;
    }

    /// <summary>
    /// 改变生命值
    /// </summary>
    /// <param name="changeData"></param>
    /// <returns></returns>
    public int HealthChange(int changeData)
    {
        this.health += changeData;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        if (health < 0)
        {
            health = 0;
        }
        return health;
    }
}
