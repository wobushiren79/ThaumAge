using UnityEditor;
using UnityEngine;

public enum BlockShapeEnum
{
    None = 0,//空
    Cube = 1, //方块
    CubeTransparent = 2,//透明方块
    CubeCuboid = 3,//长方体方块
    CubeLeaves = 4,//树叶方块
    CubeHalf = 5,//一半
    CubeAround = 6,//材质连接的方块（例子：玻璃的连接）

    Stairs = 11,//楼梯
    Face = 21,//一个面
    //FaceBoth = 22,//一个面两面渲染

    Cross = 101,// 正交叉
    CrossOblique = 102,//斜交叉
    Well = 111,//＃字型

    Liquid = 201,//液体
    LiquidCross = 202,//液体正交叉
    LiquidCrossOblique = 203,//液体斜交叉

    CropCross = 301,//种植，正交叉
    CropCrossOblique = 302,//种植，斜交叉
    CropWell = 311,//种植，＃字型

    Plough = 401,//耕地

    LinkChild = 80001,//链接子方块（用于填充）

    CustomAroundLRFB = 90001,//四周链接(例子：栅栏)
    CustomDirectionUpDown = 99996,//自定义 上下（例子：灯笼）
    CustomDirection = 99997,//自定义 5面方向放置 下左右前后(例子：火把)
    Custom = 99999,//自定义

}
