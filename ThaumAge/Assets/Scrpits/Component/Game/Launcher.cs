using UnityEngine;

public class Launcher : BaseMonoBehaviour
{
    void Start()
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        userData.userId = "1234";

        WorldCreateHandler.Instance.CreateChunkForRange(31, Vector3Int.zero, 2);

        UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameMain>(UIEnum.GameMain);
    }

}
