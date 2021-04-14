﻿using System.Threading;
using UnityEditor;
using UnityEngine;

public class Sky : BaseMonoBehaviour
{
    public GameObject objSun;
    public Light sunLight;

    public Color sunColorStart;
    public Color sunColorEnd;

    public GameObject objMoon;
    public Light moonLight;

    public Color moonColorStart;
    public Color moonColorEnd;

    public float timeForAngle = 0;

    public void Awake()
    {
        sunLight = objSun.GetComponent<Light>();
        moonLight = objMoon.GetComponent<Light>();
    }

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

        //光照
        if (gameTime.hour >= 6 && gameTime.hour <= 18)
        {
            sunLight.gameObject.SetActive(true);
            moonLight.gameObject.SetActive(false);
        }
        else
        {
            moonLight.gameObject.SetActive(true);
            sunLight.gameObject.SetActive(false);
        }
    }
}