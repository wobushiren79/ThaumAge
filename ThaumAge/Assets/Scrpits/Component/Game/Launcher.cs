using UnityEngine;

public class Launcher : BaseMonoBehaviour
{
    void Start()
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        userData.userId = "1234";
        //…Ë÷√÷÷◊”
        WorldCreateHandler.Instance.manager.SetWorldSeed(123);
        WorldCreateHandler.Instance.CreateChunkForRange(Vector3Int.zero, 3);
        UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameMain>(UIEnum.GameMain);
    }

}
