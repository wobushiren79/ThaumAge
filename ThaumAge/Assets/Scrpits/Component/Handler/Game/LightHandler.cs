using UnityEditor;
using UnityEngine;

public class LightHandler : BaseHandler<LightHandler, LightManager>
{

    public void InitData()
    {

    }

    private void Update()
    {
        if (GameHandler.Instance.manager.GetGameState() == GameStateEnum.Gaming)
        {
            TimeBean gameTime = GameTimeHandler.Instance.manager.GetGameTime();
            HandleForLightTransform(gameTime);
            HnaldeForDayNightTransition(gameTime);
        }
    }


    /// <summary>
    /// 处理-光照位置旋转
    /// </summary>
    /// <param name="gameTime"></param>
    public void HandleForLightTransform(TimeBean gameTime)
    {
        float lightAlpha = (float)(gameTime.hour * 60f + gameTime.minute) / (24f * 60f);

        float sunRotation = Mathf.Lerp(-90, 270, lightAlpha);
        float moonRotation = sunRotation - 180;
        Quaternion sunQuaternion = Quaternion.Euler(sunRotation, -45, 0);
        Quaternion moonQuaternion = Quaternion.Euler(moonRotation, -45, 0);
        manager.sunLight.transform.rotation = Quaternion.Lerp(manager.sunLight.transform.rotation,sunQuaternion,Time.deltaTime);
        manager.moonLight.transform.rotation = Quaternion.Lerp(manager.moonLight.transform.rotation, moonQuaternion, Time.deltaTime);
    }

    /// <summary>
    /// 处理-白天黑夜转换
    /// </summary>
    /// <param name="gameTime"></param>
    public void HnaldeForDayNightTransition(TimeBean gameTime)
    {
        if (gameTime.hour >= 6 && gameTime.hour <= 18)
        {
            manager.sunLight.shadows = LightShadows.Soft;
            manager.moonLight.shadows = LightShadows.None;
        }
        else
        {
            manager.sunLight.shadows = LightShadows.None;
            manager.moonLight.shadows = LightShadows.Soft;
        }
    }
}