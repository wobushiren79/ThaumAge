﻿using UnityEditor;
using UnityEngine;

public enum AttributeTypeEnum
{
    Health = 1,//体力
    Stamina = 2,//耐力
    Magic = 3,//魔力
    Saturation = 4,//饱食
    Air = 5,//空气

    Def = 101,//物理防御
    DefMagic = 102,//魔法防御
    DefMetal = 111,//元素金防御
    DefWood = 112,//元素木防御
    DefWater = 113,//元素水防御
    DefFire = 114,//元素火防御
    DefEarth = 115,//元素地防御

    Damage = 201,//伤害值
    DamageMagic = 202,//魔法伤害
    KnockbackDis = 211,//击退距离
    KnockbackTime = 212,//击退时间
}