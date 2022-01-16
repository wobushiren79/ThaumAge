
using System;
using System.Collections;
using UnityEngine;

public class GameLauncher : BaseLauncher
{
    public WorldTypeEnum worldType = WorldTypeEnum.Test;

    public override void Launch()
    {
        base.Launch();
        //����UI
        UIHandler.Instance.OpenUIAndCloseOther<UILoading>(UIEnum.Loading);
        //������Դ
        GameHandler.Instance.LoadGameResources(()=> 
        {
            UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
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
            WorldCreateHandler.Instance.CreateChunkRangeForCenterPosition(Vector3Int.zero, gameConfig.worldRefreshRange, CompleteForUpdateChunk);
            //�޸���Ϸ״̬
            GameHandler.Instance.manager.SetGameState(GameStateEnum.Gaming);
        });

    }

    /// <summary>
    /// ˢ����ɺ�
    /// </summary>
    public void CompleteForUpdateChunk()
    {
        //����UI
        UIHandler.Instance.OpenUIAndCloseOther<UIGameMain>(UIEnum.GameMain);
        //�޸�����
        WeatherHandler.Instance.ChangeWeather(WeatherTypeEnum.Cloudy, 2000);
        //���ؽ�ɫ����
        GameControlHandler.Instance.SetPlayerControlEnabled(true);
        //��ʼ��λ��
        GameHandler.Instance.manager.player.InitPosition();
    }

}
