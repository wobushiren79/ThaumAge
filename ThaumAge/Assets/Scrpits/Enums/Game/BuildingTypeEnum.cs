using UnityEditor;
using UnityEngine;

public enum BuildingTypeEnum
{
    Null = 0,
    NormalTree = 1,//普通树木
    NormalTreeSnow = 2,//普通树木 带雪
    ObliqueTree = 11,//斜树 例如椰子树
    TallTree = 21,//高树

    DeadWood = 1001,//枯木
    FallDownTree = 1002,//倒下的树
    Cactus = 1003,//仙人掌
    Seaweed = 1004,//水草
    Mushroom = 1005, //蘑菇
    MushrooSmall = 1006,//小蘑菇
    MushrooBig = 1007,//大蘑菇
    StoneMoss = 1008, //苔藓石

    InfusionAltar = 10001,//注魔祭坛
    InfernalFurnace = 10002,//炼狱熔炉
    GolemPress = 10003//傀儡工坊
}