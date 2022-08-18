using UnityEditor;
using UnityEngine;

public class GameTimeHandler : BaseHandler<GameTimeHandler, GameTimeManager>
{
    public float timeScale = 1;

    protected float timeOffset = 0;
    protected float timeOffsetForUnscaled = 0;

    public void Update()
    {
        GameStateEnum gameState = GameHandler.Instance.manager.GetGameState();          
        Time.timeScale = timeScale;
        //受缩放时间影响
        if (timeOffset >= 2)
        {
            switch (gameState) 
            {
                case GameStateEnum.Gaming:
                    //游戏中游戏时间处理
                    HandleForGameTime();
                    break;
                case GameStateEnum.Main:
                    //主界面游戏时间处理
                    HandleForMainTime();
                    break;
            }
            timeOffset = 0;
        }
        timeOffset += Time.deltaTime;

        //不受缩放时间影响
        if (timeOffsetForUnscaled >= 1)
        {
            switch (gameState)
            {
                case GameStateEnum.Gaming:
                    //游玩时间处理
                    HandleForPlayTime();
                    break;
            }
            timeOffsetForUnscaled = 0;
        }
        timeOffsetForUnscaled += Time.unscaledDeltaTime;
    }

    /// <summary>
    /// 设置时间
    /// </summary>
    /// <param name="hour"></param>
    /// <param name="minute"></param>
    public void SetGameTime(int hour,int minute)
    {
        TimeBean timeData = manager.GetGameTime();
        timeData.hour = hour;
        timeData.minute = minute;
    }

    /// <summary>
    /// 处理-主界面游戏时间
    /// </summary>
    public void HandleForMainTime()
    {
        TimeBean timeData = manager.GetMainTime();
        timeData.AddTimeForYMHMS(0, 0, 0, 0, 1, 0);
    }

    /// <summary>
    /// 处理-游戏时间
    /// </summary>
    public void HandleForGameTime()
    {
        GameStateEnum gameState = GameHandler.Instance.manager.GetGameState();
        if (gameState != GameStateEnum.Gaming)
            return;
        TimeBean timeData = manager.GetGameTime();
        timeData.AddTimeForYMHMS(0, 0, 0, 0, 1, 0);
    }

    /// <summary>
    /// 处理-游玩时间
    /// </summary>
    public void HandleForPlayTime()
    {
        GameStateEnum gameState = GameHandler.Instance.manager.GetGameState();
        if (gameState != GameStateEnum.Gaming)
            return;
        TimeBean timeData = manager.GetPlayTime();
        timeData.AddTimeForHMS(0, 0, 1);
    }

}