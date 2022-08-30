using UnityEditor;
using UnityEngine;
using DG.Tweening;
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
        aiEntity.SetChaseTarget(atkObj);
        aiEntity.ChangeIntent(AIIntentEnum.MonsterChase);
    }

    /// <summary>
    /// 近战攻击
    /// </summary>
    public virtual void AttackMelee()
    {
        //获取攻击范围
        CombatCommon.GetRangeDamage(creatureInfo.range_damage, out float lengthRangeDamage, out float widthRangeDamage, out float heightRangeDamage);
        //获取打中的目标
        Collider[] targetArray = CombatCommon.TargetCheck(gameObject, lengthRangeDamage, widthRangeDamage, heightRangeDamage, 1 << LayerInfo.Character);
        //伤害打中的目标
        DamageBean damageData = creatureInfo.GetDamageData();
        CombatCommon.DamageTarget(gameObject, damageData, targetArray);
        //调整身体角度
        LookTarget();
        //播放攻击动画
        PlayAnimForAttackMelee();
    }


    /// <summary>
    /// 远程攻击
    /// </summary>
    public virtual void AttackRemote()
    {
        //调整身体角度
        LookTarget();
        //播放攻击动画
        PlayAnimForAttackRemote();
    }

    /// <summary>
    /// 播放近战攻击动画
    /// </summary>
    public virtual void PlayAnimForAttackMelee()
    {
        creatureAnim.PlayBaseAnim(CreatureAnimBaseState.AttackMelee);
    }

    /// <summary>
    /// 播放远程攻击动画
    /// </summary>
    public virtual void PlayAnimForAttackRemote()
    {
        creatureAnim.PlayBaseAnim(CreatureAnimBaseState.AttackRemote);
    }

    /// <summary>
    /// 调整身体角度
    /// </summary>
    public void LookTarget()
    {
        //Vector3 lookAngle =  GameUtil.GetLookAtEuler(transform.position, aiEntity.objChaseTarget.transform.position);
        GameObject objTarget = aiEntity.GetChaseTarget();
        Vector3 targetPosition = objTarget.transform.position;
        //朝摄像头方向移动
        transform.DOLookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.z), 0.5f);
    }
}