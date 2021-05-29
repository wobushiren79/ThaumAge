using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SceneElementHandler : BaseHandler<SceneElementHandler, SceneElementManager>
{
    //天空旋转角度
    public float timeForSkyAngle = 0;

    public void Update()
    {
        if (GameHandler.Instance.manager.GetGameState() == GameStateEnum.Gaming)
        {
            TimeBean gameTime = GameTimeHandler.Instance.manager.GetGameTime();
            HandleForSky(gameTime);
            HandleForStar(gameTime);
        }
    }

    /// <summary>
    /// 处理-天空
    /// </summary>
    public void HandleForSky(TimeBean gameTime)
    {
        float totalTime = 24f * 60f;
        float currentTime = gameTime.hour * 60 + gameTime.minute;
        timeForSkyAngle = (currentTime / totalTime * 360) + 180;

        Quaternion rotate = Quaternion.AngleAxis(timeForSkyAngle, new Vector3(1, 0, 1));
        manager.sky.transform.rotation = Quaternion.Lerp(manager.sky.transform.rotation, rotate, Time.deltaTime);
    }

    /// <summary>
    /// 处理-星星
    /// </summary>
    /// <param name="gameTime"></param>
    public void HandleForStar(TimeBean gameTime)
    {
        if (gameTime.hour >= 6 && gameTime.hour <= 18)
        {
            manager.star.ShowStar(false);
        }
        else
        {
            manager.star.ShowStar(true);
        }
    }
}