using UnityEditor;
using UnityEngine;

public class CreatureCptBaseAnimal : CreatureCptBase
{
    protected AIAnimalEntity aiEntity;

    protected float timeUpdateForData = 0;
    protected float timeForData = 10;
    public override void Awake()
    {
        base.Awake();
        aiEntity = gameObject.AddComponentEX<AIAnimalEntity>();
        aiEntity.SetData(this);
    }

    public override void Update()
    {
        base.Update();
        //每隔10S加一次生命
        timeUpdateForData += Time.deltaTime;
        if (timeUpdateForData >= timeForData)
        {
            HandleForUpdateData();
        }
    }

    /// <summary>
    /// 处理-更新数据 
    /// </summary>
    public virtual void HandleForUpdateData()
    {
        CreatureStatusBean creatureStatus = creatureData.GetCreatureStatus();
        creatureStatus.HealthChange(1);
        timeUpdateForData = 0;
        //刷新血条
        creatureBattle.RefreshLifeProgress();
    }

    /// <summary>
    /// 被攻击
    /// </summary>
    /// <param name="atkObj"></param>
    /// <param name="damage"></param>
    public override void UnderAttack(GameObject atkObj, DamageBean damageData)
    {
        base.UnderAttack(atkObj, damageData);
        //逃跑
        aiEntity.ChangeIntent(AIIntentEnum.AnimalEscape);
    }
}