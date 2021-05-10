using System.Threading;
using UnityEditor;
using UnityEngine;

public class SceneElementSky : SceneElementBase
{
    public GameObject objSun;
    public GameObject objMoon;

    public void Update()
    {
        if (GameHandler.Instance.manager.GetGameState() == GameStateEnum.Gaming)
        {
            HandleForPosition();
            HandleForLookAt();
        }
    }

    /// <summary>
    /// 太阳和月亮的朝向
    /// </summary>
    public void HandleForLookAt()
    {
        Transform tfPlayer = GameHandler.Instance.manager.player.transform;
        objSun.transform.LookAt(tfPlayer);
        objMoon.transform.LookAt(tfPlayer);
    }
}