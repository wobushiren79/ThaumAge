using UnityEditor;
using UnityEngine;

public class AIIntentGolemIdle : AIBaseIntent
{
    //闲置更新时间
    protected float timeUpdateForIdle = 0;
    //本次闲置时间
    protected float timeForIdle = 1;

    protected AIIntentEnum aiIntentWork = AIIntentEnum.GolemIdle;
    public override void IntentEntering(AIBaseEntity aiEntity)
    {
        var aiGolemEntity = aiEntity as AIGolemEntity;
        if (aiGolemEntity.queueWorkIntent.Count == 0)
            return;
        aiIntentWork = aiGolemEntity.queueWorkIntent.Dequeue();
        aiGolemEntity.queueWorkIntent.Enqueue(aiIntentWork);
    }

    public override void IntentUpdate(AIBaseEntity aiEntity)
    {
        timeUpdateForIdle += Time.deltaTime;
        if (timeUpdateForIdle >= timeForIdle)
        {
            aiEntity.ChangeIntent(aiIntentWork);
        }
    }

    public override void IntentLeaving(AIBaseEntity aiEntity)
    {
        base.IntentLeaving(aiEntity);
        timeUpdateForIdle = 0;
        aiIntentWork = AIIntentEnum.GolemIdle;
    }
}