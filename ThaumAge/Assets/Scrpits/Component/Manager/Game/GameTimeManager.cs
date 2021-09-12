using UnityEditor;
using UnityEngine;

public class GameTimeManager : BaseManager
{
    public TimeBean timeForMain;

    /// <summary>
    /// 获取主界面时间
    /// </summary>
    /// <returns></returns>
    public TimeBean GetMainTime()
    {
        if (timeForMain == null)
        {
            timeForMain = new TimeBean();
            timeForMain.SetTimeForHM(WorldRandTools.Range(0, 24), 0);
        }
        return timeForMain;
    }

    /// <summary>
    /// 获取游戏里的时间
    /// </summary>
    /// <returns></returns>
    public TimeBean GetGameTime()
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        TimeBean timeData = userData.timeForGame;
        return timeData;
    }

    /// <summary>
    /// 获取玩家游玩时间
    /// </summary>
    /// <returns></returns>
    public TimeBean GetPlayTime()
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        TimeBean timeData = userData.timeForPlay;
        return timeData;
    }
}