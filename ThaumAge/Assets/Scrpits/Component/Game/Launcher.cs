using UnityEngine;

public class Launcher : BaseMonoBehaviour
{
    void Start()
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        userData.userId = "12";
        //…Ë÷√÷÷◊”
        WorldCreateHandler.Instance.manager.SetWorldSeed(132349);
        WorldCreateHandler.Instance.CreateChunkForRange(Vector3Int.zero, 2);
        UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameMain>(UIEnum.GameMain);
    }

}
