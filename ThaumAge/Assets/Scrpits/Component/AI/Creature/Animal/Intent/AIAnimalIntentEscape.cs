using UnityEditor;
using UnityEngine;

public class AIAnimalIntentEscape : AIBaseIntent
{
    public override void IntentEntering(AIBaseEntity aiEntity)
    {
        AIAnimalEntity aiCreatureEntity = aiEntity as AIAnimalEntity;
        bool isFindPath = aiCreatureEntity.aiNavigation.GetRandomRangeMovePosition(aiCreatureEntity.transform.position, 30, out Vector3 targetPosition);
        if (isFindPath)
        {
            //设置移动点
            aiCreatureEntity.aiNavigation.SetMovePosition(targetPosition);
            aiCreatureEntity.aiNavigation.SetMoveSpeed(5f);
            //播放移动动画
            aiCreatureEntity.creatureCpt.creatureAnim.PlayBaseAnim(CharacterAnimBaseState.Walk);
            aiCreatureEntity.creatureCpt.creatureAnim.SetAnimSpeed(5);
        }
    }

    public override void IntentUpdate(AIBaseEntity aiEntity)
    {
        AIAnimalEntity aiCreatureEntity = aiEntity as AIAnimalEntity;
        bool isMove = aiCreatureEntity.aiNavigation.IsMove();
        //如果已经停止移动 则搜索新的路径
        if (!isMove)
        {
            //跑到就不跑了
            aiEntity.ChangeIntent(AIAnimalIntentEnum.Idle);
        }
    }

    public override void IntentLeaving(AIBaseEntity aiEntity)
    {
        AIAnimalEntity aiCreatureEntity = aiEntity as AIAnimalEntity;
        aiCreatureEntity.aiNavigation.SetMoveSpeed(1f);
        aiCreatureEntity.creatureCpt.creatureAnim.SetAnimSpeed(1);
    }
}