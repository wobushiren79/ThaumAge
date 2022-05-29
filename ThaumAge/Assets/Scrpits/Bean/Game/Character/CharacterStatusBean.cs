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
}
