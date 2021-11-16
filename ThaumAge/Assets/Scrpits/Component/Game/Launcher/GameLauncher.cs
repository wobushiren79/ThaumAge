
using System;
using System.Collections;
using UnityEngine;

public class GameLauncher : BaseLauncher
{
    public int refreshRange = 5;

    public WorldTypeEnum worldType = WorldTypeEnum.Test;

    public override void Launch()
    {
        base.Launch();
        //打开主UI
        UIHandler.Instance.OpenUIAndCloseOther<UILoading>(UIEnum.Loading);
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        userData.userId = "Test";
        //设置种子
        WorldCreateHandler.Instance.manager.SetWorldSeed(132349);
        GameHandler.Instance.manager.SetGameState(GameStateEnum.Init);
        //开关角色控制
        GameControlHandler.Instance.SetPlayerControlEnabled(false);
        //设置世界类型
        WorldCreateHandler.Instance.SetWorldType(worldType);
        //刷新周围区块
        WorldCreateHandler.Instance.CreateChunkRangeForCenterPosition(Vector3Int.zero, refreshRange, CompleteForUpdateChunk);
        //修改游戏状态
        GameHandler.Instance.manager.SetGameState(GameStateEnum.Gaming);
    }

    /// <summary>
    /// 刷新完成后
    /// </summary>
    public void CompleteForUpdateChunk()
    {
        //打开主UI
        UIHandler.Instance.OpenUIAndCloseOther<UIGameMain>(UIEnum.GameMain);
        //修改天气
        WeatherHandler.Instance.ChangeWeather(WeatherTypeEnum.Cloudy, 2000);
        //修改灯光
        LightHandler.Instance.InitData();
        //开关角色控制
        GameControlHandler.Instance.SetPlayerControlEnabled(true);
        //初始化位置
        GameHandler.Instance.manager.player.InitPosition();
    }

}
