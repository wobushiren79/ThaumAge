using UnityEditor;
using UnityEngine;

public class AICreatureIntentIdle : AIBaseIntent
{
    public override void IntentEntering(AIBaseEntity aiEntity)
    {
        base.IntentEntering(aiEntity);
        AICreatureEntity aiCreatureEntity = aiEntity as AICreatureEntity;
        aiEntity.WaitExecuteSeconds(1, () =>
        {
            aiCreatureEntity.aiNavigation.SetMovePosition(new Vector3(Random.Range(-10f, 10f), aiEntity.transform.position.y, Random.Range(-10f, 10f)));
            aiCreatureEntity.isInit = true;
        });
    }

    public override void IntentUpdate(AIBaseEntity aiEntity)
    {
        base.IntentUpdate(aiEntity);

        AICreatureEntity aiCreatureEntity = aiEntity as AICreatureEntity;

        if (aiCreatureEntity.isInit)
        {
            if (!aiCreatureEntity.aiNavigation.IsMove())
            {
                aiCreatureEntity.aiNavigation.SetMovePosition(new Vector3(Random.Range(-10f, 10f), aiEntity.transform.position.y, Random.Range(-10f, 10f)));
            }
        }

    }
}