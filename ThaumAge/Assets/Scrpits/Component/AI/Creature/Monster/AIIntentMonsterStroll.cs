using UnityEditor;
using UnityEngine;

public class AIIntentMonsterStroll : AIBaseIntent
{
    //路径寻找范围
    public float disFindPath = 5;

    //路径搜索间隔
    public float timeForFindPath = 1;
    //路径搜索更新时间
    public float timeUpdateForFindPath = 0;

    //是否寻找到路径
    public bool isFindPath = false;

    public override void IntentEntering(AIBaseEntity aiEntity)
    {
        //默认一开始就搜索一次
        timeUpdateForFindPath = timeForFindPath;
    }

    public override void IntentUpdate(AIBaseEntity aiEntity)
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
                    aiCreatureEntity.aiNavigation.SetMoveSpeed(1f);
                    //播放移动动画
                    aiCreatureEntity.creatureCpt.creatureAnim.PlayBaseAnim(CreatureAnimBaseState.Walk);
                    aiCreatureEntity.creatureCpt.creatureAnim.SetAnimSpeed(0.7f);
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

    public override void IntentLeaving(AIBaseEntity aiEntity)
    {
        timeUpdateForFindPath = 0;
        isFindPath = false;
    }
}