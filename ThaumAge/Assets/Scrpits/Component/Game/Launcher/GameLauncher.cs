
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class GameLauncher : BaseLauncher
{
    public WorldTypeEnum testWorldType = WorldTypeEnum.Test;

    public override void Launch()
    {
        base.Launch();
        IconHandler.Instance.InitData(null);
        //打开主UI
        UIHandler.Instance.OpenUIAndCloseOther<UILoading>(UIEnum.Loading);
        //加载资源
        GameHandler.Instance.LoadGameResources(() =>
        {
            UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
            userData.userPosition.GetWorldPosition(out WorldTypeEnum worldType, out Vector3 worldPosition);
            //如果是测试数据
            if (userData.userId.Equals("Test"))
            {
                worldType = testWorldType;
            }
            testWorldType = worldType;
            //设置游戏状态
            GameHandler.Instance.manager.SetGameState(GameStateEnum.Init);
            //设置种子
            WorldCreateHandler.Instance.manager.SetWorldSeed(userData.seed);
            //开关角色控制
            GameControlHandler.Instance.SetPlayerControlEnabled(false);
            //设置世界类型
            WorldCreateHandler.Instance.SetWorldType(worldType);
            //设置远景模糊
            VolumeHandler.Instance.SetDepthOfField(worldType);
            //刷新周围区块
            GameConfigBean gameConfig = GameDataHandler.Instance.manager.GetGameConfig();
            WorldCreateHandler.Instance.CreateChunkRangeForCenterPosition(Vector3Int.zero, gameConfig.worldRefreshRange, true, CompleteForUpdateChunk);
        });
    }

    /// <summary>
    /// 刷新完成后
    /// </summary>
    public void CompleteForUpdateChunk()
    {
        //修改游戏状态
        GameHandler.Instance.manager.SetGameState(GameStateEnum.Gaming);
        //打开主UI
        UIHandler.Instance.OpenUIAndCloseOther<UIGameMain>(UIEnum.GameMain);
        //修改天气
        WeatherHandler.Instance.ChangeWeather(WeatherTypeEnum.Cloudy, 2000);
        //开关角色控制
        GameControlHandler.Instance.SetPlayerControlEnabled(true);
        //初始化位置
        GameHandler.Instance.manager.player.InitPosition();
        //初始化游戏角色
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        userData.userPosition.GetWorldPosition(out WorldTypeEnum worldType, out Vector3 worldPosition);
        GameHandler.Instance.InitCharacter(worldPosition);
    }

}
