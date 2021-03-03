using UnityEditor;
using UnityEngine;

public abstract class AIBaseIntent
{
    public AIBaseEntity aiEntity;
    public AIIntentEnum aiIntent;

    public AIBaseIntent(AIIntentEnum aiIntent,AIBaseEntity aiEntity)
    {
        this.aiIntent = aiIntent;
        this.aiEntity = aiEntity;
    }

    /// <summary>
    /// 进入意图
    /// </summary>
    public abstract void IntentEntering();

    /// <summary>
    /// 表现意图
    /// </summary>
    public abstract void IntentActUpdate();
    public abstract void IntentActFixUpdate();

    /// <summary>
    /// 离开意图
    /// </summary>
    public abstract void IntentLeaving();
}