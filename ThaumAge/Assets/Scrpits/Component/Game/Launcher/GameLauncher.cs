
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
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        userData.userId = "Test";
        //设置种子
        WorldCreateHandler.Instance.manager.SetWorldSeed(132349);
        GameHandler.Instance.manager.SetGameState(GameStateEnum.Init);
        //开关角色控制
        GameControlHandler.Instance.manager.controlForPlayer.EnabledControl(false);
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
        UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameMain>(UIEnum.GameMain);
        //修改天气
        WeatherHandler.Instance.ChangeWeather(WeatherTypeEnum.Cloudy, 2000);
        //修改灯光
        LightHandler.Instance.InitData();
        //初始化摄像头数据
        CameraHandler.Instance.InitGameData();
        //开关角色控制
        GameControlHandler.Instance.manager.controlForPlayer.EnabledControl(true);
        //初始化位置
        GameHandler.Instance.manager.player.InitPosition();
    }

}
