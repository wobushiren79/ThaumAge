using UnityEditor;
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
        //获取攻击范围
        CombatCommon.GetRangeDamage(creatureInfo.range_damage, out float lengthRangeDamage, out float widthRangeDamage, out float heightRangeDamage);
        //获取打中的目标
        Collider[] targetArray = CombatCommon.TargetCheck(gameObject, lengthRangeDamage, widthRangeDamage, heightRangeDamage, 1 << LayerInfo.Character);
        //伤害打中的目标
        CombatCommon.DamageTarget(gameObject, 2, targetArray);
        //调整身体角度
        LookTarget();
    }

    /// <summary>
    /// 远程攻击
    /// </summary>
    public void AttackRemote()
    {
        Debug.LogError("AttackRemote");
        MathUtil.GetBezierPoints(20,transform.position, aiEntity.objChaseTarget.transform.position,10);
        //调整身体角度
        LookTarget();
    }

    /// <summary>
    /// 调整身体角度
    /// </summary>
    public void LookTarget()
    {
        Vector3 lookAngle =  GameUtil.GetLookAtEuler(transform.position, aiEntity.objChaseTarget.transform.position);
        Quaternion rotate = Quaternion.Euler(new Vector3(0, lookAngle.y, 0));
        //朝摄像头方向移动
        transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.unscaledDeltaTime);
    }
}