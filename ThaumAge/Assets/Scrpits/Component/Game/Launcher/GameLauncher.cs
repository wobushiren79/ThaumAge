
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
        //��������
        WorldCreateHandler.Instance.manager.SetWorldSeed(132349);
        GameHandler.Instance.manager.SetGameState(GameStateEnum.Init);
        //���ؽ�ɫ����
        GameControlHandler.Instance.manager.controlForPlayer.EnabledControl(false);
        //������������
        WorldCreateHandler.Instance.SetWorldType(worldType);
        //ˢ����Χ����
        WorldCreateHandler.Instance.CreateChunkRangeForCenterPosition(Vector3Int.zero, refreshRange, CompleteForUpdateChunk);
        //�޸���Ϸ״̬
        GameHandler.Instance.manager.SetGameState(GameStateEnum.Gaming);
    }

    /// <summary>
    /// ˢ����ɺ�
    /// </summary>
    public void CompleteForUpdateChunk()
    {
        //����UI
        UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameMain>(UIEnum.GameMain);
        //�޸�����
        WeatherHandler.Instance.ChangeWeather(WeatherTypeEnum.Cloudy, 2000);
        //�޸ĵƹ�
        LightHandler.Instance.InitData();
        //��ʼ������ͷ����
        CameraHandler.Instance.InitGameData();
        //���ؽ�ɫ����
        GameControlHandler.Instance.manager.controlForPlayer.EnabledControl(true);
        //��ʼ��λ��
        GameHandler.Instance.manager.player.InitPosition();
    }

}
