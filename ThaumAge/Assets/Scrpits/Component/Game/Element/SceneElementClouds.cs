using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.VFX.Utility;
using UnityEngine.VFX;

public class SceneElementClouds : SceneElementBase
{
    protected VisualEffect cloundsVisualEffect;

    private void Start()
    {
        InitData();
    }

    public void InitData()
    {
        cloundsVisualEffect = GetComponent<VisualEffect>();
    }

    private void Update()
    {
        HandleForPosition();
    }

    public void ChangeCloudsColor(Color colorCloud, float changeTime)
    {

    }
}