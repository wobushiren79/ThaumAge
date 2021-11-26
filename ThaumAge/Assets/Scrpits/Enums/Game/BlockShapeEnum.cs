using UnityEditor;
using UnityEngine;

public enum BlockShapeEnum
{
    None = 0,//空
    Cube = 1, //方块
    CubeTransparent = 2,//透明方块
    CubeCuboid = 3,//长方体方块
    CubeLeaves = 4,//树叶方块
    Cross = 101,// 正交叉
    CrossOblique = 102,//斜交叉
    Well = 111,//＃字型

    Liquid = 201,//液体

    PlantCross = 301,//种植，正交叉
    PlantCrossOblique = 302,//种植，斜交叉
    PlantWell = 303,//种植，＃字型

    Custom = 99999,//自定义
}
