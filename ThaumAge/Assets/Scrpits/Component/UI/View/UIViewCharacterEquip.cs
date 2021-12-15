using UnityEditor;
using UnityEngine;

public partial class UIViewCharacterEquip : BaseUIView
{
    public override void RefreshUI()
    {
        base.RefreshUI();

        ui_Equip_Head.SetLimitType(ItemsTypeEnum.Hats);
        ui_Equip_Hand.SetLimitType(ItemsTypeEnum.Gloves);
        ui_Equip_Body.SetLimitType(ItemsTypeEnum.Clothes);
        ui_Equip_Foot.SetLimitType(ItemsTypeEnum.Shoes);

        ui_Equip_AccHead.SetLimitType(ItemsTypeEnum.Headwear);
        ui_Equip_AccLeftHead.SetLimitType(ItemsTypeEnum.Ring);
        ui_Equip_AccRightHead.SetLimitType(ItemsTypeEnum.Ring);
        ui_Equip_AccCape.SetLimitType(ItemsTypeEnum.Cape);

    }
}