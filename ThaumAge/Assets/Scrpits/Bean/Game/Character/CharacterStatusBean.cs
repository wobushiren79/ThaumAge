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
    public float stamina = 10f;
    public int maxStamina = 10;

    //魔力值
    public int magic = 0;
    public int maxMagic = 0;

    //饥饿值
    public int saturation = 10;
    public int maxSaturation = 10;

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
        EventHandler.Instance.TriggerEvent(EventsInfo.CharacterStatus_StatusChange);
        return true;
    }
}
