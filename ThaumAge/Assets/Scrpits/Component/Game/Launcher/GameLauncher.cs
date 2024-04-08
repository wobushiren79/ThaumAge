
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class GameLauncher : BaseLauncher
{
   
    public WorldTypeEnum testWorldType = WorldTypeEnum.Test;
    public int seed = 0;
    public Terrain3DCShaderNoiseLayer testTerrain3DCShaderNoise;
    
    public override void Launch()
    {
        base.Launch();
        IconHandler.Instance.InitData();
        //打开主UI
        UIHandler.Instance.OpenUIAndCloseOther<UILoading>();
        //初始化摄像头数据
        CameraHandler.Instance.InitGameCameraData();
        //加载资源
        GameHandler.Instance.LoadGameResources(() =>
        {
            UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
            userData.userExitPosition.GetWorldPosition(out WorldTypeEnum worldType, out Vector3 worldPositionUser);
            //如果是测试数据
            if (userData.userId.Equals("Test"))
            {
                worldType = testWorldType;
            }
            testWorldType = worldType;
            //设置游戏状态
            GameHandler.Instance.manager.SetGameState(GameStateEnum.Init);
            //设置种子
            if(Application.isEditor && seed != 0)
            {
                userData.seed = seed;
                LogUtil.Log($"测试世界生成 种子{userData.seed}");
            }
            WorldCreateHandler.Instance.manager.SetWorldSeed(userData.seed);
            //开关角色控制
            GameControlHandler.Instance.SetPlayerControlEnabled(false);
            //渲染设置
            VolumeHandler.Instance.SetFog(GameStateEnum.Gaming);

            GameConfigBean gameConfig = GameDataHandler.Instance.manager.GetGameConfig();
            WorldCreateHandler.Instance.ChangeWorld(worldType, CompleteForUpdateChunk, Vector3Int.CeilToInt(worldPositionUser), gameConfig.worldRefreshRange);
        });
    }

    /// <summary>
    /// 刷新完成后
    /// </summary>
    public void CompleteForUpdateChunk()
    {
        StartCoroutine(CoroutineForCompleteForUpdateChunk());
    }

    public IEnumerator CoroutineForCompleteForUpdateChunk()
    {
        while (!WorldCreateHandler.Instance.CheckAllInitChunkLoadComplete())
        {
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(0.1f);
        //修改游戏状态
        GameHandler.Instance.manager.SetGameState(GameStateEnum.Gaming);
        //修改天气
        WeatherHandler.Instance.ChangeWeather(WeatherTypeEnum.Cloudy, 2000);
        //开关角色控制
        GameControlHandler.Instance.SetPlayerControlEnabled(true);
        //初始化游戏角色
        GameHandler.Instance.InitCharacter();
        //修改光照
        LightHandler.Instance.InitLight();
        //打开主UI
        UIHandler.Instance.OpenUIAndCloseOther<UIGameMain>();
    }

}
