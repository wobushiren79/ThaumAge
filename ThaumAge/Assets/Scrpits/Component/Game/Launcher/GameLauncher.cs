
using System;
using System.Collections;
using UnityEngine;

public class GameLauncher : BaseLauncher
{
    public int refreshRange = 5;

    public int seed = 132349;

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
            userData.userId = "Test";


            //������Ϸ״̬
            GameHandler.Instance.manager.SetGameState(GameStateEnum.Init);
            //��������
            WorldCreateHandler.Instance.manager.SetWorldSeed(seed);
            //���ؽ�ɫ����
            GameControlHandler.Instance.SetPlayerControlEnabled(false);
            //������������
            WorldCreateHandler.Instance.SetWorldType(worldType);
            //����Զ��ģ��
            VolumeHandler.Instance.SetDepthOfField(worldType);
            //ˢ����Χ����
            WorldCreateHandler.Instance.CreateChunkRangeForCenterPosition(Vector3Int.zero, refreshRange, CompleteForUpdateChunk);
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
