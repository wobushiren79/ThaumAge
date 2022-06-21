using UnityEditor;
using UnityEngine;
public class AIIntentMonsterIdle : AIBaseIntent
{
    //闲置更新时间
    protected float timeUpdateForIdle = 0;
    //本次闲置时间
    protected float timeForIdle = 0;

    public override void IntentEntering(AIBaseEntity aiEntity)
    {
        AIMonsterEntity aiCreatureEntity = aiEntity as AIMonsterEntity;
        //播放闲置动画
        aiCreatureEntity.creatureCpt.creatureAnim.PlayBaseAnim(CreatureAnimBaseState.Idle);
        //随机设置闲置时间
        timeForIdle = Random.Range(5f, 10f);
    }

    public override void IntentUpdate(AIBaseEntity aiEntity)
    {
        timeUpdateForIdle += Time.deltaTime;
        if (timeUpdateForIdle >= timeForIdle)
        {
            //闲置结束 开始闲逛
            //开始闲逛
            aiEntity.ChangeIntent(AIIntentEnum.MonsterStroll);
        }
    }

    public override void IntentLeaving(AIBaseEntity aiEntity)
    {
        base.IntentLeaving(aiEntity);
        timeUpdateForIdle = 0;
        timeForIdle = 0;
    }
}