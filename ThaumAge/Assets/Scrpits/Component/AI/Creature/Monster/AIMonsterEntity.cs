using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class AIMonsterEntity : AICreateEntity
{
    protected override void InitIntentEnum(List<AIIntentEnum> listIntentEnum)
    {
        listIntentEnum.Add(AIIntentEnum.MonsterIdle);
        listIntentEnum.Add(AIIntentEnum.MonsterStroll);
        listIntentEnum.Add(AIIntentEnum.MonsterChase);
        listIntentEnum.Add(AIIntentEnum.MonsterAttack);
        listIntentEnum.Add(AIIntentEnum.MonsterDead);
    }

    public override void SetData(CreatureCptBase creatureCpt)
    {
        base.SetData(creatureCpt);
        //默认闲置
        ChangeIntent(AIIntentEnum.MonsterIdle);
    }
}