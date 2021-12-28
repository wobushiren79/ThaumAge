using UnityEditor;
using UnityEngine;

public class CreatureCptBase : BaseMonoBehaviour
{
    //生物基础动画
    public AnimForCreature animForCreature;
    //生物信息
    public CreatureInfoBean creatureInfo;
    //生物数据
    public CreatureBean creatureData;

    public virtual void Awake()
    {
        Animator aiAnimator = GetComponentInChildren<Animator>();
        animForCreature = new AnimForCreature(aiAnimator);
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    public void SetData(CreatureInfoBean creatureInfo)
    {
        this.creatureInfo = creatureInfo;
        creatureData = new CreatureBean();
        creatureData.maxLife = creatureInfo.life;
        creatureData.currentLife = creatureInfo.life;
    }

    /// <summary>
    /// 遭到攻击
    /// </summary>
    /// <param name="damage"></param>
    public void UnderAttack(int damage)
    {
        //展示伤害数值特效
        EffectBean effectData = new EffectBean();
        effectData.effectName = EffectInfo.DamageText_1;
        effectData.effectType = EffectTypeEnum.Normal;
        effectData.timeForShow = 1f;
        effectData.effectPosition = transform.position + new Vector3(0, 1, 0);
        EffectHandler.Instance.ShowEffect(effectData, (effect) =>
        {
            EffectDamageText damageText = effect as EffectDamageText;
            damageText.SetData($"{damage}");
        });
    }
}