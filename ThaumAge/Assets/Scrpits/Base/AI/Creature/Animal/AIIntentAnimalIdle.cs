using UnityEditor;
using UnityEngine;

public class AIIntentAnimalIdle : AIBaseIntent
{
    //闲置更新时间
    protected float timeUpdateForIdle = 0;
    //本次闲置时间
    protected float timeForIdle = 0;

    public override void IntentEntering(AIBaseEntity aiEntity)
    {
        AIAnimalEntity aiCreatureEntity = aiEntity as AIAnimalEntity;
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
            //获取游戏时间
            TimeBean timeData = GameTimeHandler.Instance.manager.GetGameTime();
            //如果0-6点就休息
            if (timeData.hour >= 0 && timeData.hour < 6)
            {
                //开始休息
                aiEntity.ChangeIntent(AIIntentEnum.AnimalRest);
            }
            else
            {
                //开始闲逛
                aiEntity.ChangeIntent(AIIntentEnum.AnimalStroll);
            }
        }
    }

    public override void IntentLeaving(AIBaseEntity aiEntity)
    {
        base.IntentLeaving(aiEntity);
        timeUpdateForIdle = 0;
        timeForIdle = 0;
    }
}