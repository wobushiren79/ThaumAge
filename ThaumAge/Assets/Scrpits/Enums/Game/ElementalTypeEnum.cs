using UnityEditor;
using UnityEngine;

//元素类型
public enum ElementalTypeEnum
{
    //原始元素
    None = 0,
    Metal = 1,//金
    Wood = 2,//木
    Water = 3,//水
    Fire = 4,//火
    Earth = 5,//土
    Light = 6,//光
    Dark = 7,//暗

    //1级元素
    Life = 1001 ,//生命 = 土 + 水
    Wind = 1002 ,//风 = 水 + 木
    Potentia = 1003 ,//能量 = 火+ 木
    Instrumentum = 1004,//工具= 金 + 木

    Order = 1901,//秩序 = 金 + 木 + 水 + 火 + 土
    Chaos = 1902,//混沌 = 光 + 暗

    //2级元素
    Vacuous = 2001,//虚空 = 风 + 混沌

     //3级元素
    Magic = 3001,//魔力 = 虚空 + 能量
}