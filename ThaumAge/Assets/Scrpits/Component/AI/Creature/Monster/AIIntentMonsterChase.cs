using UnityEditor;
using UnityEngine;

public class AIIntentMonsterChase : AIBaseIntent
{
    //路径搜索间隔
    public float timeForFindPath = 1;
    //路径搜索更新时间
    public float timeUpdateForFindPath = 0;
    //敌人追逐时间
    //public float timeForChaseEnemy = 30;
    //敌人追逐更新时间
    //public float timeUpdateForChaseEnemy = 0;

    //是否寻找到路径
    public bool isFindPath = false;

    public override void IntentEntering(AIBaseEntity aiEntity)
    {
        //默认开始搜索一次
        timeUpdateForFindPath = timeForFindPath;
    }

    public override void IntentUpdate(AIBaseEntity aiEntity)
    {
        HandleForFindPath();
    }

    public override void IntentLeaving(AIBaseEntity aiEntity)
    {
        timeUpdateForFindPath = 0;
        //timeUpdateForChaseEnemy = 0;

        AIMonsterEntity aiCreatureEntity = aiEntity as AIMonsterEntity;

        aiCreatureEntity.aiNavigation.SetMoveSpeed(aiCreatureEntity.creatureCpt.creatureInfo.speed_move);
        aiCreatureEntity.creatureCpt.creatureAnim.SetAnimSpeed(1);

        aiCreatureEntity.aiNavigation.StopMove();
    }

    /// <summary>
    /// 处理-路径
    /// </summary>
    public void HandleForFindPath()
    {
        //路径搜索 一直向目标移动
        timeUpdateForFindPath += Time.deltaTime;
        if (timeUpdateForFindPath >= timeForFindPath)
        {
            AIMonsterEntity aiCreatureEntity = aiEntity as AIMonsterEntity;
            GameObject objTarget = aiCreatureEntity.GetChaseTarget();
            if (objTarget == null)
            {
                aiCreatureEntity.ChangeIntent(AIIntentEnum.MonsterStroll);
                return;
            }
            float disTarget = Vector3.Distance(aiEntity.transform.position, objTarget.transform.position);
            //如果距离大于丢失距离 则不再追逐
            if (disTarget >= aiCreatureEntity.creatureCpt.creatureInfo.dis_loss)
            {
                aiCreatureEntity.SetChaseTarget(null);
                aiEntity.ChangeIntent(AIIntentEnum.MonsterStroll);
                return;
            }

            float disAttackMelee = aiCreatureEntity.creatureCpt.creatureInfo.dis_attack_melee;
            float disAttackRemote = aiCreatureEntity.creatureCpt.creatureInfo.dis_attack_remote;
            //如果位于远程攻击距离和近战攻击距离之间 并且有远程攻击方式 则使用远程攻击
            if ((disTarget > disAttackMelee && disTarget <= disAttackRemote) && disAttackRemote != 0)
            {
                aiEntity.ChangeIntent(AIIntentEnum.MonsterAttackRemote);
                return;
            }
            //如果是小于近战攻击距离
            else if (disTarget <= disAttackMelee)
            {
                //如果没有近战攻击方式 则使用远程
                if (disAttackMelee == 0)
                {
                    aiEntity.ChangeIntent(AIIntentEnum.MonsterAttackRemote);
                    return;
                }
                else
                {
                    aiEntity.ChangeIntent(AIIntentEnum.MonsterAttackMelee);
                    return;
                }
            }


            //移动到目标身边 或者身边2米的距离
            isFindPath = aiCreatureEntity.aiNavigation.SetMovePosition(objTarget.transform.position, true, 5);
            if (isFindPath)
            {
                //设置移动速度
                aiCreatureEntity.aiNavigation.SetMoveSpeed(aiCreatureEntity.creatureCpt.creatureInfo.speed_move * 3);
                //播放移动动画
                aiCreatureEntity.creatureCpt.creatureAnim.PlayBaseAnim(CreatureAnimBaseState.Walk);
                aiCreatureEntity.creatureCpt.creatureAnim.SetAnimSpeed(aiCreatureEntity.creatureCpt.creatureInfo.speed_move * 1.5f);
            }
            timeUpdateForFindPath = 0;
        }

    }
}