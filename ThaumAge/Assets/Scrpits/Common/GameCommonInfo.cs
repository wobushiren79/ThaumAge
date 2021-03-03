using UnityEngine;
using UnityEditor;

public class GameCommonInfo
{
    //游戏用户ID
    public static string GameUserId;
    //随机种子
    public static int RandomSeed = 1564;
    // 预加载场景名字
    public static ScenesChangeBean ScenesChangeData = new ScenesChangeBean();

    public static void ClearData()
    {
        GameUserId = null;
        ScenesChangeData = new ScenesChangeBean();
    }

    /// <summary>
    /// 随机化种子
    /// </summary>
    public static void InitRandomSeed()
    {
        Random.InitState(RandomSeed);
    }

}