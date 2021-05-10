using UnityEditor;
using UnityEngine;

public class SceneElementStar : SceneElementBase
{
    public void Update()
    {
        if (GameHandler.Instance.manager.GetGameState() == GameStateEnum.Gaming)
        {
            HandleForPosition();
        }
    }
}