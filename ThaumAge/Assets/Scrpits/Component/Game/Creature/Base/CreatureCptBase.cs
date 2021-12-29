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
    //生物生命条
    protected CreatureCptLifeProgress lifeProgress;

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
        //展示血条
        ShowLifeProgress();
    }

    /// <summary>
    /// 展示血条
    /// </summary>
    public void ShowLifeProgress()
    {
        if (lifeProgress == null)
        {
            Player player = GameHandler.Instance.manager.player;
            if (player.GetCharacter() == this)
            {
                //如果是玩家自己 则不显示血条
            }
            else
            {
                //如果是其他生物 则显示血条
                lifeProgress = CreatureHandler.Instance.CreateCreatureLifeProgress(gameObject);
            }
        }
        if (lifeProgress != null)
            lifeProgress.SetData(creatureData.maxLife, creatureData.currentLife);
    }
}