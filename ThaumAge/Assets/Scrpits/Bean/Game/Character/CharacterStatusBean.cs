using UnityEditor;
using UnityEngine;
using System;

[Serializable]
public class CharacterStatusBean
{
    //生命值
    public int curHealth = 100;
    public int health = 100;

    //耐力值
    public float curStamina = 10f;
    public int stamina = 10;

    //魔力值
    public int curMagic = 0;
    public int magic = 0;

    //饥饿值
    public float curSaturation = 10;
    public int saturation = 10;

    //空气
    public float curAir = 10;
    public int air = 10;

    //物防
    public int def = 0;
    //魔防
    public int defMagic = 0;

    public int defMetal = 0;
    public int defWooden = 0;
    public int defWater = 0;
    public int defFire = 0;
    public int defEarth = 0;

    //攻击
    public int damage;
    public int damageMagic;

    /// <summary>
    /// 改变耐力
    /// </summary>
    /// <param name="changeData">修改的值</param>
    public bool StaminaChange(float changeData)
    {
        if (changeData > 0 && curStamina >= stamina)
        {
            return false;
        }
        if (changeData < 0 && curStamina <= 0)
        {
            return false;
        }
        curStamina += changeData;
        if (curStamina > stamina)
            curStamina = stamina;
        if (curStamina < 0)
            curStamina = 0;
        return true;
    }

    /// <summary>
    /// 改变饥饿值
    /// </summary>
    /// <returns></returns>
    public float SaturationChange(float changeData)
    {
        this.curSaturation += changeData;
        if (curSaturation > saturation)
        {
            curSaturation = saturation;
        }
        if (curSaturation < 0)
        {
            curSaturation = 0;
        }
        return curSaturation;
    }

    /// <summary>
    /// 改变生命值
    /// </summary>
    /// <param name="changeData"></param>
    /// <returns></returns>
    public int HealthChange(int changeData)
    {
        this.curHealth += changeData;
        if (curHealth > health)
        {
            curHealth = health;
        }
        if (curHealth < 0)
        {
            curHealth = 0;
        }
        return curHealth;
    }
}
