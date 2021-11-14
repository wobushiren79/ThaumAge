﻿using UnityEditor;
using UnityEngine;

public enum BlockShapeEnum
{
    None = 0,//空
    Cube = 1, //方块
    CubeTransparent = 2,//透明方块
    CubeCuboid = 3,//长方体方块
    Cross = 101,// 正交叉
    CrossOblique = 102,//斜交叉

    Liquid = 201,//液体

    PlantCrossOblique = 302,//种植，斜交叉

    Custom = 99999,//自定义
}
