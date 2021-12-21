using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class UIViewCharacterEquip : BaseUIView
{
    public Dictionary<EquipTypeEnum, UIViewItemContainer> dicEquip = new Dictionary<EquipTypeEnum, UIViewItemContainer>();

    public void Start()
    {
        UserDataBean userData = GameDataHandler.Instance.manager.GetUserData();
        dicEquip.Clear();
        dicEquip.Add(EquipTypeEnum.Hats, ui_Equip_Hats);
        dicEquip.Add(EquipTypeEnum.Gloves, ui_Equip_Gloves);
        dicEquip.Add(EquipTypeEnum.Clothes, ui_Equip_Clothes);
        dicEquip.Add(EquipTypeEnum.Shoes, ui_Equip_Shoes);
        dicEquip.Add(EquipTypeEnum.Headwear, ui_Equip_Headwear);
        dicEquip.Add(EquipTypeEnum.LeftRing, ui_Equip_LeftRing);
        dicEquip.Add(EquipTypeEnum.RightRing, ui_Equip_RightRing);
        dicEquip.Add(EquipTypeEnum.Cape, ui_Equip_Cape);

        foreach (var itemContainer in dicEquip)
        {
            ItemsBean itemData = userData.userEquip.GetEquipByType(itemContainer.Key);
            itemContainer.Value.SetLimitType(itemContainer.Key);
            itemContainer.Value.SetData(itemData);
            itemContainer.Value.SetHintText(UserEquipBean.GetEquipName(itemContainer.Key));
            itemContainer.Value.SetCallBackForSetViewItem(CallBackForSetEquip);
        }
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
    }

    /// <summary>
    /// 设置装备回调
    /// </summary>
    /// <param name="changeContainer"></param>
    /// <param name="itemId"></param>
    public void CallBackForSetEquip(UIViewItemContainer changeContainer, long itemId)
    {
        foreach (var itemContainer in dicEquip)
        {
            if (changeContainer == itemContainer.Value)
            {
                //更换装备
                Player player = GameHandler.Instance.manager.player;
                Character character = player.GetCharacter();
                character.characterEquip.ChangeEquip(itemContainer.Key, itemId);
            }
        }
    }

}