using System.Threading;
using UnityEditor;
using UnityEngine;

public class Sky : BaseMonoBehaviour
{
    public GameObject objSun;
    public GameObject objMoon;

    public Color mainLightStart;
    public Color mainLightEnd;

    public float timeForAngle = 0;

    public void Update()
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
        transform.rotation = Quaternion.Lerp(transform.rotation, rotate, Time.deltaTime);

        Light mainLight = LightHandler.Instance.manager.mainLight;

        Vector3 mainLightPosition;
        Vector3 mainLightAnagles;

        bool isShowLensFlare;
        //光照
        if (gameTime.hour >= 6 && gameTime.hour <= 18)
        {
            mainLightPosition = objSun.transform.position;
            mainLightAnagles = objSun.transform.eulerAngles;
            isShowLensFlare = true;
        }
        else
        {
            mainLightPosition = objMoon.transform.position;
            mainLightAnagles = objMoon.transform.eulerAngles;
            isShowLensFlare = false;
        }

        mainLight.transform.position = Vector3.Lerp(mainLight.transform.position, mainLightPosition, Time.deltaTime);
        mainLight.transform.eulerAngles = Vector3.Lerp(mainLight.transform.eulerAngles, mainLightAnagles, Time.deltaTime);

        float lerpColor;
        Color lightColor;
        if (gameTime.hour >= 0 && gameTime.hour < 12)
        {
            lerpColor = (gameTime.hour * 60 + gameTime.minute) / (float)(12 * 60);
            lightColor = Color.Lerp(mainLightEnd, mainLightStart, lerpColor);
        }
        else
        {
            lerpColor = ((gameTime.hour - 12) * 60 + gameTime.minute) / (float)(12 * 60);
            lightColor = Color.Lerp(mainLightStart, mainLightEnd, lerpColor);
        }
        //天空盒颜色
        LightHandler.Instance.manager.SetSkyBoxColor(lightColor);
        //设置主光照颜色
        LightHandler.Instance.manager.SetMainLightColor(lightColor);
        //设置环境光颜色
        LightHandler.Instance.manager.SetAmbientLight(lightColor * 0.7f);
        //设置光晕
        LightHandler.Instance.manager.SetMainLensFlare(isShowLensFlare);
    }
}