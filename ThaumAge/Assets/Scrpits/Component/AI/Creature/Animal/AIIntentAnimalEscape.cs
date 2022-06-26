using UnityEditor;
using UnityEngine;

public class AIIntentAnimalEscape : AIBaseIntent
{
    public override void IntentEntering(AIBaseEntity aiEntity)
    {
        AIAnimalEntity aiCreatureEntity = aiEntity as AIAnimalEntity;
        bool isFindPath = aiCreatureEntity.aiNavigation.GetRandomRangeMovePosition(aiCreatureEntity.transform.position, 30, out Vector3 targetPosition);
        if (isFindPath)
        {
            //设置移动点
            aiCreatureEntity.aiNavigation.SetMovePosition(targetPosition);
            aiCreatureEntity.aiNavigation.SetMoveSpeed(aiCreatureEntity.creatureCpt.creatureInfo.speed_move * 5f);
            //播放移动动画
            aiCreatureEntity.creatureCpt.creatureAnim.PlayBaseAnim(CreatureAnimBaseState.Walk);
            aiCreatureEntity.creatureCpt.creatureAnim.SetAnimSpeed(3);
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
            aiEntity.ChangeIntent(AIIntentEnum.AnimalIdle);
        }
    }

    public override void IntentLeaving(AIBaseEntity aiEntity)
    {
        AIAnimalEntity aiCreatureEntity = aiEntity as AIAnimalEntity;
        aiCreatureEntity.aiNavigation.SetMoveSpeed(aiCreatureEntity.creatureCpt.creatureInfo.speed_move);
        aiCreatureEntity.creatureCpt.creatureAnim.SetAnimSpeed(1);

        aiCreatureEntity.aiNavigation.StopMove();
    }
}