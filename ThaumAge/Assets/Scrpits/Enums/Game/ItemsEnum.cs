using UnityEditor;
using UnityEngine;

public enum ItemsTypeEnum
{
    Block = 1,

    //装备类
    Hats = 11,//帽子
    Clothes = 12,//衣服
    Gloves = 13,//手套
    Shoes = 14,//鞋子

}


public enum ItemDropStateEnum
{
    DropNoPick,//掉落不可捡取
    DropPick,//掉落可捡取
    Picking,//捡起中
}