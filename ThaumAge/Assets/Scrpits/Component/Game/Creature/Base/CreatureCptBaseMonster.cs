﻿using UnityEditor;
using UnityEngine;

public class CreatureCptBaseMonster : CreatureCptBase
{
    protected AIMonsterEntity aiEntity;

    protected float timeUpdateForData = 0;
    protected float timeForData = 10;

    public override void Awake()
    {
        base.Awake();
        aiEntity = gameObject.AddComponentEX<AIMonsterEntity>();
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
        creatureData.AddLife(1);
        timeUpdateForData = 0;
        //刷新血条
        creatureBattle.RefreshLifeProgress();
    }

    /// <summary>
    /// 近战攻击
    /// </summary>
    public void AttackMelee()
    {        
        //获取打中的目标
        Collider[] targetArray = CombatCommon.TargetCheck(gameObject, 2, 2, 2, 1 << LayerInfo.Character);
        //伤害打中的目标
        CombatCommon.DamageTarget(gameObject, 2, targetArray);
    }

    /// <summary>
    /// 远程攻击
    /// </summary>
    public void AttackRemote()
    {
        Debug.LogError("AttackRemote");
    }
}