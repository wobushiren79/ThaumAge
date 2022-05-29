﻿using UnityEditor;
using UnityEngine;

public partial class UIViewCharacterStatusShow : BaseUIView
{
    protected CharacterStatusBean characterStatus;

    /// <summary>
    /// 设置数据
    /// </summary>
    public void SetData(CharacterStatusBean characterStatus)
    {
        this.characterStatus = characterStatus;
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        if (characterStatus == null)
            return;
        SetHealth(characterStatus.health, characterStatus.maxHealth);
        SetStamina(characterStatus.stamina, characterStatus.maxStamina);
        SetMagic(characterStatus.magic, characterStatus.maxMagic);
        SetSaturation(characterStatus.saturation, characterStatus.maxSaturation);
    }

    /// <summary>
    /// 设置生命值
    /// </summary>
    public void SetHealth(int health, int maxHealth)
    {
        ui_ViewCharacterStatusPro_Health.SetData(health, maxHealth);
    }

    /// <summary>
    /// 设置耐力值
    /// </summary>
    public void SetStamina(int stamina, int maxStamina)
    {
        ui_ViewCharacterStatusPro_Stamina.SetData(stamina, maxStamina);
    }

    /// <summary>
    /// 设置魔力值
    /// </summary>
    public void SetMagic(int magic, int maxMagic)
    {
        if (maxMagic == 0)
        {
            ui_ViewCharacterStatusPro_Magic.ShowObj(false);
        }
        else
        {
            ui_ViewCharacterStatusPro_Magic.ShowObj(true);
        }
        ui_ViewCharacterStatusPro_Magic.SetData(magic, maxMagic);
    }

    /// <summary>
    /// 设置饥饿值
    /// </summary>
    public void SetSaturation(int saturation, int maxSaturation)
    {
        ui_ViewCharacterStatusPro_Saturation.SetData(saturation, maxSaturation);
    }
}