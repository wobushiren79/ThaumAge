
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
        //����UI
        UIHandler.Instance.OpenUIAndCloseOther<UILoading>();
        //��ʼ������ͷ����
        CameraHandler.Instance.InitGameCameraData();
        //������Դ
        GameHandler.Instance.LoadGameResources(() =>
        {
            UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
            userData.userExitPosition.GetWorldPosition(out WorldTypeEnum worldType, out Vector3 worldPositionUser);
            //����ǲ�������
            if (userData.userId.Equals("Test"))
            {
                worldType = testWorldType;
                //��������
                if (Application.isEditor && seed != 0)
                {
                    userData.seed = seed;
                    LogUtil.Log($"������������ ����{userData.seed}");
                }
            }
            testWorldType = worldType;
            //������Ϸ״̬
            GameHandler.Instance.manager.SetGameState(GameStateEnum.Init);

            WorldCreateHandler.Instance.manager.SetWorldSeed(userData.seed);
            //���ؽ�ɫ����
            GameControlHandler.Instance.SetPlayerControlEnabled(false);

            GameConfigBean gameConfig = GameDataHandler.Instance.manager.GetGameConfig();

            WorldCreateHandler.Instance.ChangeWorld(worldType, CompleteForUpdateChunk, worldPositionUser, gameConfig.worldRefreshRange);
        });
    }

    /// <summary>
    /// ˢ����ɺ�
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
        //�޸���Ϸ״̬
        GameHandler.Instance.manager.SetGameState(GameStateEnum.Gaming);
        //һ�γ�ʼ������һ����������
        CreatureHandler.Instance.HandleForCreatureCreateLoadData();
        //�޸�����
        WeatherHandler.Instance.ChangeWeather(WeatherTypeEnum.Cloudy, 2000);
        //���ؽ�ɫ����
        GameControlHandler.Instance.SetPlayerControlEnabled(true);
        //��ʼ����Ϸ��ɫ
        GameHandler.Instance.InitCharacter();
        //�޸Ĺ���
        LightHandler.Instance.InitLight();
        //����UI
        UIHandler.Instance.OpenUIAndCloseOther<UIGameMain>();
    }

}
