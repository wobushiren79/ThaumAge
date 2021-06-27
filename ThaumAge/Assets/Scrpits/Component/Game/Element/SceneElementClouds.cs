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

    public override Vector3 HandleForPosition()
    {
        Transform tfPlayer = GameHandler.Instance.manager.player.transform;
        transform.position = new Vector3(tfPlayer.position.x, 200, tfPlayer.position.z);
        return transform.position;
    }

    public void ChangeCloudsColor(Color colorCloud, float changeTime)
    {

    }
}