using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class LightHandler : BaseHandler<LightHandler, LightManager>
{

    private void Update()
    {
        GameStateEnum gameState = GameHandler.Instance.manager.GetGameState();
        switch (gameState)
        {
            //菜单界面时
            case GameStateEnum.Main:
                TimeBean mainTime = GameTimeHandler.Instance.manager.GetMainTime();
                HandleForLightTransform(mainTime);
                HnaldeForDayNightTransition(mainTime);
                break;
            //游戏进行中
            case GameStateEnum.Gaming:
                TimeBean gameTime = GameTimeHandler.Instance.manager.GetGameTime();
                HandleForLightTransform(gameTime);
                HnaldeForDayNightTransition(gameTime);
                break;
        }
    }

    public void InitLight()
    {
        GameStateEnum gameState = GameHandler.Instance.manager.GetGameState();
        switch (gameState)
        {
            //菜单界面时
            case GameStateEnum.Main:
                TimeBean mainTime = GameTimeHandler.Instance.manager.GetMainTime();
                HandleForLightTransform(mainTime,false);
                HnaldeForDayNightTransition(mainTime);
                break;
            //游戏进行中
            case GameStateEnum.Gaming:
                TimeBean gameTime = GameTimeHandler.Instance.manager.GetGameTime();
                HandleForLightTransform(gameTime, false);
                HnaldeForDayNightTransition(gameTime);
                break;
        }
    }

    /// <summary>
    /// 修改光照阴影质量
    /// </summary>
    /// <param name="level"></param>
    public void ChangeShadowResolutionLevel(int level)
    {
        List<HDAdditionalLightData> listHDLightData = manager.GetAllHDLightData();
        for (int i = 0; i < listHDLightData.Count; i++)
        {
            HDAdditionalLightData lightData = listHDLightData[i];
            lightData.SetShadowResolutionLevel(level);
        }
    }

    /// <summary>
    /// 处理-光照位置旋转
    /// </summary>
    /// <param name="gameTime"></param>
    public void HandleForLightTransform(TimeBean gameTime,bool isLerp = true)
    {
        float lightAlpha = (float)(gameTime.hour * 60f + gameTime.minute) / (24f * 60f);

        float sunRotation = Mathf.Lerp(-90, 270, lightAlpha);
        float moonRotation = sunRotation - 180;
        Quaternion sunQuaternion = Quaternion.Euler(sunRotation, -45, 0);
        Quaternion moonQuaternion = Quaternion.Euler(moonRotation, -45, 0);
        if (isLerp)
        {
            manager.sunLight.transform.localRotation = Quaternion.Lerp(manager.sunLight.transform.localRotation, sunQuaternion, Time.deltaTime);
            manager.moonLight.transform.localRotation = Quaternion.Lerp(manager.moonLight.transform.localRotation, moonQuaternion, Time.deltaTime);
        }
        else
        {
            manager.sunLight.transform.localRotation = sunQuaternion;
            manager.moonLight.transform.localRotation = moonQuaternion;
        }
    }

    protected int lastLightType = 1;
    protected float lerpShadowDimmer = 0;

    /// <summary>
    /// 处理-白天黑夜转换
    /// </summary>
    /// <param name="gameTime"></param>
    public void HnaldeForDayNightTransition(TimeBean gameTime)
    {
        if (gameTime.hour * 60 >= 6 * 60 && gameTime.hour * 60 + gameTime.minute <= 18.25 * 60)
        {
            if (lastLightType == 2)
            {
                lerpShadowDimmer = 0;
            }
            HandleForShadowDimmer();
            manager.sunLight.shadows = LightShadows.Soft;
            manager.moonLight.shadows = LightShadows.None;
            lastLightType = 1;
        }
        else
        {
            if (lastLightType == 1)
            {
                lerpShadowDimmer = 0;
            }
            HandleForShadowDimmer();
            manager.sunLight.shadows = LightShadows.None;
            manager.moonLight.shadows = LightShadows.Soft;
            lastLightType = 2;
        }
    }

    /// <summary>
    /// 处理阴影强度
    /// </summary>
    public void HandleForShadowDimmer()
    {
        if (lerpShadowDimmer > 0.95f)
        {
            lerpShadowDimmer = 0.95f;
        }
        manager.sunLightHD.shadowDimmer = lerpShadowDimmer;
        manager.moonLightHD.shadowDimmer = lerpShadowDimmer;
        lerpShadowDimmer += Time.deltaTime;
    }

}