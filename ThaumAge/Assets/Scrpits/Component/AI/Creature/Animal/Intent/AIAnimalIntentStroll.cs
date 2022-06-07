using UnityEditor;
using UnityEngine;

public class AIAnimalIntentStroll : AIBaseIntent
{
    //路径寻找范围
    public float disFindPath = 5;

    //闲逛时间
    public float timeForStroll = 0;
    //闲逛更新时间
    public float timeUpdateForStroll = 0;

    //路径搜索间隔
    public float timeForFindPath = 1;
    //路径搜索更新时间
    public float timeUpdateForFindPath = 0;

    //是否寻找到路径
    public bool isFindPath = false;

    public override void IntentEntering(AIBaseEntity aiEntity)
    {
        //随机设置闲逛时间
        timeForStroll = Random.Range(10f , 30f);
        //默认一开始就搜索一次
        timeUpdateForFindPath = timeForFindPath;
    }

    public override void IntentUpdate(AIBaseEntity aiEntity)
    {
        timeUpdateForStroll += Time.deltaTime;
        if (timeUpdateForStroll >= timeForStroll)
        {
            //如果闲逛时间结束
            aiEntity.ChangeIntent(AIAnimalIntentEnum.Idle);
            timeUpdateForStroll = 0;
            return;
        }
        //判断是否找到想要前往的地点
        if (!isFindPath)
        {
            //路径搜索
            timeUpdateForFindPath += Time.deltaTime;
            if (timeUpdateForFindPath >= timeForFindPath)
            {
                AIAnimalEntity aiCreatureEntity = aiEntity as AIAnimalEntity;
                isFindPath = aiCreatureEntity.aiNavigation.GetRandomRangeMovePosition(aiCreatureEntity.transform.position, disFindPath, out Vector3 targetPosition);
                if (isFindPath)
                {
                    //设置移动点
                    aiCreatureEntity.aiNavigation.SetMovePosition(targetPosition);
                    //播放移动动画
                    aiCreatureEntity.creatureCpt.creatureAnim.PlayBaseAnim(CharacterAnimBaseState.Walk);
                }
                timeUpdateForFindPath = 0;
            }
        }
        //如果已经找到路径了
        else
        {
            AIAnimalEntity aiCreatureEntity = aiEntity as AIAnimalEntity;
            bool isMove = aiCreatureEntity.aiNavigation.IsMove();
            //如果已经停止移动 则搜索新的路径
            if (!isMove)
            {
                aiCreatureEntity.creatureCpt.creatureAnim.PlayBaseAnim(CharacterAnimBaseState.Idle);
                isFindPath = false;
            }
        }
    }

    public override void IntentLeaving(AIBaseEntity aiEntity)
    {
        timeForStroll = 0;
        timeUpdateForStroll = 0;
        timeUpdateForFindPath = 0;
        isFindPath = false;
    }
}