using UnityEditor;
using UnityEngine;
using DG.Tweening;

public class CreatureBattle : CreatureBase
{
    //生物生命条
    protected CreatureCptLifeProgress lifeProgress;

    //被击飞的冷却时间
    protected float timeCDForHitFly = 1;
    protected bool isHitFly = false;
    public CreatureBattle(CreatureCptBase creature) : base(creature)
    {

    }


    /// <summary>
    /// 遭到攻击
    /// </summary>
    /// <param name="damage"></param>
    public void UnderAttack(GameObject atkObj, int damage)
    {
        if (CheckIsDead())
            return;
         
        //扣除伤害
        creature.creatureData.AddLife(-damage);
        //展示伤害数值特效
        EffectBean effectData = new();
        effectData.effectName = EffectInfo.DamageText_1;
        effectData.effectType = EffectTypeEnum.Normal;
        effectData.timeForShow = 1f;
        effectData.effectPosition = creature.transform.position + new Vector3(0, 1, 0);
        EffectHandler.Instance.ShowEffect(effectData, (effect) =>
        {
            EffectDamageText damageText = effect as EffectDamageText;
            damageText.SetData($"{damage}");
        });
        //展示血条
        ShowLifeProgress();

        //如果伤害大于0则颤抖
        if (damage > 0)
        {
            //击飞
            HitFly(atkObj);
            //颤抖
            ShakeBody();
            //飙血
            ShowBlood();
        }

        //检测是否死亡
        if (CheckIsDead())
        {
            //隐藏生命条
            lifeProgress.ShowObj(false);
            //死亡
            creature.Dead();
        }
    }

    /// <summary>
    /// 检测是否死亡
    /// </summary>
    public bool CheckIsDead()
    {
        if (creature.creatureData.currentLife <= 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 击飞
    /// </summary>
    public void HitFly(GameObject atkObj)
    {
        //如果进入了击飞的CD则无法被击飞
        if (isHitFly)
            return;
        isHitFly = true;
        creature.WaitExecuteSeconds(timeCDForHitFly,()=> 
        {
            isHitFly = false;
        });
        //如果有攻击的物体
        if (atkObj == null)
        {
            //击退
            //rbCreature.AddForce(new Vector3(0, 100, 0));
        }
        else
        {
            Vector3 hitDirection = (creature.transform.position - atkObj.transform.position).normalized + Vector3.up;
            //rbCreature.AddForce(hitDirection * 100);
        }
    }

    /// <summary>
    /// 颤抖身体
    /// </summary>
    public void ShakeBody()
    {
        creature.transform.DOKill();
        creature.transform.DOShakeScale(0.1f, 0.1f);
    }

    /// <summary>
    /// 显示血条
    /// </summary>
    public void ShowLifeProgress()
    {
        if (lifeProgress == null)
        {
            Player player = GameHandler.Instance.manager.player;
            if (player.GetCharacter() == creature)
            {
                //如果是玩家自己 则不显示血条
            }
            else
            {
                //如果是其他生物 则显示血条
                lifeProgress = CreatureHandler.Instance.CreateCreatureLifeProgress(creature.gameObject);
            }
        }
        if (lifeProgress != null)
            lifeProgress.SetData(creature.creatureData.maxLife, creature.creatureData.currentLife);
    }

    /// <summary>
    /// 刷新血条
    /// </summary>
    public void RefreshLifeProgress()
    {
        if (lifeProgress != null)
            lifeProgress.RefreshData(creature.creatureData.maxLife, creature.creatureData.currentLife);
    }

    /// <summary>
    /// 飙血
    /// </summary>
    public void ShowBlood()
    {
        EffectBean effectData = new EffectBean();
        effectData.effectType = EffectTypeEnum.Visual;
        effectData.effectName = EffectInfo.Effect_Blood_1;
        effectData.effectPosition = creature.transform.position + Vector3.up;
        effectData.timeForShow = 0.5f;
        EffectHandler.Instance.ShowEffect(effectData,(effect)=> 
        {
            effect.PlayEffect();
        });
    }
}