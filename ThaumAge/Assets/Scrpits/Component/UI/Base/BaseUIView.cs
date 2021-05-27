﻿using UnityEditor;
using UnityEngine;

public class BaseUIView : BaseMonoBehaviour
{
    protected RectTransform rectTransform;
    //原始UI大小
    protected Vector2 uiSizeoOriginal;

    public virtual void Awake()
    {
        AutoLinkUI();
        rectTransform = ((RectTransform)transform);
        uiSizeoOriginal = rectTransform.sizeDelta;
    }

    protected virtual void OnEnable()
    {
        RefreshUI();
    }

    /// <summary>
    /// 刷新UI大小
    /// </summary>
    public virtual void RefreshUI()
    {
        GameConfigBean gameConfig = GameDataHandler.Instance.manager.GetGameConfig();
        ChangeUISize(gameConfig.uiSize);
    }

    /// <summary>
    /// 修改UI大小
    /// </summary>
    /// <param name="size"></param>
    public virtual void ChangeUISize(float size)
    {
        if (rectTransform != null)
            rectTransform.sizeDelta = new Vector2(uiSizeoOriginal.x * size, uiSizeoOriginal.y * size);
    }

}