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
            Transform tfPlayer = GameHandler.Instance.manager.player.transform;
            HandleForPosition();

            objSun.transform.localRotation = LightHandler.Instance.manager.sunLight.transform.localRotation;
            objMoon.transform.localRotation = LightHandler.Instance.manager.moonLight.transform.localRotation;

            objSun.transform.position = transform.position;
            objMoon.transform.position = transform.position;

            objSun.transform.Translate(0, 0, -4000);
            objMoon.transform.Translate(0, 0, -4000);

            //HandleForLookAt(tfPlayer.transform.position);
        }
        else if (GameHandler.Instance.manager.GetGameState() == GameStateEnum.Main)
        {
            transform.position = Vector3.zero;
            objSun.transform.localRotation = LightHandler.Instance.manager.sunLight.transform.localRotation;
            objMoon.transform.localRotation = LightHandler.Instance.manager.moonLight.transform.localRotation;

            objSun.transform.position = transform.position;
            objMoon.transform.position = transform.position;

            objSun.transform.Translate(0, 0, -4000);
            objMoon.transform.Translate(0, 0, -4000);

            //HandleForLookAt(Vector3.zero);
        }
    }

    /// <summary>
    /// 太阳和月亮的朝向
    /// </summary>
    public void HandleForLookAt(Vector3 targetPos)
    {
        objSun.transform.LookAt(targetPos);
        objMoon.transform.LookAt(targetPos);
    }
}