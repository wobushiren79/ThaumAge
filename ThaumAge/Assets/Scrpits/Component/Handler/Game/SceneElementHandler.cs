using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SceneElementHandler : BaseHandler<SceneElementHandler, SceneElementManager>
{
    //天空旋转角度
    public float timeForSkyAngle = 0;

    protected List<SceneElementBlockBase> listSceneElementBlock = new List<SceneElementBlockBase>();

    protected float timeUpdateElementBlock = 0;
    protected float timeUpdateElementBlockMax = 1;
    public void Update()
    {
        if (GameHandler.Instance.manager.GetGameState() == GameStateEnum.Gaming)
        {
            TimeBean gameTime = GameTimeHandler.Instance.manager.GetGameTime();
            HandleForSky(gameTime);
            HandleForStar(gameTime);

            timeUpdateElementBlock += Time.deltaTime;
            if (timeUpdateElementBlock > timeUpdateElementBlockMax)
            {
                timeUpdateElementBlock = 0;
                HandleForBlockElemental();
            }
        }
    }

    public void AddSceneElementBlock(SceneElementBlockBase sceneElementBlock)
    {
        listSceneElementBlock.Add(sceneElementBlock);
    }

    public void RemoveSceneElementBlock(SceneElementBlockBase sceneElementBlock)
    {
        listSceneElementBlock.Remove(sceneElementBlock);
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

    /// <summary>
    /// 处理方块元素
    /// </summary>
    public void HandleForBlockElemental()
    {
        if (listSceneElementBlock.Count > 0)
        {
            for (int i = 0; i < listSceneElementBlock.Count; i++)
            {
                var blockElement = listSceneElementBlock[i];
                blockElement.Update();
            }
        }
    }

    /// <summary>
    /// 创建场景元素
    /// </summary>
    public void CreateSceneElementBlock(SceneElementBlockBean sceneElementBlockData)
    {
        for (int i = 0; i < listSceneElementBlock.Count; i++)
        {
            var itemData = listSceneElementBlock[i];
            //如果该位置已经有了 则不能再创建新的
            if (itemData.sceneElementBlockData.position == sceneElementBlockData.position)
            {
                return;
            }
        }
        ElementalTypeEnum elementalType = sceneElementBlockData.GetElementalType();
        if (elementalType == ElementalTypeEnum.Fire)
        {
            CreateSceneElementBlockFire(sceneElementBlockData);
        }
    }

    protected void CreateSceneElementBlockFire(SceneElementBlockBean sceneElementBlockData)
    {
        SceneElementBlockFire sceneElementBlock = new SceneElementBlockFire();
        sceneElementBlock.SetData(sceneElementBlockData);
    }

}