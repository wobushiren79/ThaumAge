using UnityEditor;
using UnityEngine;
using DG.Tweening;

public class CreatureBattle : CreatureBase
{
    //刚体-生物
    protected Rigidbody rbCreature;
    //生物生命条
    protected CreatureCptLifeProgress lifeProgress;

    public CreatureBattle(CreatureCptBase creature, Rigidbody rbCreature) : base(creature)
    {
        this.rbCreature = rbCreature;
    }

    /// <summary>
    /// 遭到攻击
    /// </summary>
    /// <param name="damage"></param>
    public void UnderAttack(GameObject atkObj, int damage)
    {
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
        }
    }

    /// <summary>
    /// 击飞
    /// </summary>
    public void HitFly(GameObject atkObj)
    {
        if (atkObj == null)
        {
            //击退
            rbCreature.AddForce(new Vector3(0, 100, 0));
        }
        else
        {
            Vector3 hitDirection = (creature.transform.position - atkObj.transform.position).normalized + Vector3.up;
            rbCreature.AddForce(hitDirection * 100);
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
    /// 展示血条
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
}