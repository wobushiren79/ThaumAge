using System;
using System.Collections;
using UnityEngine;

public class Launcher : BaseMonoBehaviour
{
    void Start()
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        userData.userId = "Test";
        //设置种子
        WorldCreateHandler.Instance.manager.SetWorldSeed(132349);
        GameHandler.Instance.manager.ChangeGameState(GameStateEnum.Init);
        LightHandler.Instance.manager.InitData();

        //开关角色控制
        GameControlHandler.Instance.manager.controlForPlayer.EnabledControl(false);
        //加载玩数据后
        Action callBackForLoadData = () =>
        {
            StartCoroutine(CoroutineForUpdateChunk());
        };

        WorldCreateHandler.Instance.CreateChunkForRangeForCenterPosition(Vector3Int.zero, 2, callBackForLoadData);

    }

    /// <summary>
    /// 刷新完成后
    /// </summary>
    public void CompleteForUpdateChunk()
    {
        //打开主UI
        UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameMain>(UIEnum.GameMain);
        //设置FPS
        FPSHandler.Instance.SetData(true, 120);
        //修改天气
        WeatherHandler.Instance.ChangeWeather(WeatherTypeEnum.Cloudy, 2000);
        //初始化位置
        GameHandler.Instance.manager.player.InitPosition();
        //开关角色控制
        GameControlHandler.Instance.manager.controlForPlayer.EnabledControl(true);
        //修改游戏状态
        GameHandler.Instance.manager.ChangeGameState(GameStateEnum.Gaming);
    }

    public IEnumerator CoroutineForUpdateChunk()
    {
        while (GameHandler.Instance.manager.GetGameState() == GameStateEnum.Init)
        {
            yield return new WaitForFixedUpdate();
            WorldCreateHandler.Instance.manager.HandleForUpdateChunk(CompleteForUpdateChunk);
        }
    }

}
