using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

[Serializable]
public class ItemMetaMagicInstrument : ItemBaseMeta
{
    public List<ItemsBean> listMagicCore;

    //魔力值
    public int curMana = 0;
    public int mana = 0;

    /// <summary>
    /// 是否有足够的魔法
    /// </summary>
    /// <returns></returns>
    public bool HasEnoughMana(int targetMana)
    {
        if (curMana < targetMana)
        {
            return false;
        }
        else
        {
            return true;
        }
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