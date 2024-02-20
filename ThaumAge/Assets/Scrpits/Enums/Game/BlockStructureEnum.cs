using UnityEditor;
using UnityEngine;

public enum BlockStructureEnum
{
    Null = 0,
    NormalTree = 1,//普通树木
    NormalTreeSnow = 2,//普通树木 带雪
    ObliqueTree = 11,//斜树 例如椰子树

    DeadWood = 1001,//枯木
    FallDownTree = 1002,//倒下的树
    Cactus = 1003,//仙人掌
    Seaweed = 1004,//水草
}