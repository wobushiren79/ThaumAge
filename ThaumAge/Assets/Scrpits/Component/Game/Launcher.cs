
using System;
using System.Collections;
using UnityEngine;

public class Launcher : BaseMonoBehaviour
{
    public int refreshRange = 5;

    public WorldTypeEnum worldType = WorldTypeEnum.Test;

    void Start()
    {
        //������һ���ڴ�
        SystemUtil.GCCollect();
        //����FPS
        FPSHandler.Instance.SetData(true, 120);
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        userData.userId = "Test";
        //��������
        WorldCreateHandler.Instance.manager.SetWorldSeed(132349);
        GameHandler.Instance.manager.ChangeGameState(GameStateEnum.Init);
        //���ؽ�ɫ����
        GameControlHandler.Instance.manager.controlForPlayer.EnabledControl(false);
        //������������
        WorldCreateHandler.Instance.SetWorldType(worldType);
        //ˢ����Χ����
        WorldCreateHandler.Instance.CreateChunkRangeForCenterPosition(Vector3Int.zero, refreshRange, CompleteForUpdateChunk);
        //�޸���Ϸ״̬
        GameHandler.Instance.manager.ChangeGameState(GameStateEnum.Gaming);
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
        CameraHandler.Instance.InitData();
        //���ؽ�ɫ����
        GameControlHandler.Instance.manager.controlForPlayer.EnabledControl(true);
        //��ʼ��λ��
        GameHandler.Instance.manager.player.InitPosition();
    }

}
