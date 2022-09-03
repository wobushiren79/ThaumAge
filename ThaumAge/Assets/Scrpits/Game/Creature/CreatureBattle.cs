using UnityEditor;
using UnityEngine;
using DG.Tweening;

public class CreatureBattle : CreatureBase
{
    //生物生命条
    protected CreatureCptLifeProgress lifeProgress;

    //是否被击飞
    public bool isHitFly = false;

    public CreatureBattle(CreatureCptBase creature) : base(creature)
    {

    }

    /// <summary>
    /// 遭到攻击
    /// </summary>
    /// <param name="damage"></param>
    public void UnderAttack(GameObject atkObj, DamageBean damageData)
    {
        if (CheckIsDead())
            return;
        //如果是玩家
        CreatureTypeEnum creatureType = creature.creatureData.GetCreatureType();
        switch (creatureType)
        {
            case CreatureTypeEnum.Player:
                UnderAttackForPlayer(atkObj, damageData);
                break;
            default:
                UnderAttackForCreature(atkObj, damageData);
                break;
        }
    }

    protected void UnderAttackForPlayer(GameObject atkObj, DamageBean damageData)
    {
        //处理伤害数据
        damageData.ExecuteData(atkObj, creature,
            out int damage);
        if (damage > 0)
        {
            //颤抖
            ShakeBody();
            //摄像头抖动
            CameraHandler.Instance.ShakeCamera(0.25f);
        }
        //刷新UI
        EventHandler.Instance.TriggerEvent(EventsInfo.CharacterStatus_StatusChange);

        //检测是否死亡
        CheckIsDead();
    }

    protected void UnderAttackForCreature(GameObject atkObj, DamageBean damageData)
    {
        //处理伤害数据
        damageData.ExecuteData(atkObj, creature,
            out int damage);
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
        }
    }

    /// <summary>
    /// 检测是否死亡
    /// </summary>
    public bool CheckIsDead()
    {
        if (creature.creatureData.GetCreatureType() == CreatureTypeEnum.Player)
        {
            UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
            CreatureStatusBean creatureStatus = userData.characterData.GetCreatureStatus();
            if (creatureStatus.curHealth <= 0)
            {
                //死亡
                creature.Dead();
                return true;
            }
        }
        else
        {
            CreatureStatusBean creatureStatus = creature.creatureData.GetCreatureStatus();
            if (creatureStatus.curHealth <= 0)
            {
                //死亡
                creature.Dead();
                return true;
            }
        }
        return false;
    }


    /// <summary>
    /// 颤抖身体
    /// </summary>
    public void ShakeBody()
    {
        if (creature.creatureData.GetCreatureType() == CreatureTypeEnum.Player)
        {
            Player player = GameHandler.Instance.manager.player;
            player.transform.DOKill(true);
            player.transform.DOShakeScale(0.1f, 0.1f);
        }
        else
        {
            creature.transform.DOKill(true);
            creature.transform.DOShakeScale(0.1f, 0.1f);
        }
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
        {
            CreatureStatusBean creatureStatus = creature.creatureData.GetCreatureStatus();
            lifeProgress.SetData(creatureStatus.health, creatureStatus.curHealth);
        }
    }

    /// <summary>
    /// 刷新血条
    /// </summary>
    public void RefreshLifeProgress()
    {
        if (lifeProgress != null)
        {
            CreatureStatusBean creatureStatus = creature.creatureData.GetCreatureStatus();
            lifeProgress.RefreshData(creatureStatus.health, creatureStatus.curHealth);
        }
    }

    /// <summary>
    /// 飙血
    /// </summary>
    public void ShowBlood()
    {
        EffectBean effectData = new EffectBean();
        effectData.effectType = EffectTypeEnum.Visual;
        effectData.effectName = EffectInfo.Effect_Blood_1;
        effectData.effectPosition = creature.transform.position;
        effectData.timeForShow = 0.5f;
        EffectHandler.Instance.ShowEffect(effectData, (effect) =>
         {
             effect.PlayEffect();
         });
    }
}