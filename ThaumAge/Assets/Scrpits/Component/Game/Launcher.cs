using UnityEngine;

public class Launcher : BaseMonoBehaviour
{
    void Start()
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        userData.userId = "Test";
        //…Ë÷√÷÷◊”
        WorldCreateHandler.Instance.manager.SetWorldSeed(132349);
        GameHandler.Instance.manager.ChangeGameState(GameStateEnum.Gaming);
        WorldCreateHandler.Instance.CreateChunkForRange(Vector3Int.zero, 3);
        //UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameMain>(UIEnum.GameMain);
        FPSHandler.Instance.SetData(true, 120);
    }

}
