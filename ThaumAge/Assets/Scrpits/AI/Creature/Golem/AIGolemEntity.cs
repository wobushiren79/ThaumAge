using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGolemEntity : AICreateEntity
{
    protected override void InitIntentEnum(List<AIIntentEnum> listIntentEnum)
    {
        //根据不同的核心 添加不同的意图
        listIntentEnum.Add(AIIntentEnum.GolemIdle);
    }

    public override void SetData(CreatureCptBase creatureCpt)
    {
        base.SetData(creatureCpt);
        //默认闲置
        ChangeIntent(AIIntentEnum.GolemIdle);
    }
}
