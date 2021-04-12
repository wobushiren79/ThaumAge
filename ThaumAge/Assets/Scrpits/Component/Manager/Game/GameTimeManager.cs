using UnityEditor;
using UnityEngine;

public class GameTimeManager : BaseManager
{



    public TimeBean GetGameTime()
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        TimeBean timeData = userData.timeForGame;
        return timeData;
    }

    public TimeBean GetPlayTime()
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        TimeBean timeData = userData.timeForPlay;
        return timeData;
    }
}