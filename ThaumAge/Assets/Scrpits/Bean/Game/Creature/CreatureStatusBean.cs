using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class CreatureStatusBean
{
    //生命值
    public int curHealth = 100;
    public int health = 100;

    //耐力值
    public float curStamina = 10f;
    public int stamina = 10;

    //魔力值
    public int curMana = 0;
    public int mana = 0;

    //饥饿值
    public float curSaturation = 10;
    public int saturation = 10;

    //空气
    public float curAir = 10;
    public int air = 10;

    //物防
    public int def = 0;
    //魔防
    public int defMagic = 0;

    public int defMetal = 0;
    public int defWooden = 0;
    public int defWater = 0;
    public int defFire = 0;
    public int defEarth = 0;

    //攻击
    public int damage;
    public int damageMagic;

    //移动速度添加
    public float moveSpeedAdd;

    public List<CreatureStatusChangeBean> listStatusChange = new List<CreatureStatusChangeBean>();

    /// <summary>
    /// 处理状态改变
    /// </summary>
    public void HanleForStatusChange(float intervalTime)
    {
        moveSpeedAdd = 0;
        if (listStatusChange.IsNull())
            return;

        for (int i = 0; i < listStatusChange.Count; i++)
        {
            CreatureStatusChangeBean itemStatusChange = listStatusChange[i];
            CreatureStatusChangeTypeEnum creatureStatusChangeType = itemStatusChange.GetChangeType();
            switch (creatureStatusChangeType)
            {
                case CreatureStatusChangeTypeEnum.HealthAdd:
                    HealthChange((int)itemStatusChange.changeValue);
                    break;
                case CreatureStatusChangeTypeEnum.MoveSpeedAdd:
                    moveSpeedAdd = itemStatusChange.changeValue;
                    break;
            }
            if (itemStatusChange.time - intervalTime <= 0)
            {
                RemoveStatusChange(itemStatusChange);
                i--;
            }
            else
            {
                itemStatusChange.time -= intervalTime;
                listStatusChange[i] = itemStatusChange;
            }
        }
    }

    /// <summary>
    /// 移除状态改变
    /// </summary>
    public void RemoveStatusChange(CreatureStatusChangeBean creatureStatusChange)
    {
        //CreatureStatusChangeTypeEnum creatureStatusChangeType = creatureStatusChange.GetChangeType();
        //switch (creatureStatusChangeType)
        //{
        //    case CreatureStatusChangeTypeEnum.HealthAdd:
        //        break;
        //    case CreatureStatusChangeTypeEnum.MoveSpeedAdd:
        //        moveSpeedAdd = 0;
        //        break;
        //}
        listStatusChange.Remove(creatureStatusChange);
    }

    /// <summary>
    /// 添加状态改变
    /// </summary>
    public void AddStatusChange(CreatureStatusChangeBean creatureStatusChange)
    {
        for (int i = 0; i < listStatusChange.Count; i++)
        {
            CreatureStatusChangeBean itemStatusChange = listStatusChange[i];
            if (itemStatusChange.changType == creatureStatusChange.changType)
            {
                if(creatureStatusChange.time > itemStatusChange.time)
                {
                    itemStatusChange.time = creatureStatusChange.time;
                }
                return;
            }
        }
        listStatusChange.Add(creatureStatusChange);
    }

    /// <summary>
    /// 是否有足够的魔法
    /// </summary>
    /// <returns></returns>
    public bool HasEnoughMagic(int targetMagic)
    {
        if (curMana < targetMagic)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// 回复所有状态
    /// </summary>
    public void ReplyAllStatus()
    {
        curHealth = health;
        curStamina = stamina;
        curMana = mana;
        curSaturation = saturation;
        curAir = air;
    }

    /// <summary>
    /// 改变耐力
    /// </summary>
    /// <param name="changeData">修改的值</param>
    public bool StaminaChange(float changeData)
    {
        if (changeData > 0 && curStamina >= stamina)
        {
            return false;
        }
        if (changeData < 0 && curStamina <= 0)
        {
            return false;
        }
        curStamina += changeData;
        if (curStamina > stamina)
            curStamina = stamina;
        if (curStamina < 0)
            curStamina = 0;
        return true;
    }

    /// <summary>
    /// 改变饥饿值
    /// </summary>
    /// <returns></returns>
    public float SaturationChange(float changeData)
    {
        this.curSaturation += changeData;
        if (curSaturation > saturation)
        {
            curSaturation = saturation;
        }
        if (curSaturation < 0)
        {
            curSaturation = 0;
        }
        return curSaturation;
    }

    /// <summary>
    /// 改变生命值
    /// </summary>
    /// <param name="changeData"></param>
    /// <returns></returns>
    public int HealthChange(int changeData)
    {
        this.curHealth += changeData;
        if (curHealth > health)
        {
            curHealth = health;
        }
        if (curHealth < 0)
        {
            curHealth = 0;
        }
        return curHealth;
    }

    /// <summary>
    /// 改变魔法值
    /// </summary>
    /// <param name="changeData"></param>
    /// <returns></returns>
    public int ManaChange(int changeData)
    {
        this.curMana += changeData;
        if (curMana > mana)
        {
            curMana = mana;
        }
        if (curMana < 0)
        {
            curMana = 0;
        }
        return curMana;
    }

}
