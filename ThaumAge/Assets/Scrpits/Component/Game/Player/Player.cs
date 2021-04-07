﻿using UnityEditor;
using UnityEngine;

public class Player : BaseMonoBehaviour
{
    protected float timeForWorldUpdate = 0;

    private void Update()
    {
        HandleForWorldUpdate();
    }

    /// <summary>
    /// 处理-世界刷新
    /// </summary>
    public void HandleForWorldUpdate()
    {
        timeForWorldUpdate -= Time.deltaTime;
        if (timeForWorldUpdate <= 0)
        {
            WorldCreateHandler.Instance.CreateChunkForRange(transform.position, 3);
            timeForWorldUpdate = 1;
        }
    }
}