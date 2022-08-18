using UnityEditor;
using UnityEngine;

public class AIIntentMonsterAttackRemote : AIBaseIntent
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
        timeForAttack = aiCreatureEntity.creatureCpt.creatureInfo.time_interval_attack_remote;
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
            float disAttackMelee = aiCreatureEntity.creatureCpt.creatureInfo.dis_attack_melee;
            //如果超出攻击范围 就开始追逐
            if (disTarget > aiCreatureEntity.creatureCpt.creatureInfo.dis_attack_remote)
            {
                aiEntity.ChangeIntent(AIIntentEnum.MonsterChase);
                return;
            }
            //如果进入近战范围 并且有近战攻击方式 则使用近战
            else if (disAttackMelee != 0 && disTarget <= disAttackMelee)
            {
                aiEntity.ChangeIntent(AIIntentEnum.MonsterAttackMelee);
                return;
            }

            //攻击
            aiCreatureEntity.AttackRemote();
        }
    }

    public override void IntentLeaving(AIBaseEntity aiEntity)
    {
        timeUpdateForAttack = 0;
    }
}