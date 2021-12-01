﻿using UnityEditor;
using UnityEngine;

public abstract class AIBaseIntent
{
    public AIBaseEntity aiEntity;
    public System.Enum aiIntent;

    public AIBaseIntent()
    {

    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    /// <param name="aiIntent"></param>
    /// <param name="aiEntity"></param>
    public void InitData(System.Enum aiIntent, AIBaseEntity aiEntity)
    {
        this.aiIntent = aiIntent;
        this.aiEntity = aiEntity;
    }

    /// <summary>
    /// 进入意图
    /// </summary>
    public virtual void IntentEntering(AIBaseEntity aiEntity)
    { 

    }

    /// <summary>
    /// 表现意图
    /// </summary>
    public virtual void IntentUpdate(AIBaseEntity aiEntity)
    {

    }

    public virtual void IntentFixUpdate(AIBaseEntity aiEntity)
    {

    }

    /// <summary>
    /// 离开意图
    /// </summary>
    public virtual void IntentLeaving(AIBaseEntity aiEntity)
    { 
    
    }
}