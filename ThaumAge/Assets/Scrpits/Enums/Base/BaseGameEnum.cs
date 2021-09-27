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
    cn = 1,
    en = 2,
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