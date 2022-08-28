using UnityEditor;
using UnityEngine;

public partial class UIViewCharacterStatusShow : BaseUIView
{
    protected CharacterBean characterData;

    public override void Awake()
    {
        base.Awake();
        EventHandler.Instance.RegisterEvent(EventsInfo.CharacterStatus_StatusChange, RefreshUI);
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    public void SetData(CharacterBean characterData)
    {
        this.characterData = characterData;
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        if (characterData == null)
            return;

        CharacterStatusBean characterStatus = characterData.characterStatus;

        SetHealth(characterStatus.curHealth, characterData.GetAttributeValue(AttributeTypeEnum.Health));
        SetStamina(Mathf.RoundToInt(characterStatus.curStamina), characterData.GetAttributeValue(AttributeTypeEnum.Stamina));
        SetMagic(characterStatus.curMagic, characterData.GetAttributeValue(AttributeTypeEnum.Magic));
        SetSaturation(Mathf.RoundToInt(characterStatus.curSaturation), characterData.GetAttributeValue(AttributeTypeEnum.Saturation));
        SetAir(Mathf.RoundToInt(characterStatus.curAir), characterData.GetAttributeValue(AttributeTypeEnum.Air));
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

    /// <summary>
    /// 设置氧气
    /// </summary>
    public void SetAir(int air, int maxAir)
    {
        ui_ViewCharacterStatusPro_Air.SetData(air, maxAir);
    }
}