using UnityEditor;
using UnityEngine;

public enum ItemsTypeEnum
{
    None = 0,//null;
    Block = 1,//方块
    Seed = 2,//种子
    Food = 3,//食物

    Material = 9,//素材（合成素材）
    Sundry = 10,//杂项

    //装备类
    Hats = 11,//帽子
    Clothes = 12,//衣服
    Gloves = 13,//手套
    Shoes = 14,//鞋子
    Trousers = 15,//裤子

    Headwear = 16,//头饰
    Ring = 17,//戒指
    Cape = 18,//披风

    Hoe = 21,//锄头
    Pickaxe = 22,//镐子
    Axe = 23,//斧头
    Shovel = 24,//铲子
    WateringCan = 29,//洒水壶
    Sword = 31,//剑
    Knife = 32,//刀
    Bow = 33, //弓

    MagicCore = 40,//法术核心
    Wand = 41, // 法杖
    Cap = 42,//法杖杖端
    Rod = 43,//法杖杖柄

    Gloem = 99,//傀儡
    Creature = 101,//生物
    Empty = 999,//空手
}


public enum ItemDropStateEnum
{
    DropNoPick,//掉落不可捡取
    DropPick,//掉落可捡取
    Picking,//捡起中
}