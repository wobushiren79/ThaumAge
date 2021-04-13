using System.Threading;
using UnityEditor;
using UnityEngine;

public class Sky : BaseMonoBehaviour
{
    public GameObject objSun;
    public GameObject objMoon;

    public float timeForAngle = 0;

    private void Update()
    {
        HandleForPosition();
        HandleForTime();
    }

    /// <summary>
    /// 位置处理
    /// </summary>
    public void HandleForPosition()
    {
        Transform tfPlayer = GameHandler.Instance.manager.player.transform;

        objSun.transform.LookAt(tfPlayer);
        objMoon.transform.LookAt(tfPlayer);

        transform.position = tfPlayer.position;
    }

    /// <summary>
    /// 时间处理
    /// </summary>
    public void HandleForTime()
    {
        TimeBean gameTime = GameTimeHandler.Instance.manager.GetGameTime();
        float totalTime = 24f * 60f;
        float currentTime = gameTime.hour * 60 + gameTime.minute;
        timeForAngle = (currentTime / totalTime * 360) + 180;

        Quaternion rotate = Quaternion.AngleAxis(timeForAngle, new Vector3(1, 0, 1));
        transform.rotation = Quaternion.Lerp(rotate, transform.rotation, 0.02f);
    }
}