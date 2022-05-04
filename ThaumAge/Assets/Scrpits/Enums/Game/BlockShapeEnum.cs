﻿using UnityEditor;
using UnityEngine;

public enum BlockShapeEnum
{
    None = 0,//空
    Cube = 1, //方块
    CubeTransparent = 2,//透明方块
    CubeCuboid = 3,//长方体方块
    CubeLeaves = 4,//树叶方块
    CubeHalf = 5,//一半

    Stairs = 11,//楼梯

    Cross = 101,// 正交叉
    CrossOblique = 102,//斜交叉
    Well = 111,//＃字型

    Liquid = 201,//液体

    CropCross = 301,//种植，正交叉
    CropCrossOblique = 302,//种植，斜交叉
    CropWell = 303,//种植，＃字型

    Plough = 401,//耕地

    CustomAroundLRFB = 90001,//四周链接

    CustomDirectionUpDown = 99996,//自定义 上下
    CustomDirection = 99997,//自定义 5面方向放置 下左右前后

    CustomLink = 99998,//自定义 4面环绕链接
    Custom = 99999,//自定义

}
