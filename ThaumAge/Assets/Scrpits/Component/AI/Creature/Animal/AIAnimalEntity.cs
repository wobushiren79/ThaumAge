using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AIAnimalEntity : AICreateEntity
{
    /// <summary>
    ///  初始化意图枚举
    /// </summary>
    /// <param name="listIntentEnum"></param>
    protected override void InitIntentEnum(List<AIIntentEnum> listIntentEnum)
    {
        listIntentEnum.Add(AIIntentEnum.AnimalIdle);
        listIntentEnum.Add(AIIntentEnum.AnimalStroll);
        listIntentEnum.Add(AIIntentEnum.AnimalRest);
        listIntentEnum.Add(AIIntentEnum.AnimalEscape);
        listIntentEnum.Add(AIIntentEnum.AnimalDead);
    }

    public override void SetData(CreatureCptBase creatureCpt)
    {
        base.SetData(creatureCpt);
        //默认闲置
        ChangeIntent(AIIntentEnum.AnimalIdle);
    }
}