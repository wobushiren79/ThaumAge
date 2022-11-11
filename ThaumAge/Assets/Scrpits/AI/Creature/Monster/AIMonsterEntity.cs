using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class AIMonsterEntity : AICreateEntity
{
    //追逐目标
    protected GameObject objChaseTarget;

    protected override void InitIntentEnum(List<AIIntentEnum> listIntentEnum)
    {
        listIntentEnum.Add(AIIntentEnum.MonsterIdle);
        listIntentEnum.Add(AIIntentEnum.MonsterStroll);
        listIntentEnum.Add(AIIntentEnum.MonsterChase);
        listIntentEnum.Add(AIIntentEnum.MonsterAttackMelee);
        listIntentEnum.Add(AIIntentEnum.MonsterAttackRemote);
        listIntentEnum.Add(AIIntentEnum.MonsterDead);
    }

    public override void SetData(CreatureCptBase creatureCpt)
    {
        base.SetData(creatureCpt);
        //默认闲置
        ChangeIntent(AIIntentEnum.MonsterIdle);
    }

    /// <summary>
    /// 近战攻击
    /// </summary>
    public void AttackMelee()
    {
        CreatureCptBaseMonster creatureCptMonster = creatureCpt as CreatureCptBaseMonster;
        creatureCptMonster.AttackMelee();
    }

    /// <summary>
    /// 远程攻击
    /// </summary>
    public void AttackRemote()
    {
        CreatureCptBaseMonster creatureCptMonster = creatureCpt as CreatureCptBaseMonster;
        creatureCptMonster.AttackRemote();
    }

    /// <summary>
    /// 获取追逐目标
    /// </summary>
    /// <returns></returns>
    public GameObject GetChaseTarget()
    {
        return objChaseTarget;
    }

    /// <summary>
    /// 设置追逐莫表
    /// </summary>
    /// <param name="objChaseTarget"></param>
    public void SetChaseTarget(GameObject objChaseTarget)
    {
        this.objChaseTarget = objChaseTarget;
    }

}