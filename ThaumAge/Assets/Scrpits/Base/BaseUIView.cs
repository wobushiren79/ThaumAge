﻿using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BaseUIView : BaseUIInit
{
    protected RectTransform rectTransform;
    //原始UI大小
    protected Vector2 uiSizeOriginal;

    public override void Awake()
    {
        base.Awake();
        rectTransform = ((RectTransform)transform);
        uiSizeOriginal = rectTransform.sizeDelta;
    }

    public virtual void OnEnable()
    {
        RefreshUI();
    }

    public virtual void Show()
    {
        gameObject.ShowObj(true);
    }

    public virtual void Hide()
    {
        gameObject.ShowObj(false);
    }
}