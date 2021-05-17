using System;
using System.Collections;
using UnityEngine;

public class Launcher : BaseMonoBehaviour
{
    void Start()
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        userData.userId = "Test";
        //��������
        WorldCreateHandler.Instance.manager.SetWorldSeed(132349);
        GameHandler.Instance.manager.ChangeGameState(GameStateEnum.Init);
        LightHandler.Instance.manager.InitData();

        //���ؽ�ɫ����
        GameControlHandler.Instance.manager.controlForPlayer.EnabledControl(false);
        //���������ݺ�
        Action callBackForLoadData = () =>
        {
            StartCoroutine(CoroutineForUpdateChunk());
        };
        WorldCreateHandler.Instance.CreateChunkForRangeForCenterPosition(Vector3Int.zero, 5, callBackForLoadData);
    }

    /// <summary>
    /// ˢ����ɺ�
    /// </summary>
    public void CompleteForUpdateChunk()
    {
        //����UI
        UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameMain>(UIEnum.GameMain);
        //����FPS
        FPSHandler.Instance.SetData(true, 120);
        //�޸�����
        WeatherHandler.Instance.ChangeWeather(WeatherTypeEnum.Cloudy, 2000);
        //��ʼ��λ��
        GameHandler.Instance.manager.player.InitPosition();
        //���ؽ�ɫ����
        GameControlHandler.Instance.manager.controlForPlayer.EnabledControl(true);
        //�޸���Ϸ״̬
        GameHandler.Instance.manager.ChangeGameState(GameStateEnum.Gaming);
    }

    public IEnumerator CoroutineForUpdateChunk()
    {
        while (GameHandler.Instance.manager.GetGameState() == GameStateEnum.Init)
        {
            yield return new WaitForFixedUpdate();
            WorldCreateHandler.Instance.manager.HandleForUpdateChunk(CompleteForUpdateChunk);
        }
    }

}
