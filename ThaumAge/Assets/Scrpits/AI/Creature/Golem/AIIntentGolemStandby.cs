using UnityEditor;
using UnityEngine;

public class AIIntentGolemStandby : AIBaseIntent
{
    public override void IntentEntering(AIBaseEntity aiEntity)
    {
        var aiGolemEntity = aiEntity as AIGolemEntity;
        //播放闲置动画
        aiGolemEntity.creatureCpt.creatureAnim.PlayBaseAnim(CreatureAnimBaseState.Idle);
    }
    public override void IntentUpdate(AIBaseEntity aiEntity)
    {

    }
    public override void IntentLeaving(AIBaseEntity aiEntity)
    {

    }
}