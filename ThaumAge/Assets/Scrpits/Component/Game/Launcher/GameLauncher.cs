
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
        //����UI
        UIHandler.Instance.OpenUIAndCloseOther<UILoading>(UIEnum.Loading);
        //������Դ
        GameHandler.Instance.LoadGameResources(() =>
        {
            UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
            userData.userPosition.GetWorldPosition(out WorldTypeEnum worldType, out Vector3 worldPosition);
            //����ǲ�������
            if (userData.userId.Equals("Test"))
            {
                worldType = testWorldType;
            }
            testWorldType = worldType;
            //������Ϸ״̬
            GameHandler.Instance.manager.SetGameState(GameStateEnum.Init);
            //��������
            WorldCreateHandler.Instance.manager.SetWorldSeed(userData.seed);
            //���ؽ�ɫ����
            GameControlHandler.Instance.SetPlayerControlEnabled(false);
            //������������
            WorldCreateHandler.Instance.SetWorldType(worldType);
            //����Զ��ģ��
            VolumeHandler.Instance.SetDepthOfField(worldType);
            //ˢ����Χ����
            GameConfigBean gameConfig = GameDataHandler.Instance.manager.GetGameConfig();
            WorldCreateHandler.Instance.CreateChunkRangeForCenterPosition(Vector3Int.zero, gameConfig.worldRefreshRange, true, CompleteForUpdateChunk);
        });
    }

    /// <summary>
    /// ˢ����ɺ�
    /// </summary>
    public void CompleteForUpdateChunk()
    {
        //�޸���Ϸ״̬
        GameHandler.Instance.manager.SetGameState(GameStateEnum.Gaming);
        //����UI
        UIHandler.Instance.OpenUIAndCloseOther<UIGameMain>(UIEnum.GameMain);
        //�޸�����
        WeatherHandler.Instance.ChangeWeather(WeatherTypeEnum.Cloudy, 2000);
        //���ؽ�ɫ����
        GameControlHandler.Instance.SetPlayerControlEnabled(true);
        //��ʼ��λ��
        GameHandler.Instance.manager.player.InitPosition();
        //��ʼ����Ϸ��ɫ
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        userData.userPosition.GetWorldPosition(out WorldTypeEnum worldType, out Vector3 worldPosition);
        GameHandler.Instance.InitCharacter(worldPosition);
    }

}
