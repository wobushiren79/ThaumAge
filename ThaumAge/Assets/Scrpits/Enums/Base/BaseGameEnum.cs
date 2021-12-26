﻿public enum UITypeEnum
{
    UIBase = 0,
    Dialog,
    Toast,
    Popup,
    Overlay,
    Model3D,
}

/// <summary>
/// 有效值
/// </summary>
public enum ValidEnum
{
    Disable = 0,
    Enable = 1
}

/// <summary>
/// 抗锯齿模式
/// </summary>
public enum AntialiasingEnum
{
    None = 0,
    FXAA = 1,//Fast Approximate Anti-aliasing
    TAA = 2,//Temporal Anti-aliasing
    SMAA = 3,//Subpixel Morphological Anti-aliasing
}

/// <summary>
/// 2维方向
/// </summary>
public enum Direction2DEnum
{
    None = 0,
    Left = 1,
    Right = 2,
    UP = 3,
    Down = 4,
}

/// <summary>
/// 3维方向
/// </summary>
public enum DirectionEnum
{
    None = 0,
    UP = 1,
    Down = 2,
    Left = 3,
    Right = 4,
    Forward = 5,
    Back = 6,
}

/// <summary>
/// 语言
/// </summary>
public enum LanguageEnum
{
    cn = 0,
    en = 1,
}

/// <summary>
/// 构建类型
/// </summary>
public enum ProjectBuildTypeEnum
{
    Release = 1,
    Debug = 2,
}

/// <summary>
/// 四季
/// </summary>
public enum SeasonsEnum
{
    Other = 0,
    Spring = 1,//春
    Summer = 2,//夏
    Autumn = 3,//秋
    Winter = 4,//冬
}

/// <summary>
/// 输入类型枚举
/// </summary>
public enum InputActionUIEnum
{
    Navigate, Submit, Cancel, Point, Click, ScrollWheel, MiddleClick, RightClick, TrackedDevicePosition, TrackedDeviceOrientation,
    F1, F2, F3, F4, F5, F6, F7, F8, F9, F10, F11, F12,
    N1, N2, N3, N4, N5, N6, N7, N8, N9, N0, NAdd, NSub,//1234567890+-
    ESC,B
}

/// <summary>
/// 粒子类型
/// </summary>
public enum EffectTypeEnum
{
    Normal = 0,//老板粒子系统
    Visual = 1//新版shader粒子系统
}
