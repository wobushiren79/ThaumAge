using UnityEditor;
using UnityEngine;

public enum ItemsTypeEnum
{
    None = 0,//nul;
    Block = 1,//方块
    Seed = 2,//种子
    Food = 3,//食物
    //装备类
    Hats = 11,//帽子
    Clothes = 12,//衣服
    Gloves = 13,//手套
    Shoes = 14,//鞋子

    Headwear = 15,//头饰
    Ring = 16,//戒指
    Cape = 17,//披风

    Hoe = 21,//锄头
    Pickaxe = 22,//镐子
    Axe = 23,//斧头
    Shovel = 24,//铲子

    Sword = 31,//剑
    Knife = 32,//刀
    Bow = 33, //弓

    Creature = 101,//生物
}


public enum ItemDropStateEnum
{
    DropNoPick,//掉落不可捡取
    DropPick,//掉落可捡取
    Picking,//捡起中
}