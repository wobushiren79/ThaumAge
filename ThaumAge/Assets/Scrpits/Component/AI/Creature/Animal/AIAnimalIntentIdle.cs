using UnityEditor;
using UnityEngine;

public class AIAnimalIntentIdle : AIBaseIntent
{
    public override void IntentEntering(AIBaseEntity aiEntity)
    {
        base.IntentEntering(aiEntity);
        AIAnimalEntity aiCreatureEntity = aiEntity as AIAnimalEntity;
        aiEntity.WaitExecuteSeconds(1, () =>
        {
            aiCreatureEntity.aiNavigation.SetMovePosition(new Vector3(Random.Range(-10f, 10f), aiEntity.transform.position.y, Random.Range(-10f, 10f)));
        });
    }

    public override void IntentUpdate(AIBaseEntity aiEntity)
    {
        base.IntentUpdate(aiEntity);

        AIAnimalEntity aiCreatureEntity = aiEntity as AIAnimalEntity;

        if (!aiCreatureEntity.aiNavigation.IsMove())
        {
            aiCreatureEntity.aiNavigation.SetMovePosition(new Vector3(Random.Range(-10f, 10f), aiEntity.transform.position.y, Random.Range(-10f, 10f)));
        }

    }
}