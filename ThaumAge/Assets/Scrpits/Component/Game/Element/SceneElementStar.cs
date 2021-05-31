using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

public class SceneElementStar : SceneElementBase
{
    public VisualEffect starEffect;

    protected float starShowLerp = 0;
    protected float starShowLast = 0;

    public void Awake()
    {
        starEffect.playRate = 0.1f;
    }

    public void Update()
    {
        if (GameHandler.Instance.manager.GetGameState() == GameStateEnum.Gaming)
        {
            HandleForPosition();
            HandleForShow();
        }
    }

    public void HandleForShow()
    {
        starShowLast = Mathf.Lerp(starShowLast, starShowLerp, Time.deltaTime * 0.2f);
        starEffect.SetFloat("ShowLerp", starShowLast);
    }

    /// <summary>
    /// 展示星辰
    /// </summary>
    /// <param name="isShow"></param>
    public void ShowStar(bool isShow)
    {
        if (isShow)
        {
            starShowLerp = 1;
        }
        else
        {
            starShowLerp = 0;
        }
    }
}