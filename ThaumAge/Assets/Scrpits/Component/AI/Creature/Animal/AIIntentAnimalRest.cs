using UnityEditor;
using UnityEngine;

public class AIIntentAnimalRest : AIBaseIntent
{
    protected TimeBean timeData;
    public override void IntentEntering(AIBaseEntity aiEntity)
    {
        AIAnimalEntity aiCreatureEntity = aiEntity as AIAnimalEntity;
        //获取游戏时间
        timeData = GameTimeHandler.Instance.manager.GetGameTime();
        //播放闲置动画
        aiCreatureEntity.creatureCpt.creatureAnim.PlayBaseAnim(CharacterAnimBaseState.Seat);
    }

    public override void IntentUpdate(AIBaseEntity aiEntity)
    {
        if (timeData.hour >= 6)
        {
            //休息结束
            aiEntity.ChangeIntent(AIIntentEnum.AnimalIdle);
        }
    }

    public override void IntentLeaving(AIBaseEntity aiEntity)
    {
        base.IntentLeaving(aiEntity);
    }
}