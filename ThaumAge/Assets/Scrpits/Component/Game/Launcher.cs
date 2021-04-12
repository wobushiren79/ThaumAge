using UnityEngine;

public class Launcher : BaseMonoBehaviour
{
    void Start()
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        userData.userId = "Test";
        //��������
        WorldCreateHandler.Instance.manager.SetWorldSeed(132349);
        WorldCreateHandler.Instance.CreateChunkForRange(Vector3Int.zero, 5);
        UIHandler.Instance.manager.OpenUIAndCloseOther<UIGameMain>(UIEnum.GameMain);
    }

}
