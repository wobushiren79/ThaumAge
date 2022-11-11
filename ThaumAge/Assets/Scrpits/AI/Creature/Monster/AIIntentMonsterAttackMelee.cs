using UnityEditor;
using UnityEngine;

public class AIIntentMonsterAttackMelee : AIBaseIntent
{
    //时间-攻击间隔
    protected float timeForAttack = 1;
    protected float timeUpdateForAttack = 0;

    public override void IntentEntering(AIBaseEntity aiEntity)
    {
        //默认一开始攻击一次
        timeUpdateForAttack = timeForAttack;
        AIMonsterEntity aiCreatureEntity = aiEntity as AIMonsterEntity;
        //设置攻击间隔
        timeForAttack = aiCreatureEntity.creatureCpt.creatureInfo.time_interval_attack_melee;
    }


    public override void IntentUpdate(AIBaseEntity aiEntity)
    {
        timeUpdateForAttack += Time.deltaTime;
        if (timeUpdateForAttack >= timeForAttack)
        {
            timeUpdateForAttack = 0;
            AIMonsterEntity aiCreatureEntity = aiEntity as AIMonsterEntity;
            GameObject objTarget = aiCreatureEntity.GetChaseTarget();
            if (objTarget == null)
            {
                aiCreatureEntity.ChangeIntent(AIIntentEnum.MonsterStroll);
                return;
            }
            float disTarget = Vector3.Distance(aiEntity.transform.position, objTarget.transform.position);
            //如果超出攻击范围 就开始追逐
            if (disTarget > aiCreatureEntity.creatureCpt.creatureInfo.dis_attack_melee)
            {
                aiEntity.ChangeIntent(AIIntentEnum.MonsterChase);
                return;
            }

            //攻击
            aiCreatureEntity.AttackMelee();
        }
    }

    public override void IntentLeaving(AIBaseEntity aiEntity)
    {
        timeUpdateForAttack = 0;
    }
}