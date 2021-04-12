using UnityEditor;
using UnityEngine;

public class GameTimeHandler : BaseHandler<GameTimeHandler, GameTimeManager>
{
    protected float timeOffset = 0;
    protected float timeOffsetForUnscaled = 0;

    public void Update()
    {
        //受缩放时间影响
        if (timeOffset >= 1)
        {
            HandleForGameTime();
            timeOffset = 0;
        }
        timeOffset += Time.deltaTime;

        //不受缩放时间影响
        if (timeOffsetForUnscaled >= 1)
        {
            HandleForPlayTime();
            timeOffsetForUnscaled = 0;
        }
        timeOffsetForUnscaled += Time.unscaledDeltaTime;
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