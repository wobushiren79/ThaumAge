using DG.Tweening;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DamageBean
{
    public Dictionary<AttributeTypeEnum, string> dicDamageData = new Dictionary<AttributeTypeEnum, string>();

    public DamageBean(string dataStr)
    {
        string[] itemDataStr = dataStr.SplitForArrayStr('|');
        for (int i = 0; i < itemDataStr.Length; i++)
        {
            string[] itemDetailsDataStr = itemDataStr[i].SplitForArrayStr(':');
            AttributeTypeEnum damageAdditionEnum = EnumExtension.GetEnum<AttributeTypeEnum>(itemDetailsDataStr[0]);
            switch (damageAdditionEnum)
            {
                case AttributeTypeEnum.Damage:
                case AttributeTypeEnum.DamageMagic:
                case AttributeTypeEnum.KnockbackDis:
                case AttributeTypeEnum.KnockbackTime:
                    dicDamageData.Add(damageAdditionEnum, itemDetailsDataStr[1]);
                    break;
            }
        }
    }


    /// <summary>
    /// 获取数据
    /// </summary>
    /// <param name="damageAddition"></param>
    /// <returns></returns>
    public string GetData(AttributeTypeEnum attributeType)
    {
        if (dicDamageData.TryGetValue(AttributeTypeEnum.Damage, out string value))
        {
            return value;
        }
        return null;
    }

    /// <summary>
    /// 获取伤害值
    /// </summary>
    /// <returns></returns>
    public int GetDamage()
    {
        string data = GetData(AttributeTypeEnum.Damage);
        if (data.IsNull())
        {
            return 0;
        }
        return int.Parse(data);
    }


    /// <summary>
    /// 执行数据
    /// </summary>
    /// <param name="atkObj">执行攻击的物体</param>
    /// <param name="beAtkCreature">被攻击的生物</param>
    public void ExecuteData(GameObject atkObj, CreatureCptBase beAtkCreature,
        out int damage)
    {
        damage = 0;
        float disKnockback = 0;
        float timeKnockback = 0;
        foreach (var itemDamgeData in dicDamageData)
        {
            switch (itemDamgeData.Key)
            {
                case AttributeTypeEnum.Damage:
                    damage = int.Parse(itemDamgeData.Value);
                    HandleForDamage(damage, atkObj, beAtkCreature);
                    break;
                case AttributeTypeEnum.KnockbackDis:
                    disKnockback = float.Parse(itemDamgeData.Value);
                    break;
                case AttributeTypeEnum.KnockbackTime:
                    timeKnockback = float.Parse(itemDamgeData.Value);
                    break;
            }
        }
        //击飞
        if (disKnockback != 0 && damage > 0)
            HandleForKnockback(disKnockback, timeKnockback, atkObj, beAtkCreature);
    }

    /// <summary>
    /// 处理-伤害
    /// </summary>
    public void HandleForDamage(int damage, GameObject atkObj, CreatureCptBase beAtkCreature)
    {
        //如果被攻击的是玩家
        CreatureTypeEnum creatureType = beAtkCreature.creatureData.GetCreatureType();
        if (creatureType == CreatureTypeEnum.Player)
        {
            //扣除伤害
            UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
            CharacterStatusBean characterStatus = userData.characterData.GetCharacterStatus();
            characterStatus.HealthChange(-damage);
        }
        //其他生物被攻击
        else
        {
            //扣除伤害
            beAtkCreature.creatureData.AddLife(-damage);
        }
    }

    /// <summary>
    /// 处理击退
    /// </summary>
    public void HandleForKnockback(float disKnockback, float timeKnockback, GameObject atkObj, CreatureCptBase beAtkCreature)
    {
        //如果进入了击飞的CD则无法被击飞
        if (beAtkCreature.creatureBattle.isHitFly)
            return;
        //如果有攻击的物体
        if (atkObj == null)
        {

        }
        else
        {
            beAtkCreature.creatureBattle.isHitFly = true;
            Vector3 hitDirection = (beAtkCreature.transform.position - atkObj.transform.position).normalized + Vector3.up * 0.1f;
            float timeCount = 0;
            //击退
            hitDirection *= disKnockback;

            CreatureTypeEnum creatureType = beAtkCreature.creatureData.GetCreatureType();
            //如果被攻击的是玩家
            if (creatureType == CreatureTypeEnum.Player)
            {
                //使用刚体位移
                GameControlHandler.Instance.manager.controlForPlayer.AddForce(hitDirection, ForceMode.Impulse);
                beAtkCreature.creatureBattle.isHitFly = false;
            }
            //其他生物被攻击
            else
            {
                //默认0.25s的击飞时间
                if (timeKnockback == 0)
                    timeKnockback = 0.25f;
                DOTween
                      .To(() => timeCount, data => { timeCount = data; }, timeKnockback, timeKnockback)
                      .OnUpdate(() =>
                      {
                          beAtkCreature.transform.position += (hitDirection * Time.fixedDeltaTime);
                      })
                      .OnComplete(() =>
                      {
                          beAtkCreature.creatureBattle.isHitFly = false;
                      });
            }
        }
    }
}
