﻿using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BaseUIView : BaseUIInit
{
    public RectTransform rectTransform;
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
}