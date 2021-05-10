using UnityEditor;
using UnityEngine;

public class LightHandler : BaseHandler<LightHandler, LightManager>
{

    private void Update()
    {
        if (GameHandler.Instance.manager.GetGameState() == GameStateEnum.Gaming)
        {
            TimeBean gameTime = GameTimeHandler.Instance.manager.GetGameTime();
            HandleForLightColor(gameTime);
            HandleForLightTransform(gameTime);
        }
    }

    /// <summary>
    /// 处理-光照颜色
    /// </summary>
    /// <param name="gameTime"></param>
    public void HandleForLightColor(TimeBean gameTime)
    {
        float lerpColor;
        Color lightTopColor;
        Color lightMiddleColor;
        Color lightBottomColor;

        if (gameTime.hour >= 0 && gameTime.hour < 12)
        {
            lerpColor = (gameTime.hour * 60 + gameTime.minute) / (float)(12 * 60);
            lightTopColor = Color.Lerp(manager.mainLightTopEnd, manager.mainLightTopStart, lerpColor);
            lightMiddleColor = Color.Lerp(manager.mainLightMiddleEnd, manager.mainLightMiddleStart, lerpColor);
            lightBottomColor = Color.Lerp(manager.mainLightBottomEnd, manager.mainLightBottomStart, lerpColor);
        }
        else
        {
            lerpColor = ((gameTime.hour - 12) * 60 + gameTime.minute) / (float)(12 * 60);
            lightTopColor = Color.Lerp(manager.mainLightTopStart, manager.mainLightTopEnd, lerpColor);
            lightMiddleColor = Color.Lerp(manager.mainLightMiddleStart, manager.mainLightMiddleEnd, lerpColor);
            lightBottomColor = Color.Lerp(manager.mainLightBottomStart, manager.mainLightBottomEnd, lerpColor);
        }
        //天空盒颜色
        manager.SetSkyBoxColor(lightTopColor, lightMiddleColor, lightBottomColor);
        //设置主光照颜色
        manager.SetMainLightColor(lightBottomColor);
        //设置环境光颜色
        manager.SetAmbientLight(lightTopColor * 0.7f);
    }

    /// <summary>
    /// 处理-光照位置旋转
    /// </summary>
    /// <param name="gameTime"></param>
    public void HandleForLightTransform(TimeBean gameTime)
    {
        SceneElementSky sky = SceneElementHandler.Instance.manager.sky;
        Vector3 position;
        Vector3 angle;
        if (gameTime.hour >= 6 && gameTime.hour <= 18)
        {
            position = sky.objSun.transform.position;
            angle = sky.objSun.transform.eulerAngles;
        }
        else
        {
            position = sky.objMoon.transform.position;
            angle = sky.objMoon.transform.eulerAngles;
        }
        manager.mainLight.transform.position = position;
        manager.mainLight.transform.eulerAngles = angle;
    }
}