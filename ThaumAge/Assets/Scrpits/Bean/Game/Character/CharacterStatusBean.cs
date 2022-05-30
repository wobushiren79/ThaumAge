using UnityEditor;
using UnityEngine;
using System;

[Serializable]
public class CharacterStatusBean
{
    //生命值
    public int health = 10;
    public int maxHealth = 10;

    //耐力值
    public int stamina = 10;
    public int maxStamina = 10;

    //魔力值
    public int magic = 0;
    public int maxMagic = 0;

    //饥饿值
    public int saturation = 10;
    public int maxSaturation = 10;

    //耐力消耗
    protected float staminaExpend = 0;

    /// <summary>
    /// 耐力消耗
    /// </summary>
    /// <param name="subData"></param>
    public bool StaminaExpend(float subData)
    {
        if (stamina <= 0)
            return false;
        staminaExpend += subData;
        if (staminaExpend > 1)
        {
            staminaExpend = 0;
            StaminaChange(-1);
        }
        return true;
    }

    /// <summary>
    /// 耐力修改
    /// </summary>
    /// <param name="changeData">修改的值</param>
    public void StaminaChange(int changeData)
    {
        stamina += changeData;
        if (stamina > maxStamina)
            stamina = maxStamina;
        if (stamina < 0)
            stamina = 0;
    }
}
