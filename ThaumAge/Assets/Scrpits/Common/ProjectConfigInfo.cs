using UnityEngine;
using UnityEditor;

public class ProjectConfigInfo
{
    static ProjectConfigInfo()
    {
        //日志显示
#if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true;
#else
        if (Debug.isDebugBuild)
        {
            Debug.unityLogger.logEnabled = true;
        }
        else
        {
            Debug.unityLogger.logEnabled = false;
        }
#endif
    }

    /// <summary>
    /// 游戏版本
    /// </summary>
    public readonly static string GAME_VERSION = "0.0.1";

    /// <summary>
    /// 是否打开日志输出
    /// </summary>
    public static readonly bool IS_OPEN_LOG_MSG = true;

    /// <summary>
    /// 游戏生成版本
    /// </summary>
    public readonly static ProjectBuildTypeEnum BUILD_TYPE = ProjectBuildTypeEnum.Debug;

    /// <summary>
    /// 寻路方式
    /// </summary>
    public readonly static PathFindingEnum AI_PATHFINDING = PathFindingEnum.Navigation;

    /// <summary>
    /// steamAppId
    /// </summary>
    public readonly static string STEAM_APP_ID = "983170";

    /// <summary>
    /// steam所有用户群组key
    /// </summary>
    public readonly static string STEAM_KEY_ALL = "B0147AEB59B2D274DBF8BF54AAA7C0AB";

    /// <summary>
    /// 数据库名称
    /// </summary>
    public readonly static string DATA_BASE_INFO_NAME = "ThaumAge.db";

}