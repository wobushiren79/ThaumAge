using UnityEngine;

public class Launcher : BaseMonoBehaviour
{
    void Start()
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        userData.userId = "Test";
        //ÉèÖÃÖÖ×Ó
        WorldCreateHandler.Instance.manager.SetWorldSeed(132349);
        GameHandler.Instance.manager.ChangeGameState(GameStateEnum.Init);
        WorldCreateHandler.Instance.CreateChunkForRangeForCenterPosition(Vector3Int.zero, 3, () =>
         {
             GameHandler.Instance.manager.ChangeGameState(GameStateEnum.Gaming);
         });
        //UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameMain>(UIEnum.GameMain);
        FPSHandler.Instance.SetData(true, 120);
    }

}
