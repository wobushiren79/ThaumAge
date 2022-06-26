using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AIIntentMonsterStroll : AIBaseIntent
{
    //路径寻找范围
    public float disFindPath = 5;
    //搜索半径
    public float disSearchRange = 10;

    //路径搜索间隔
    public float timeForFindPath = 1;
    //路径搜索更新时间
    public float timeUpdateForFindPath = 0;
    //敌人搜索间隔
    public float timeForSearchEnemy = 1;
    //敌人搜索更新时间
    public float timeUpdateForSearchEnemy = 0;

    //是否寻找到路径
    public bool isFindPath = false;


    public override void IntentEntering(AIBaseEntity aiEntity)
    {
        AIMonsterEntity aiCreatureEntity = aiEntity as AIMonsterEntity;
        //设置视野距离
        disSearchRange = aiCreatureEntity.creatureCpt.creatureInfo.sight_range;
        //默认一开始就搜索一次
        timeUpdateForFindPath = timeForFindPath;
    }

    public override void IntentUpdate(AIBaseEntity aiEntity)
    {
        HandleForFindPath();
        HandleForSearchEnemy();
    }

    public override void IntentLeaving(AIBaseEntity aiEntity)
    {
        timeUpdateForFindPath = 0;
        timeUpdateForSearchEnemy = 0;
        isFindPath = false;

        AIMonsterEntity aiCreatureEntity = aiEntity as AIMonsterEntity;
        aiCreatureEntity.aiNavigation.StopMove();
    }

    /// <summary>
    /// 处理-路径
    /// </summary>
    public void HandleForFindPath()
    {
        //判断是否找到想要前往的地点
        if (!isFindPath)
        {
            //路径搜索
            timeUpdateForFindPath += Time.deltaTime;
            if (timeUpdateForFindPath >= timeForFindPath)
            {
                AIMonsterEntity aiCreatureEntity = aiEntity as AIMonsterEntity;
                isFindPath = aiCreatureEntity.aiNavigation.GetRandomRangeMovePosition(aiCreatureEntity.transform.position, disFindPath, out Vector3 targetPosition);
                if (isFindPath)
                {
                    //设置移动点
                    aiCreatureEntity.aiNavigation.SetMovePosition(targetPosition);
                    aiCreatureEntity.aiNavigation.SetMoveSpeed(aiCreatureEntity.creatureCpt.creatureInfo.speed_move);
                    //播放移动动画
                    aiCreatureEntity.creatureCpt.creatureAnim.PlayBaseAnim(CreatureAnimBaseState.Walk);
                    aiCreatureEntity.creatureCpt.creatureAnim.SetAnimSpeed(1);
                }
                timeUpdateForFindPath = 0;
            }
        }
        //如果已经找到路径了
        else
        {
            AIMonsterEntity aiCreatureEntity = aiEntity as AIMonsterEntity;
            bool isMove = aiCreatureEntity.aiNavigation.IsMove();
            //如果已经停止移动 则搜索新的路径
            if (!isMove)
            {
                aiCreatureEntity.creatureCpt.creatureAnim.PlayBaseAnim(CreatureAnimBaseState.Idle);
                isFindPath = false;
            }
        }
    }

    /// <summary>
    /// 处理-搜索敌人
    /// </summary>
    public void HandleForSearchEnemy()
    {
        timeUpdateForSearchEnemy += Time.deltaTime;
        if (timeUpdateForSearchEnemy >= timeForSearchEnemy)
        {
            timeUpdateForSearchEnemy = 0;
            //开始搜索一次范围内的敌人
            List<Collider> listSearchTarget = AIBaseCommon.SightSearchCircle(aiEntity.transform.position + new Vector3(0, 0.5f, 0), disSearchRange, 1 << LayerInfo.Character, 1 << LayerInfo.ChunkCollider);
            if (listSearchTarget.IsNull())
                return;
            AIMonsterEntity aiCreatureEntity = aiEntity as AIMonsterEntity;
            aiCreatureEntity.objChaseTarget = listSearchTarget[0].gameObject;
            aiEntity.ChangeIntent(AIIntentEnum.MonsterChase);
        }
    }


}